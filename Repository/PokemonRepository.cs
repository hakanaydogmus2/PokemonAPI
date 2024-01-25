using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;
namespace PokemonWebApi.Repository
{
	
	public class PokemonRepository : IPokemonRepository
	{
		private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var PokemonOwnerEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
			var PokemonCategoryEntity = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

			var pokemonOwner = new PokemonOwner()
			{
				Owner = PokemonOwnerEntity,
				Pokemon = pokemon,
			};

			_context.Add(pokemonOwner);

			var pokemonCategory = new PokemonCategory()
			{
				Category = PokemonCategoryEntity,
				Pokemon = pokemon,
			};

			_context.Add(pokemonCategory);

			_context.Add(pokemon);
			return Save();
        }

        public Pokemon GetPokemon(int id)
		{
			return _context.Pokemons.Where(p => p.Id.Equals(id)).FirstOrDefault();
		}

		public Pokemon GetPokemon(string name)
		{
			return _context.Pokemons.Where(p => p.Name.Equals(name)).FirstOrDefault();
		}

		public decimal GetPokemonRating(int id)
		{
			var review = _context.Reviews.Where(r => r.Id.Equals(id));
		

			if(_context.Reviews.Count() <= 0)
			{
				return 0;
			}
		
			
			return ((decimal)review.Sum(r => r.Rating) / review.Count());
			
		}

		public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }

		public bool PokemonExist(int id)
		{
			
			return _context.Pokemons.Any(p => p.Id.Equals(id));
		}

        public bool Save()
        {
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
        }
    }
}
