using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Mediator.ProfileMediator;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class ListTraining
    {
        public class Query : IRequest<List<UserTrainingDto>>
        {
            public string Username { get; set; }
            public string Predicate { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Query, List<UserTrainingDto>>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

            public async Task<List<UserTrainingDto>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = "Not found" });

                var queryable = user.UserTrainings
                    .OrderBy(a => a.Training.DateOfCreation)
                    .AsQueryable();

                switch (request.Predicate)
                {
                    case "past":
                        queryable = queryable.Where(a => a.Training.DateOfCreation <= DateTime.Now);
                        break;
                    case "owning":
                        queryable = queryable.Where(a => a.IsOwner);
                        break;
                    default:
                        queryable = queryable.Where(a => a.Training.DateOfCreation >= DateTime.Now);
                        break;
                }

                var trainings = queryable.ToList();
                var trainingsToReturn = new List<UserTrainingDto>();

                foreach (var training in trainings)
                {
                    var UserTraining = new UserTrainingDto
                    {
                        Id = training.Training.TrainingId,
                        Title = training.Training.Title,
                        Category = training.Training.Category?.Label,
                        Date = training.Training.DateOfCreation
                    };

                    trainingsToReturn.Add(UserTraining);
                }

                return trainingsToReturn;
            }
        }
    }
}
