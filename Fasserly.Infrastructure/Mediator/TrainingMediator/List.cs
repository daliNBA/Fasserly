using AutoMapper;
using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class List
    {
        public class TrainingEnvelope
        {
            public List<TrainingDto> Trainings { get; set; }
            public int TrainingCount { get; set; }
        }
        public class Query : IRequest<TrainingEnvelope>
        {
            public int? Limit { get; set; }
            public int? OffSet { get; set; }
            public bool IsBuyer { get; }
            public bool IsOwner { get; }
            public DateTime? StartDate { get; }

            public Query(int? limit, int? offSet, bool isBuyer, bool isOwner, DateTime? startDate)
            {
                Limit = limit;
                OffSet = offSet;
                IsBuyer = isBuyer;
                IsOwner = isOwner;
                StartDate = startDate ?? DateTime.Now.AddDays(100);
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Query, TrainingEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DbContextOptions<DatabaseContext> options, IMapper mapper, IUserAccessor userAccessor) : base(options)
            {
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<TrainingEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Trainings
                    .Where(t => t.IsActive )
                    .OrderBy(x => x.DateOfCreation)
                    .AsQueryable();

                if (request.IsBuyer && !request.IsOwner)
                {
                    query = query.Where(x => x.UserTrainings.Any(a => a.UserFasserly.UserName == _userAccessor.GetCurrentUserName()));
                }

                if (request.IsOwner && !request.IsBuyer)
                {
                    query = query.Where(x => x.UserTrainings.Any(a => a.UserFasserly.UserName == _userAccessor.GetCurrentUserName() && a.IsOwner));
                }

                var trainings = await query.Skip(request.OffSet ?? 0)
                    .Take(request.Limit ?? 3)
                    .ToListAsync();
                return new TrainingEnvelope
                {
                    Trainings = _mapper.Map<List<Training>, List<TrainingDto>>(trainings),
                    TrainingCount = query.Count(),
                };
            }
        }
    }
}
