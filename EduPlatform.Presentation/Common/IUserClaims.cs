namespace EduPlatform.Presentation.Common
{
    public interface IUserClaims
    {
        string GetCurrentUserEmail();
        string GetCurrentUserId();
        List<string> GetUserRoles();
        int GetUserId();
    }
}
