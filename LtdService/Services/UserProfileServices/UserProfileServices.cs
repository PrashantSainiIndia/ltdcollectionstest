using LtdDomain.Models.UserProfile;
using LtdService.CommonServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdService.Services.UserProfileServices
{
    public class UserProfileServices : IUserProfileService, IUserProfileRepository
    {
        private IUserProfileRepository _userProfileRepository;
        private IModelDataAnnotationCheck _modelDataAnnotationCheck;

        public UserProfileServices(IUserProfileRepository userProfileRepository, IModelDataAnnotationCheck modelDataAnnotationCheck)
        {
            _userProfileRepository = userProfileRepository;
            _modelDataAnnotationCheck = modelDataAnnotationCheck;
        }

        public void Add(IUserProfileModel userProfileModel)
        {
            _userProfileRepository.Add(userProfileModel);
        }
        public void Update(IUserProfileModel userProfileModel)
        {
            _userProfileRepository.Update(userProfileModel);
        }

        public void Delete(IUserProfileModel userProfileModel)
        {
            _userProfileRepository.Update(userProfileModel);
        }

        public IEnumerable<UserProfileModel> GetAll()
        {
            return _userProfileRepository.GetAll();
        }

        public UserProfileModel GetById(int id)
        {
            return _userProfileRepository.GetById(id);
        }

        public void ValidateModelDataAnnotations(IUserProfileModel userProfileModel)
        {
            _modelDataAnnotationCheck.ValidateModelDataAnnotations(userProfileModel);
        }
    }
}
