namespace Settings.Application.Common.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateToken(int userId);
        bool ValidateCurrentToken(string token);
    }
}