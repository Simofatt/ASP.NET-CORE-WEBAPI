using AutoMapper;
using DAO2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using Utility;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet(nameof(GetCategory))]
        [ProducesResponseType(200, Type = typeof(IList<Category>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCategory()
        {
            var categories = _mapper.Map<IList<CategoryDto>>(_unitOfWork.Category.GetAll().ToList());
            if (categories is null || categories.Count() ==0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        

        [HttpGet( nameof(GetPokemonByCategory) + "/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IList<PokemonDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPokemonByCategory (int categoryId)
        {
            var pokemon = _mapper.Map<IList<PokemonDto>>(_unitOfWork.Category.GetPokemonByCategory(categoryId).ToList());
            if (!ModelState.IsValid)
                return BadRequest();
            if(!_unitOfWork.Category.ItExists(categoryId)) 
                return NotFound();  
            return Ok(pokemon); 
        }

        [HttpPost(nameof(AddCategory))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult AddCategory([FromBody] CategoryDto category)
        {
            if (category is null)
                return BadRequest(ModelState);
            
            var categoryCheck = _unitOfWork.Category.GetAll().Where(c=>c.Name.Trim().ToLower() == category.Name.Trim().ToLower()).FirstOrDefault();
            var categoryCheckId = _unitOfWork.Category.ItExists(category.Id);
            if (categoryCheck != null)
            {
                ModelState.AddModelError("", "The category already exists");
                return StatusCode(422, ModelState);
            }

            if (categoryCheckId)
            {
                    ModelState.AddModelError("", "The category already exists");
                    return StatusCode(500, ModelState);   
            }
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(category);
           
            _unitOfWork.Category.Add(categoryMap);
           if(!_unitOfWork.Save())
            {
                ModelState.AddModelError("", "There is a problem in saving the entity");
                return StatusCode(422, ModelState);
            }

            return Ok("Succefully created!");
        }


        [HttpDelete(nameof(RemoveCategory) + "/{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public IActionResult RemoveCategory(int categoryId) {

            var exists = _unitOfWork.Category.ItExists(categoryId);
            if(!exists)
                return NotFound();
           
            var category = _unitOfWork.Category.Get(c=>c.Id== categoryId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _unitOfWork.Category.Remove(category);
            bool save = _unitOfWork.Save();
            if(!save)
            {
                ModelState.AddModelError("","Problem with saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Category has been deleted");
        }


        [HttpPut(nameof(Update))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        
        public IActionResult Update([FromQuery]int categoryId, [FromBody] CategoryDto category) {
            var checkCategory = _unitOfWork.Category.ItExists(categoryId);
            if (category  is null)
                return BadRequest(ModelState);
            if(categoryId == category.Id)
                return BadRequest(ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!checkCategory)
                return NotFound();

            var categoryMap = _mapper.Map<Category>(category);
            if (categoryMap is not null)
            {
                _unitOfWork.Category.Update(categoryMap);
                bool save = _unitOfWork.Save();
                if(!save)
                {
                    ModelState.AddModelError("", "Something is wrong with Saving");
                    return StatusCode(500, ModelState);
                }
            }
            return NoContent(); 
        }
    }



   

}
