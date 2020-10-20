using AutoMapper;
using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.DataAccess;
using Fasserly.Infrastructure.Error;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fasserly.Infrastructure.Mediator.CommentMediator
{

    public class Create
    {
        public class Command : IRequest<CommentDto>
        {
            public string Body { get; set; }
            public Guid TrainingId { get; set; }
            public string Username { get; set; }
        }

        public class TrainingValidation : AbstractValidator<Command>
        {
            public TrainingValidation()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : BaseDataAccess, IRequestHandler<Command, CommentDto>
        {
            private readonly IMapper _mapper;

            public Handler(DbContextOptions<DatabaseContext> options, IMapper mapper) : base(options)
            {
                _mapper = mapper;
            }

            public async Task<CommentDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var training = await context.Trainings.FindAsync(request.TrainingId);
                if (training == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });
                var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);
                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { training = "Not found" });

                var comment = new Comment
                {
                    Body = request.Body,
                    Author = user,
                    Training = training,
                    DateOfComment = DateTime.Now,
                };

                training.comments.Add(comment);

                context.Add(comment);

                var success = await context.SaveChangesAsync() > 0;
                if (success) return _mapper.Map<CommentDto>(comment);

                throw new Exception("Saving commetn problem");
            }
        }
    }
}
