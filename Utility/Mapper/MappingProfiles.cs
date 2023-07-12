using AutoMapper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Utility.Mapper
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles() 
        {
        CreateMap<Pokemon, PokemonDto>();
        CreateMap<PokemonDto, Pokemon>();
        CreateMap<Category, CategoryDto>();
        CreateMap<Owner,OwnerDto>();
        CreateMap<OwnerDto, Owner>();
        CreateMap<Country,CountryDto>();
        CreateMap<CountryDto, Country>();
        CreateMap<Review, ReviewDto>();
        CreateMap<Review, ReviewDto2>();
        CreateMap<Reviewer, ReviewerDto>();
        CreateMap<CategoryDto, Category>();
        }    
    }
}
