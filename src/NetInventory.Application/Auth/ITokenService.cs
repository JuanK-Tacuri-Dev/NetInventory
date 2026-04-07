namespace NetInventory.Application.Auth;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(string userId, string email);
}
