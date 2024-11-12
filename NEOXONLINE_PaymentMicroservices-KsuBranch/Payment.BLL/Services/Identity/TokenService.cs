using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payment.BLL.Contracts.Identity;
using Payment.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ILogger = Payment.BLL.Contracts;

namespace Payment.BLL.Contracts
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly TokenValidationParameters _tokenValidationParameters;


        public TokenService(ILogger<TokenService> logger, IConfiguration configuration, IOptionsMonitor<JwtBearerOptions> jwtBearerOptions)
        {
            _logger = logger;
            _configuration = configuration;

            // Получение параметров валидации через IOptionsMonitor
            var options = jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme);
            if (options != null)
            {
                _tokenValidationParameters = options.TokenValidationParameters;
            }
            else
            {
                throw new Exception("JwtBearerOptions are not configured properly.");
            }
        }

        public List<Claim> DecryptToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var list = jwtToken.Claims.ToList();
            return list;
        }

        public User GetUser(List<Claim> claimsList)
        {
            return new User
            {
                Id = Guid.Parse((claimsList.Where(claim => claim.Type.Equals("id")).First()).Value),
                FirstName = (claimsList.Where(claim => claim.Type.Equals("preferred_username")).First()).Value,
                Email = (claimsList.Where(claim => claim.Type.Equals("email")).First()).Value,
                Сountry = (claimsList.Where(claim => claim.Type.Equals("country")).First()).Value,
                Address = (claimsList.Where(claim => claim.Type.Equals("address")).First()).Value,
                PhoneNumber = (claimsList.Where(claim => claim.Type.Equals("phoneNumber")).First()).Value,
            };
        }
    }
}
