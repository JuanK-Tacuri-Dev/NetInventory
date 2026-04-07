namespace NetInventory.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string GetCurrentUser();
    string GetCurrentUserId();
}
