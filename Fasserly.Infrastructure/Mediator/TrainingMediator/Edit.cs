using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
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
            public string Title { get; set; }
            public string Description { get; set; }
            public string Language { get; set; }
            public int? Rating { get; set; }
            public string Price { get; set; }
            public bool IsActive { get; set; }
            public string Category { get; set; }
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
                    //RuleFor(x => x.Price).Matches("^[0-9]([.,][0-9]{1,3})?$");
                }
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var training = await context.Trainings.FindAsync(request.TrainingId);
                var cat = await context.Categories.Where(x => x.Label == request.Category).FirstOrDefaultAsync();

                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                //Edit object
                training.IsActive = request.IsActive;
                training.Title = request.Title ?? training.Title;
                training.Description = request.Description ?? training.Description;
                training.Price = Convert.ToDecimal(request.Price, new CultureInfo("en-US"));
                training.UpdateDate = DateTime.Now;
                training.Category = cat;
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;

                throw new Exception("Saving problem");
            }
        }
    }
}
