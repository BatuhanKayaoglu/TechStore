using AutoMapper;
using EksiSozluk.Api.Application.Cache;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.Queries;
using EksİSozluk.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Queries.GetAllUser
{
    public class GetAllUserlQueryHandler: IRequestHandler<GetAllUserQuery, List<GetAllUserViewModel>>
    {

        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IRedisCacheService redisCacheService;

        public GetAllUserlQueryHandler(IUserRepository userRepository, IMapper mapper, IRedisCacheService redisCacheService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.redisCacheService = redisCacheService;
        }

        public async Task<List<GetAllUserViewModel>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            List<User> users = await redisCacheService.GetAllAsync(default);  
            if (users != null)
                return mapper.Map<List<GetAllUserViewModel>>(users);

            // if not found in cache, get from db   
            List<User> data = await userRepository.GetAll();
            var mappedUserList = mapper.Map<List<GetAllUserViewModel>>(data);
            return mappedUserList;  
        }   
    }
}
