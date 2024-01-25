using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;
using PokemonWebApi.DTO;

namespace PokemonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        
        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper, IOwnerRepository ownerRepository, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _ownerRepository = ownerRepository;
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]

        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());
            
            if (!ModelState.IsValid)
            {
                return BadRequest();

            }
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type =typeof(Pokemon))]
        [ProducesResponseType(400)]
        
        public IActionResult GetPokemon(int id)
        {
            if(_pokemonRepository.PokemonExist(id) == false)
            {
                return NotFound();
            }

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonRating(int id)
        {
            
            if (!_pokemonRepository.PokemonExist(id))
            {
                return NotFound();
            }
            var rating = _pokemonRepository.GetPokemonRating(id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(rating);
        }
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId,[FromBody] PokemonDTO pokemonCreate)
        {
            if(pokemonCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!_ownerRepository.OwnerExists(ownerId))
            {
                ModelState.AddModelError("", "There is no owner with this id");
                return StatusCode(404, ModelState);
            }

            if (!_categoryRepository.CategoryExists(categoryId))
            {
                ModelState.AddModelError("", "There is no Owner with this id");
                return StatusCode(404, ModelState);
            }

            var pokemons = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() ==  pokemonCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if(pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon is already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var pokemonMap = _mapper.Map<Pokemon>(pokemons);

            if (!_pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);

            }

            return Ok("Pokemon Successfully Created");


           
        }



	}
}
