namespace ServiceName.Core.Common.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateToken(int userId);
        bool ValidateToken(string token);
    }
}