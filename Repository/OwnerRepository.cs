using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repository
{
	public class OwnerRepository : IOwnerRepository
	{
		private readonly DataContext _context;
		public OwnerRepository(DataContext dataContext)
		{
			_context = dataContext;
		}

        public bool CreateOwner(Owner owner)
        {
            _context.Add(owner);
			return Save();
        }

        public Owner GetOwner(int ownerId)
		{
			return _context.Owners.Where(o => o.Id.Equals(ownerId)).FirstOrDefault();
		}

		public ICollection<Owner> GetOwners()
		{
			return _context.Owners.ToList();
		}

		public ICollection<Owner> GetOwnersFromPokemon(int pokeId)
		{
			return _context.PokemonOwners.Where(p => p.Pokemon.Id.Equals(pokeId))
				.Select(o => o.Owner).ToList();
		}

		public ICollection<Pokemon> GetPokemonsFromOwner(int ownerId)
		{
			return _context.PokemonOwners.Where(o => o.Owner.Id.Equals(ownerId))
				.Select(p => p.Pokemon).ToList();
		}

		public bool OwnerExists(int ownerId)
		{
			return _context.Owners.Any(o => o.Id.Equals(ownerId));
		}

        public bool Save()
        {
            var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
        }

        public bool UpdateOwner(Owner owner)
        {
            _context.Update(owner);
			return Save();
        }
    }
}
