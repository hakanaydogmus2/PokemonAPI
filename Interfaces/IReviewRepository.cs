using PokemonWebApi.Models;

namespace PokemonWebApi.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsOfAPokemon(int pokeID);
        bool ReviewExists(int reviewId);
        bool CreateReview(Review review);
        bool Save();
    }
}
