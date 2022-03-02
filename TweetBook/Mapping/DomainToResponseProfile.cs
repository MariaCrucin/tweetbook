using AutoMapper;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;

namespace TweetBook.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>();

            CreateMap<Tag, TagResponse>();

            CreateMap<PostTag, TagResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TagName));
        }
    }
}
