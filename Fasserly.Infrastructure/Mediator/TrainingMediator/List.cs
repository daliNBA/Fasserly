using AutoMapper;
using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class List
    {
        public class Query : IRequest<List<TrainingDto>> { }
        public class Handler : BaseDataAccess, IRequestHandler<Query, List<TrainingDto>>
        {
            private readonly IMapper _mapper;

            public Handler(DbContextOptions<DatabaseContext> options, IMapper mapper) : base(options)
            {
                _mapper = mapper;
            }
            public async Task<List<TrainingDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var trainings = await context.Trainings.Where(x => x.IsActive)
                    .ToListAsync();

                return _mapper.Map<List<Training>, List<TrainingDto>>(trainings);
            }
        }
    }
}
