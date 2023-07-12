using AutoMapper;
using DAO.Data;
using DAO2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Utility;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {

        private readonly IMapper _mapper; 
        private IUnitOfWork _unitOfWork;    

        public OwnerController (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper; 
        }
        [HttpGet(nameof(GetOwners))]
        [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_unitOfWork.Owner.GetAll("Country").ToList());
            if (owners is null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owners); 
        }

        [HttpGet(nameof(GetOwnerById)+"/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(OwnerDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwnerById(int ownerId)
        {
            var owner = _mapper.Map<OwnerDto>(_unitOfWork.Owner.Get(o => o.Id == ownerId));
            if (owner is null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

          [HttpGet(nameof(GetOwnerOfAPokemon)+"{pokemonId}")]
          [ProducesResponseType(200, Type = typeof(List<OwnerDto>))]
          [ProducesResponseType(400)]
          [ProducesResponseType(404)]
          public IActionResult GetOwnerOfAPokemon(int pokemonId) {

              var owner = _mapper.Map<List<OwnerDto>>(_unitOfWork.Owner.GetOwnerOfAPokemon(pokemonId).ToList());
            if (!_unitOfWork.Pokemon.EntityExists(pokemonId)) 
                  return NotFound();
              if (!ModelState.IsValid)
                  return BadRequest(ModelState);

              return Ok(owner);
          }
        
        
        [HttpGet(nameof(GetPokemonByOwner)+"/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(List<PokemonDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public IActionResult GetPokemonByOwner(int ownerId)
        {

            var owner = _mapper.Map<List<PokemonDto>>(_unitOfWork.Owner.GetPokemonByOwner(ownerId).ToList());
            if (owner == null || owner.Count == 0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }


        [HttpPost(nameof(CreateOwner))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateOwner([FromQuery] int CountryId,[FromBody] OwnerDto owner )
        {
            if(owner is null) 
                return BadRequest(ModelState);  
            var ownerCheck = _unitOfWork.Owner.GetAll().Where(o=>o.FirstName.Trim().ToLower() ==  owner.FirstName.Trim().ToLower()).FirstOrDefault();
            var ownerIdCheck = _unitOfWork.Owner.EntityExists(owner.Id);
            if(ownerCheck is not null )
            {
                ModelState.AddModelError("","Owner Already exists");
                return StatusCode(422,ModelState) ;
            }
            if (ownerIdCheck)
            {
                ModelState.AddModelError("", "Id Already exists");
                return StatusCode(422, ModelState);
            }

            var ownerMap = _mapper.Map<Owner>(owner);
             Country country = _unitOfWork.Country.Get(c=>c.Id==CountryId);
            if (country is null)
            {
                ModelState.AddModelError("", "Country is null");
                return StatusCode(422, ModelState);
            }else
            {
                ownerMap.Country = country;
            }
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
                    }

            _unitOfWork.Owner.Add(ownerMap);
            bool save = _unitOfWork.Save();
            if(!save)
            {
                ModelState.AddModelError("", "Something is wrong with the saving");
                return StatusCode(422, ModelState);
            }
            return Ok("Succefully created!");
        }
        [HttpDelete(nameof(Remove)+"/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Remove(int id)
        {
            bool ownerCheck = _unitOfWork.Owner.EntityExists(id);
            if (!ownerCheck)
            {
                return NotFound();
            }
            var owner = _unitOfWork.Owner.Get(o=>o.Id==id);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _unitOfWork.Owner.Remove(owner);
            bool save = _unitOfWork.Save();
            if (!save)
            {
                ModelState.AddModelError("", "Problem with saving");
                return StatusCode(402, ModelState);
            }

            return NoContent();
        }





    }
}
