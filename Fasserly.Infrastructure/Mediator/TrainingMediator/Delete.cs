using Fasserly.Database;
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
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)

            {
                var training = await context.Trainings.FindAsync(request.Id);
                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                context.Trainings.Remove(training);
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Deleting problem");
            }
        }
    }
}
