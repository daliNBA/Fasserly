using AutoMapper;
using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Details
    {
        public class Query : IRequest<TrainingDto>
        {
            public Query(Guid id)
            {
                Id = id;
            }
            public Guid Id { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Query, TrainingDto>
        {
            private readonly IMapper _mapper;

            public Handler(DbContextOptions<DatabaseContext> options, IMapper mapper) : base(options)
            {
                _mapper = mapper;
            }
            public async Task<TrainingDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var training = await context.Trainings
                    .FindAsync(request.Id);

                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                var returnedTraining = _mapper.Map<Training, TrainingDto>(training);
                return returnedTraining;
            }
        }
    }
}
