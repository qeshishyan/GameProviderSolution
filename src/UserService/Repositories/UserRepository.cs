using UserService.Entities;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<string, User> _users;
        public UserRepository()
        {
            _users = new Dictionary<string, User>
            {
                { "HFSA548-SDA544848ADS-ffght8DQ.AsdSDAS4748Q1246978", new User {Avatar = "", UserName = "User111" } },
                { "HFSA548-SDA544848ADS-51238DQ.ASDASDasd4748Q1246978", new User {Avatar = "", UserName = "User457" } },
                { "123eA548-SDA544848ADS-54AS8DQ.ASDASDAS4748Q1246978", new User {Avatar = "", UserName = "User484" } },
                { "HFSA548-SDA341sa48ADS-54122DQ.ASDA213dssaqS4748Q1246978", new User {Avatar = "", UserName = "User411" } },
                { "HFSA548-SDA5bvcDS-5hetwerqeQ.ASDASDAS4748Q1246978", new User {Avatar = "", UserName = "User784" } },
                { "HFSA548-SDAsadq18ADS-5134S8DQ.ASDwer4748Q1246978", new User {Avatar = "", UserName = "User685" } },
                { "HFSA548-SDA544848ADS-54AS8DQ.ASDASDAS4748Q1246978", new User {Avatar = "", UserName = "User134" } },
            };
        }
        public async Task<UserResponse> GetUser(GetUserRequest request)
        {
            if(_users.TryGetValue(request.Token!, out User? user))
            {
                return new UserResponse
                {
                    Avatar = user.Avatar,
                    UserName = user.UserName
                };
            }
            throw new NullReferenceException("User not found!");
        }
    }
}
