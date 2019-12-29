using AutoMapper;
using Neodenit.ActiveReader.Common.DataModels;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleViewModel>().ReverseMap();
        }
    }
}
