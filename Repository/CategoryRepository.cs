using PokemonWebApi.Data;
using PokemonWebApi.Interfaces;
using PokemonWebApi.Models;

namespace PokemonWebApi.Repository
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int id)
		{
			return _context.Categories.Any(c => c.Id.Equals(id));
		}

        public bool CreateCategory(Category category)
        {
			_context.Add(category);
			return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
			return Save();
        }

        public ICollection<Category> GetCategories()
		{
			
			return _context.Categories.OrderBy(c => c.Id).ToList();
		}

		public Category GetCategory(int id)
		{
			return _context.Categories.Where(c => c.Id.Equals(id)).FirstOrDefault();
		}

		public Category GetCategory(string name)
		{
			throw new NotImplementedException();
		}

		public ICollection<Pokemon> GetPokemonsByCategory(int categoryId)
		{
			return _context.PokemonCategories.Where(pc =>  pc.CategoryId.Equals(categoryId)).Select(pc => pc.Pokemon).ToList();
		}

        public bool Save()
        {
			var saved = _context.SaveChanges() > 0 ? true : false;
			return saved;
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
			return Save();
        }
    }
}
