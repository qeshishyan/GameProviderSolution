using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<UserResponse> GetUser(GetUserRequest request);
        Task<UserBalanceResponse> GetUserBalance(GetUserRequest request);
    }
}