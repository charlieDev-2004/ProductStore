using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.Review;
using AutoMapper;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ReviewController(IRepository<Review> reviewRepository, IMapper mapper, IRepository<Product> productRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateReview(ReviewCreateDto reviewDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = _mapper.Map<Review>(reviewDto);
            review.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            review.CreatedAt = DateTime.Now;

            if (await _reviewRepository.Create(review))
                return Ok(new { Message = "Reseña creada correctamente." });

            return StatusCode(500, new { Message = "Ha ocurrido un error al realizar la operación." });
        }

    }
}