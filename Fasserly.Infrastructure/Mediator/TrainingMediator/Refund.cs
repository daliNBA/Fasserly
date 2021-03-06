﻿using Fasserly.Database;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using Fasserly.Infrastructure.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.TrainingMediator
{
    public class Refund
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var training = await context.Trainings.FindAsync(request.Id);
                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUserName());

                var buy = await context.UserTrainings.SingleOrDefaultAsync(x => x.TrainingId == training.TrainingId && x.UserFasserlyId == user.Id);
                if (buy == null)
                    return Unit.Value;
                if (buy.IsOwner)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "you cannot remove your own courses" });

                context.Remove(buy);
                var success = await context.SaveChangesAsync() > 0;
                if (success) return Unit.Value;
                throw new Exception("Saving problem");
            }
        }

    }
}
