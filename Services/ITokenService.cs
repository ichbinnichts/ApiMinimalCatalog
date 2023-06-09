using ApiMinimalCatalog.Models;

namespace ApiMinimalCatalog.Services
{
    public interface ITokenService
    {
        public string GenerateToken(string key, string issuer, UserModel user);
    }
}
