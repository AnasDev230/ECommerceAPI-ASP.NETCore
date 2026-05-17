using Microsoft.AspNetCore.Identity;

namespace ECommerceAPI_ASP.NETCore.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
