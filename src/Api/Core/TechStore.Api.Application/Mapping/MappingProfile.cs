using AutoMapper;
using TechStore.Common.ViewModels.Queries;
using TechStore.Common.ViewModels.RequestModels;
using TechStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, LoginUserViewModel>().ReverseMap();
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();
            CreateMap<Entry, CreateEntryCommand>().ReverseMap();
            CreateMap<Entry, GetEntryDetailViewModel>().ReverseMap();
            CreateMap<EntryComment, CreateEntryCommentCommand>().ReverseMap();
            CreateMap<User, UserDetailViewModel>().ReverseMap();
            CreateMap<Entry, GetEntriesViewModel>().ForMember(x => x.CommentCount, y => y.MapFrom(z => z.EntryComments.Count));
            CreateMap<User, GetAllUserViewModel>().ReverseMap();
        }
    }
}
