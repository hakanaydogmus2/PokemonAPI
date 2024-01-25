using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;
using PokemonWebApi.DTO;

namespace PokemonWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CountryController : Controller
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
			_mapper = mapper;
        }

		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
		
		public IActionResult GetCountries()
		{
			var countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());
			if(!ModelState.IsValid)
			{
				return BadRequest();
			}
			return Ok(countries);
		}

		[HttpGet("{countryId}")]
		[ProducesResponseType(200, Type =typeof(Country))]
		[ProducesResponseType(400)]

		public IActionResult GetCountry(int countryId)
		{
			if (!_countryRepository.CountryExist(countryId))
			{
				return NotFound();
			}
			var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(countryId));

            if (!ModelState.IsValid)
            {
				return BadRequest(ModelState);
            }
			return Ok(country);
        }
		[HttpGet("owners/{ownerId}")]
		[ProducesResponseType(200, Type = typeof(Country))]
		[ProducesResponseType(400)]

		public IActionResult GetCountryOfAnOwner(int ownerId)
		{
			var country = _mapper.Map<CountryDTO>(_countryRepository
				.GetCountryByOwner(ownerId));

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);

			}
			return Ok(country);


		}

		[HttpPost]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]

		public IActionResult CreateCaountry([FromBody] CategoryDTO countryCreate)
		{
			if(countryCreate == null)
				return BadRequest(ModelState);

			var country = _countryRepository.GetCountries()
				.Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

			if (country != null)
			{
				ModelState.AddModelError("", "Country is already exists");
				return StatusCode(422, ModelState);

			}

			if(!ModelState.IsValid)
				return BadRequest(ModelState);

			var countryMap = _mapper.Map<Country>(countryCreate);

			if (_countryRepository.CreateCountry(countryMap))
			{
				ModelState.AddModelError("", "something wrong while saving");
				return StatusCode(500, ModelState);
			}

			return Ok("country successfully created");
				
		}
		[HttpPut("{countryId}")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]

		public IActionResult UpdateCategory(int countryId, [FromBody] CountryDTO updatedCountry)
		{
			if(updatedCountry == null)
				return BadRequest(ModelState);

			if(countryId != updatedCountry.Id)
				return BadRequest(ModelState);

			if(!_countryRepository.CountryExist(countryId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var countryMap = _mapper.Map<Country>(updatedCountry);

			if (!_countryRepository.UpdateCountry(countryMap))
			{
				ModelState.AddModelError("", "Something went wrong while updating country");
				return StatusCode(500, ModelState);
			}
			return NoContent();
		}


		
    }
}
