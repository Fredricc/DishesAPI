using AutoMapper;
using DishesAPI.Entities;
using DishesAPI.Models;

namespace DishesAPI.Profiles
{
    public class IngretientProfile : Profile
    {
        public IngretientProfile()
        {
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(
                d => d.DishId,
                o => o.MapFrom(s => s.Dishes.First().Id));
        }
    }
}
