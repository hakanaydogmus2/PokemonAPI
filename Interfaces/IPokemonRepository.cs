using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
	public interface IPokemonRepository
	{
		ICollection<Pokemon> GetPokemons();

		Pokemon GetPokemon(int id);
		Pokemon GetPokemon(string name);
		decimal GetPokemonRating(int id);
		bool PokemonExist(int id);
		bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
		bool Save();
		
	}
}
