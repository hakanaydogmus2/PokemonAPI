using PokemonWebApi.Models;
using System.Collections;

namespace PokemonWebApi.Interfaces
{
	public interface IOwnerRepository
	{
		ICollection<Owner> GetOwners();
		Owner GetOwner(int ownerId);
		bool OwnerExists(int ownerId);
		ICollection<Owner> GetOwnersFromPokemon(int pokeId);
		ICollection<Pokemon> GetPokemonsFromOwner(int ownerId);
		bool CreateOwner(Owner owner);
		bool UpdateOwner(Owner owner);
		bool Save();
	}
}
