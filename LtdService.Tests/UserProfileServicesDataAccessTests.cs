using LtdService.Services.UserProfileServices;
using LtdInfrastructure.DataAccess.Repositories.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using LtdService.CommonServices;
using LtdDomain.Models.UserProfile;
using CommonComponents;
using Newtonsoft.Json.Linq;

namespace LtdService.Tests
{
    [Trait("Category", "Data Access Validations")]
    public class UserProfileServicesDataAccessTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private UserProfileServices _userProfileServices;
        private string _connectionString;

        public UserProfileServicesDataAccessTests(ITestOutputHelper testOutputHelper)
        {
            _connectionString = "Data Source= ..";
            _testOutputHelper = testOutputHelper;
            _userProfileServices = new UserProfileServices(new UserProfileRepository(_connectionString), new ModelDataAnnotationCheck());
        }

        [Fact]
        public void ShouldReturnListOfUserProfiles()
        {
            List<UserProfileModel> userProfileModelList = (List<UserProfileModel>)_userProfileServices.GetAll();

            Assert.NotEmpty(userProfileModelList);

            foreach(UserProfileModel up in userProfileModelList)
            {
                _testOutputHelper.WriteLine($"UserId: {up.UserId}\nPassword: {up.Password}\n");
            }
        }

        [Fact]
        public void ShouldReturnUserProfileMatchingId()
        {
            UserProfileModel userProfileModel = null;
            int idToGet = 1;
            try
            {
                userProfileModel = _userProfileServices.GetById(idToGet);
            }
            catch(DataAccessException dae)
            {
                _testOutputHelper.WriteLine(dae.DataAccessStatusInfo.GetFormattedValues());
            }

            Assert.True(userProfileModel != null);
            Assert.True(idToGet.ToString() == userProfileModel.UserId);

            if(userProfileModel != null)
            {
                string userProfileModelJsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(userProfileModel);
                string formattedJsonStr = JToken.Parse(userProfileModelJsonStr).ToString();
                _testOutputHelper.WriteLine(formattedJsonStr);
            }
        }

        [Fact]
        public void ShouldReturnSuccessForAdd()
        {
            UserProfileModel up = new UserProfileModel() { UserId = "Unit Test 01", Password = "UnitTest01" };

            bool operationSucceeded = false;
            string dataAccessStatusJsonStr = string.Empty;
            string formattedJsonStr = string.Empty;

            try
            {
                _userProfileServices.Add(up);
                operationSucceeded = true;
            }
            catch(DataAccessException dae)
            {
                operationSucceeded = dae.DataAccessStatusInfo.OperationSucceeded;
                dataAccessStatusJsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dae.DataAccessStatusInfo);
                formattedJsonStr = JToken.Parse(dataAccessStatusJsonStr).ToString();
            }

            try
            {
                Assert.True(operationSucceeded);
                _testOutputHelper.WriteLine("The record was successfully added");
            }
            finally
            {
                _testOutputHelper.WriteLine(formattedJsonStr);
            }
        }

        [Fact]
        public void ShouldReturnSuccessForUpdate()
        {
            UserProfileModel up = new UserProfileModel() { UserId = "Unit Test 01", Password = "UnitTest@1234" };

            bool operationSucceeded = false;
            string dataAccessStatusJsonStr = string.Empty;
            string formattedJsonStr = string.Empty;

            try
            {
                _userProfileServices.Update(up);
                operationSucceeded = true;
            }
            catch (DataAccessException dae)
            {
                operationSucceeded = dae.DataAccessStatusInfo.OperationSucceeded;
                dataAccessStatusJsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dae.DataAccessStatusInfo);
                formattedJsonStr = JToken.Parse(dataAccessStatusJsonStr).ToString();
            }

            try
            {
                Assert.True(operationSucceeded);
                _testOutputHelper.WriteLine("The record was successfully updated");
            }
            finally
            {
                _testOutputHelper.WriteLine(formattedJsonStr);
            }
        }

        [Fact]
        public void ShouldReturnSuccessForDelete()
        {
            UserProfileModel up = new UserProfileModel() { UserId = "Unit Test 01", Password = "UnitTest01" };

            bool operationSucceeded = false;
            string dataAccessStatusJsonStr = string.Empty;
            string formattedJsonStr = string.Empty;

            try
            {
                _userProfileServices.Delete(up);
                operationSucceeded = true;
            }
            catch (DataAccessException dae)
            {
                operationSucceeded = dae.DataAccessStatusInfo.OperationSucceeded;
                dataAccessStatusJsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(dae.DataAccessStatusInfo);
                formattedJsonStr = JToken.Parse(dataAccessStatusJsonStr).ToString();
            }

            try
            {
                Assert.True(operationSucceeded);
                _testOutputHelper.WriteLine("The record was successfully deleted");
            }
            finally
            {
                _testOutputHelper.WriteLine(formattedJsonStr);
            }
        }
    }
}
