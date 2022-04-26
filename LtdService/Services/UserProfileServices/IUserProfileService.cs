using LtdDomain.Models.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdService.Services.UserProfileServices
{
    public interface IUserProfileService
    {
        void ValidateModelDataAnnotations(IUserProfileModel userProfileModel);
    }
}
