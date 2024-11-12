using Payment.Domain.Identity;
using System.Security.Claims;

namespace Payment.BLL.Contracts.Identity
{
    public interface ITokenService : IService
    {
        List<Claim> DecryptToken(string token);
        User GetUser(List<Claim> claimsList);
    }
}
