﻿using AutoMapper;
using Fasserly.Database.Entities;
using Fasserly.Infrastructure.Mediator.CategoryMediator;
using Fasserly.Infrastructure.Mediator.CommentMediator;
using Fasserly.Infrastructure.Mediator.TrainingMediator;
using System.Linq;

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
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.UserFasserly.DisplayName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.UserFasserly.Photos.FirstOrDefault(x => x.IsMain).Url))
                //Here we link the to class(bueyrDto and UserFasselry) with the current User to return if he is following
                .ForMember(d=>d.Following, o=>o.MapFrom<FollowingResolver>());
            CreateMap<Comment, CommentDto>()
               .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
               .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
               .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
