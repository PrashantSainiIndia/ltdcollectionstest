using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LtdDomain.Models.UserProfile
{
    public class UserProfileModel : IUserProfileModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Id is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "User Id length must be between 2 and 20")]
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
