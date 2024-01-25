using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.DTO;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class CategoryController : Controller
	{
		
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(200, Type =typeof(IEnumerable<Category>))]
		public IActionResult GetCategories()
		{
			var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());
			if(!ModelState.IsValid)
			{
				return BadRequest();
			}
			return Ok(categories);
		}

		[HttpGet("{categoryId}")]
		[ProducesResponseType(200,Type =typeof(Category))]
		[ProducesResponseType(400)]
		public IActionResult GetCategory(int categoryId)
		{
			if(!_categoryRepository.CategoryExists(categoryId))
			{
				return NotFound();
			}
			var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(categoryId));
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(category);
		}

		[HttpGet("{categoryId}/Pokemons")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
		[ProducesResponseType(400)]

		public IActionResult GetPokemonsByCategory(int categoryId)
		{
			if (!_categoryRepository.CategoryExists(categoryId))
			{
				return NotFound();
			}

			var pokemons = _mapper.Map<List<PokemonDTO>>(_categoryRepository.GetPokemonsByCategory(categoryId));
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return Ok(pokemons);
		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
		{
			if (categoryCreate == null)
				return BadRequest();
			
			var category = _categoryRepository.GetCategories()
				.Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
				.FirstOrDefault();

			if(category != null)
			{
				ModelState.AddModelError("", "Category is already exist");
				return StatusCode(422, ModelState);
			}
			if(!ModelState.IsValid)
				return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

			if (!_categoryRepository.CreateCategory(categoryMap))
			{
				ModelState.AddModelError("", "Something went wrong while saving category");
				return StatusCode(500, ModelState);
			}
			return Ok("Successfully Created");

		}
		[HttpPut("{categoryId}")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO updatedCategory)
		{
			if(updatedCategory == null)
				return BadRequest(ModelState);
			
			if(categoryId != updatedCategory.Id)
			{
				return BadRequest(ModelState);
			}

			if(!_categoryRepository.CategoryExists(categoryId))
				return NotFound();

			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			var categoryMap = _mapper.Map<Category>(updatedCategory);

			if (!_categoryRepository.UpdateCategory(categoryMap))
			{
				ModelState.AddModelError("", "Something went wrong while updating category");
				return StatusCode(500, ModelState);
			}
			return NoContent();

		}

		[HttpDelete("{categoryId}")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult DeleteCategory(int categoryId)
		{
			if (!_categoryRepository.CategoryExists(categoryId))
				return NotFound();

			var categoryToDelete = _categoryRepository.GetCategory(categoryId);

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (!_categoryRepository.DeleteCategory(categoryToDelete))
			{
				ModelState.AddModelError("", "Something Went Wrong while deleting category");

			}

			return NoContent();
		}



	}
}
