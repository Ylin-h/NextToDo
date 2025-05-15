using AutoMapper;
using NextWebApi.DTOs;
using NextWebApi.Models;

namespace NextWebApi.Utils
{
    public class AutoMapperSettings: Profile
    {
        public AutoMapperSettings()
        {
            CreateMap<AccountDTO, Account>().ReverseMap();

        }
    }
}
