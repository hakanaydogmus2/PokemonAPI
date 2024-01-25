using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
	public interface ICategoryRepository
	{
		ICollection<Category> GetCategories();
		Category GetCategory(int id);
		Category GetCategory(string name);
		ICollection<Pokemon> GetPokemonsByCategory(int categoryId);
		bool CategoryExists(int id);
		bool CreateCategory(Category category);
		bool UpdateCategory(Category category);
		bool DeleteCategory(Category category);
		bool Save();

	}
}
