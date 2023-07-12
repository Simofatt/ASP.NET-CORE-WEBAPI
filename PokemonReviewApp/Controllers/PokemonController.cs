using AutoMapper;
using DAO2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Numerics;
using Utility;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonRepository : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PokemonRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetPokemons))]
        [ProducesResponseType(200, Type = typeof(IQueryable<PokemonDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemons()
        {

            var pokemons = _mapper.Map<List<PokemonDto>>(_unitOfWork.Pokemon.GetAll().ToList());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (pokemons is null || pokemons.Count() == 0)
                return NotFound();
            return Ok(pokemons);


        }

        [HttpGet(nameof(GetPokemonById)+"/{Id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonById(int Id)
        {
            if (!_unitOfWork.Pokemon.EntityExists(Id))
            {
                return NotFound();
            }
            var pokemon = _mapper.Map<PokemonDto>(_unitOfWork.Pokemon.Get(p => p.Id == Id));
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(pokemon);
        }


        [HttpPost(nameof(CreatePokemon))]
        [ProducesResponseType(200)] // Ok
        [ProducesResponseType(400)]  //Bad Request
        [ProducesResponseType(404)] // NotFound
        [ProducesResponseType(402)] // 
        [ProducesResponseType(500)] //undocumented
        public IActionResult CreatePokemon([FromQuery] int OwnerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemon)
        {
            if (OwnerId != 0 && categoryId != 0 && pokemon is not null)
            {
                var OwnerCheck = _unitOfWork.Owner.EntityExists(OwnerId);
                var CategoryCheck = _unitOfWork.Category.ItExists(categoryId);
                if (!OwnerCheck || !CategoryCheck)
                {
                    ModelState.AddModelError("", "Owner or Category was not found!");
                    return StatusCode(402, ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var pokemonCheck = _unitOfWork.Pokemon.GetAll().Where(p => p.Name.Trim().ToLower() == pokemon.Name.ToLower()).FirstOrDefault();
                if (pokemonCheck is not null)
                {
                    ModelState.AddModelError("", "Pokemon already exists!");
                    return StatusCode(402, ModelState);
                }
                Pokemon pokemonMap = _mapper.Map<Pokemon>(pokemon);
                _unitOfWork.Pokemon.AddPokemonData(OwnerId, categoryId, pokemonMap);


                bool save = _unitOfWork.Save();
                if (!save)
                {
                    ModelState.AddModelError("", "Problem with saving!");
                    return StatusCode(402, ModelState);
                }
            }

            return Ok("Pokemon has been created");

        }
        [HttpDelete(nameof(DeletePokemon)+"/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
       
        public IActionResult DeletePokemon(int id)
        {
            var pokemonCheck = _unitOfWork.Pokemon.EntityExists(id);
            if(!pokemonCheck)
            {
                return NotFound(); 
            }
            var Pokemon = _unitOfWork.Pokemon.Get(p => p.Id == id); 
            if(!ModelState.IsValid)
            {
                return BadRequest(); 
            }
            _unitOfWork.Pokemon.Remove(Pokemon);
            bool save = _unitOfWork.Save(); 
            if(!save)
            {
                ModelState.AddModelError("", "Problem with saving");
                return StatusCode(402,ModelState);  
            }


            return NoContent();
        }
    }
}
