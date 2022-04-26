namespace LtdDomain.Models.UserProfile
{
    public interface IUserProfileModel
    {
        string Password { get; set; }
        string UserId { get; set; }
    }
}