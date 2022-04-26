using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using LtdService.CommonServices;

namespace LtdService.Tests
{
    [Trait("Category", "Model Validations")]
    public class UserProfileValidationTests : IClassFixture<UserProfileServicesFixture>
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private UserProfileServicesFixture _userProfileServiceFixture;

        public UserProfileValidationTests(UserProfileServicesFixture userProfileServiceFixture, ITestOutputHelper testOutputHeper)
        {
            this._userProfileServiceFixture = userProfileServiceFixture;
            _testOutputHelper = testOutputHeper;

            SetValidSampleValues();
        }


        public void ShouldNotThrowExceptionForDefaultTestValuesOnAnnotations()
        {
            var exception =
                Record.Exception(() => _userProfileServiceFixture.UserProfileServices.ValidateModelDataAnnotations
                (_userProfileServiceFixture.UserProfileModel));

            Assert.Null(exception);

            WriteExceptionTestResult(exception);
        }

        public void ShouldThrowExceptionIfAnyDataAnnotationCheckFails()
        {
            _userProfileServiceFixture.UserProfileModel.UserId = "1";
            _userProfileServiceFixture.UserProfileModel.Password = "1";

            Exception exception = 
                Assert.Throws<ArgumentException>(testCode: () => _userProfileServiceFixture.UserProfileServices.ValidateModelDataAnnotations
                (_userProfileServiceFixture.UserProfileModel));

            WriteExceptionTestResult(exception);
        }

        public void ShouldThrowExceptionForUserIdPasswordEmpty()
        {
            _userProfileServiceFixture.UserProfileModel.UserId = "";
            _userProfileServiceFixture.UserProfileModel.Password = "";

            Exception exception =
                Assert.Throws<ArgumentException>(testCode: () => _userProfileServiceFixture.UserProfileServices.ValidateModelDataAnnotations
                (_userProfileServiceFixture.UserProfileModel));

            WriteExceptionTestResult(exception);
        }

        private void SetValidSampleValues()
        {
            _userProfileServiceFixture.UserProfileModel.UserId = "admin";
            _userProfileServiceFixture.UserProfileModel.Password = "admin@123";
        }

        private void WriteExceptionTestResult(Exception exception)
        {
            if(exception != null)
            {
                _testOutputHelper.WriteLine(exception.Message);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                JObject json = JObject.FromObject(_userProfileServiceFixture.UserProfileModel);
                stringBuilder.Append("***** No Exception Was Thrown *****").AppendLine();
                foreach(JProperty jProperty in json.Properties())
                {
                    stringBuilder.AppendLine(jProperty.Name).Append(" --> ").Append(jProperty.Value).AppendLine();
                }
                _testOutputHelper.WriteLine(stringBuilder.ToString());
            }
        }
    }
}
