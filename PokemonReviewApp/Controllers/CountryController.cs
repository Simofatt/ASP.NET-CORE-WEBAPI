using AutoMapper;
using DAO.Data;
using DAO2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

       
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CountryController( IUnitOfWork unitOfWork,IMapper mapper) {

           
           _unitOfWork = unitOfWork; 
            _mapper = mapper;
        }

        //GET THE ALL THE OWNERS BY THE ID OF THE COUNTRY

        [HttpGet(nameof(GetOwnersByCountry) +"/{CountryId}")]
        [ProducesResponseType(200, Type = typeof(IList<OwnerDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOwnersByCountry(int CountryId)
        {
            if (!_unitOfWork.Country.ItExists(CountryId))
                return NotFound();
            var owners = _mapper.Map<IList<OwnerDto>>(_unitOfWork.Country.GetOwnersByCountry(CountryId).ToList());
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 
            
            return Ok(owners);
        }


        [HttpGet(nameof(GetCountry))]
        [ProducesResponseType(200, Type = typeof(IList<Country>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountry()
        {
            var countries = _mapper.Map<IList<CountryDto>>(_unitOfWork.Country.GetAll().ToList());
            if (countries is null || countries.Count() ==0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet(nameof(GetCountryById)+"/{countryId}")]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountryById(int countryId)
        {
            var country = _mapper.Map<CountryDto>(_unitOfWork.Country.Get(c=>c.Id==countryId));
            if (country is null )
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpPost(nameof(CreateCountry))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto country)
        {
            if(country is null)
                return BadRequest(ModelState);
            var countryCheck = _unitOfWork.Country.GetAll().Where(c => c.Name.Trim().ToLower() == country.Name.Trim().ToLower());
            var countryCheckId = _unitOfWork.Country.ItExists(country.Id);
            if(countryCheck == null)
            {
                ModelState.AddModelError("", "The country already exists");
                return StatusCode(422, ModelState);

            }
            if(countryCheckId) {
                ModelState.AddModelError("", "The Id already exists");
                return StatusCode(422, ModelState);
            }

            var countryMap = _mapper.Map<Country>(country);
            _unitOfWork.Country.Add(countryMap);
           bool save =  _unitOfWork.Save();
            if(!save)
            {
                ModelState.AddModelError("", "There is a problem with saving");
                return StatusCode(422, ModelState);
            }
            return Ok("Succefully created!");
        }

        [HttpDelete(nameof(DeleteCountry)+"/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        public IActionResult DeleteCountry(int id)
        {
            var countryCheck = _unitOfWork.Country.ItExists(id);
            if (!countryCheck)
            {
                return NotFound();
            }
            var Country = _unitOfWork.Country.Get(p => p.Id == id);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var Owner= _unitOfWork.Country.GetOwnersByCountry(id).ToList();    
            
            _unitOfWork.Owner.RemoveRange(Owner);
            _unitOfWork.Country.Remove(Country);
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

