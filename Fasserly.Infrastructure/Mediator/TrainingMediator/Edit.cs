using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid TrainingId { get; set; }
            public bool IsActive { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public DateTime DateOfCreation { get; set; }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }
            public class TrainingValidation : AbstractValidator<Command>
            {
                public TrainingValidation()
                {
                    RuleFor(x => x.Title).NotEmpty();
                    RuleFor(x => x.Description).NotEmpty();
                }
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var training = await context.Trainings.FindAsync(request.TrainingId);

                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                //Edit object
                training.IsActive = request.IsActive;
                training.Title = request.Title ?? training.Title;
                training.Description = request.Description ?? training.Description;
                training.Price = request.Price;
                training.DateOfCreation = request.DateOfCreation;

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }
    }
}
