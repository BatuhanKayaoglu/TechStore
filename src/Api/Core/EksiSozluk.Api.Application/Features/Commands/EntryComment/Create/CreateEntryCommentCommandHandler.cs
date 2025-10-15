using AutoMapper;
using EksiSozluk.Api.Application.Repositories;
using EksiSozluk.Common.ViewModels.RequestModels;
using EksİSozluk.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Features.Commands.EntryComment.Create
{
    public class CreateEntryCommentCommandHandler : IRequestHandler<CreateEntryCommentCommand, Guid>
    {
        private readonly IEntryCommentRepository entryCommentRepository;
        private readonly IMapper mapper;

        public CreateEntryCommentCommandHandler(IEntryCommentRepository entryCommentRepository, IMapper mapper)
        {
            this.entryCommentRepository = entryCommentRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEntryCommentCommand request, CancellationToken cancellationToken)
        {
            var dbEntryComment = mapper.Map<EksİSozluk.Domain.Models.EntryComment>(request);
            await entryCommentRepository.AddAsync(dbEntryComment);

            return dbEntryComment.Id;   
        }
    }
}
