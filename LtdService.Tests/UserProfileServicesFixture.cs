using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LtdService.Services.UserProfileServices;
using LtdDomain.Models.UserProfile;
using LtdService.CommonServices;

namespace LtdService.Tests
{
    public class UserProfileServicesFixture
    {
        private IUserProfileService _userProfileService;
        private IUserProfileModel _userProfileModel;

        public UserProfileServicesFixture()
        {
            _userProfileService = new UserProfileServices(null, new ModelDataAnnotationCheck());
            _userProfileModel = new UserProfileModel();
        }

        public UserProfileServices UserProfileServices
        {
            get { return (UserProfileServices)_userProfileService; }
            set { _userProfileService = value; }
        }

        public UserProfileModel UserProfileModel
        {
            get { return (UserProfileModel)_userProfileModel; }
            set { _userProfileModel = value; }
        }
    }
}
