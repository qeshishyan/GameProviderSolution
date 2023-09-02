using UserService.Models;
using UserService.Repositories;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponse> GetUser(GetUserRequest request)=>
           await _userRepository.GetUser(request);
    }
}
