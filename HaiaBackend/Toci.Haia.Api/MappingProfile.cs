using static System.Runtime.InteropServices.JavaScript.JSType;
using Toci.Haia.Database.Persistence;
using AutoMapper;

namespace Toci.Haia.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ComedyTextDto, ComedyText>();
            CreateMap<CommentDto, Comment>();
            CreateMap<ComedyText, ComedyTextDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Like, LikeDTO>();
            CreateMap<LikeDTO, Like>();
        }
    }

}
