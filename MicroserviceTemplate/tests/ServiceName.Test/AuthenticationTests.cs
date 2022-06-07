using ServiceName.Infrastructure.Authentication;

namespace ServiceName.Test
{
    public class AuthenticationTests
    {
        [Fact]
        public void GenerateAndValidateJwtToken()
        {
            MockJwtAuthenticationService authService = new MockJwtAuthenticationService();
            var token = authService.GenerateToken(1);
            var isValid = authService.ValidateToken(token);
        }

        [Fact]
        public void ValidateJWT()
        {

        }
    }
}