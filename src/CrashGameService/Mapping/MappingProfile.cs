using AutoMapper;
using CrashGameService.Entities;
using CrashGameService.Models;

namespace CrashGameService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BetRequest, Bet>();
        }
    }
}
