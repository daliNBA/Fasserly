using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Create
    {
        public class Command : IRequest
        {
            public Guid TrainingId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public bool IsActive { get; set; }
            public DateTime DateOfCreation { get; set; }
        }

        public class TrainingValidation : AbstractValidator<Command>
        {
            public TrainingValidation()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).Password();
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            public Handler(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var training = new Training
                {
                    TrainingId = request.TrainingId,
                    Title = request.Title,
                    IsActive = request.IsActive,
                    DateOfCreation = request.DateOfCreation,
                    Description = request.Description,
                    Price = request.Price
                };
                context.Trainings.Add(training);
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }
    }
}
