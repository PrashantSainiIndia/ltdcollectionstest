using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LtdDomain;
using LtdDomain.Models.UserProfile;

namespace LtdService.Services.UserProfileServices
{
    public interface IUserProfileRepository
    {
        IEnumerable<UserProfileModel> GetAll();
        UserProfileModel GetById(int id);
        void Add(IUserProfileModel userProfileModel);
        void Update(IUserProfileModel userProfileModel);
        void Delete(IUserProfileModel userProfileModel);
    }
}
