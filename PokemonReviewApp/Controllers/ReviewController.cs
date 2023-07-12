using AutoMapper;
using DAO2.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Utility;

namespace ReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(nameof(GetReviews)+"/reviewDto")]
        [ProducesResponseType(200, Type = typeof(List<ReviewDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviews()
        {
            List<ReviewDto> reviews = _mapper.Map<List<ReviewDto>>(_unitOfWork.Reviews.GetAll().ToList());
            if (reviews is null || reviews.Count == 0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet(nameof(GetReviewById)+"/{reviewId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewById(int reviewId)
        {
            var review = _mapper.Map<ReviewDto>(_unitOfWork.Reviews.Get(r => r.Id == reviewId));
            if (!_unitOfWork.Reviews.ItExists(reviewId) )
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }
        [HttpGet(nameof(GetReviewByPokemon)+"/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewByPokemon(int pokemonId)
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_unitOfWork.Reviews.GetReviewByPokemonId(pokemonId));
            if (!_unitOfWork.Reviews.ItExists(pokemonId))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }
        [HttpGet(nameof(GetReviewByPokemon))]
        [ProducesResponseType(200, Type = typeof(List<ReviewDto2>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetReviewsPokemon()
        {
            var data = _mapper.Map<List<ReviewDto2>>(_unitOfWork.Reviews.GetAll("Pokemon,Reviewer"));
            if (data is null || data.Count() ==0)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(data);
        }
    }
}
