using AutoMapper;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Mediator.CategoryMediator;
using Fasserly.Infrastructure.Mediator.TrainingMediator;
using Fasserly.Infrastructure.Mediator.UserMediator;

namespace Fasserly.Infrastructure.Mediator
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Training, TrainingDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<UserTraining, BuyerDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.UserFasserly.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.UserFasserly.DisplayName));
        }
    }
}
