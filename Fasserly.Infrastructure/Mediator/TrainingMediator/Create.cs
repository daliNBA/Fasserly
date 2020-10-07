using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using Fasserly.Infrastructure.DataAccess;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Create
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

        public class TrainingValidation : AbstractValidator<Command>
        {
            public TrainingValidation()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                //RuleFor(x => x.Price).Matches("^[0-9]([.,][0-9]{1,3})?$");
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command>
        {
            private readonly IUserAccessor _userAccessor;

            public Handler(DbContextOptions<DatabaseContext> options, IUserAccessor userAccessor) : base(options)
            {
                _userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());
                var cat = await context.Categories.SingleOrDefaultAsync(x => x.Label == request.Category);

                var training = new Training
                {
                    TrainingId = request.TrainingId,
                    Title = request.Title,
                    IsActive = request.IsActive,
                    DateOfCreation = DateTime.Now,
                    Description = request.Description,
                    Price = Convert.ToDecimal(request.Price, new CultureInfo("en-US")),
                    Language = request.Language,
                    category = cat,
                };
                context.Trainings.Add(training);

                var buy = new UserTraining
                {
                    UserFasserly = user,
                    Training = training,
                    DateJoined = DateTime.Now,
                    IsOwner = true,
                };

                context.Add(buy);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }
    }
}
