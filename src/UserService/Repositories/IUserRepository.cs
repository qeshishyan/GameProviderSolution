﻿using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        Task<UserResponse> GetUser(GetUserRequest request);
    }
}
