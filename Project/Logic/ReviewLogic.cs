using Project.DataAccess;

public class ReviewLogic
{
    public readonly ReviewAccess reviewAccess = new();
    public readonly ProductLogic productLogic = new();

    public ProductModel? GetProductById(string inputId)
    {
        if (!int.TryParse(inputId, out int parsedId))
        {
            MenuHelpers.Warn("Product not found.");
            return null;
        }

        var product = productLogic.GetProducts().FirstOrDefault(p => p.ProductID == parsedId);
        if (product == null)
        {
            MenuHelpers.Warn("Product not found.");
        }

        return product;
    }

    public void AddReview(string productId)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return;
        }

        AddReview(product);
    }

    public void AddReview(ProductModel product)
    {
        string review;
        do
        {
            review = MenuHelpers.Prompt($"Leave a review for \"{product.Name}\" (max 250 characters):") ?? "";
            if (review.Length > 250)
            {
                MenuHelpers.Warn("Review is too long.");
            }
        } while (review.Length > 250);

        int rating;
        do
        {
            string? input = MenuHelpers.Prompt($"How many stars do you give \"{product.Name}\"? (0 - 5)");
            if (!int.TryParse(input, out rating) || rating < 0 || rating > 5)
            {
                MenuHelpers.Warn("Invalid rating (0 - 5).");
                rating = -1;
            }
        } while (rating < 0 || rating > 5);

        reviewAccess.AddReview((int)product.ProductID, review, rating);
        MenuHelpers.Confirm("Review added!");
    }

    public string StarCalculate()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var grouped = reviewAccess.GetAllReviews().GroupBy(r => r.ProductId);
        foreach (var group in grouped)
        {
            double avg = group.Average(p => p.Rating);
            Console.WriteLine($"{group.Key}: {BuildStars(avg)} | {avg:F1}");
        }

        return "all";
    }

    public List<ReviewModel> GetReviews(long productId)
    {
        return reviewAccess.GetReviewsByProduct((int)productId);
    }

    public (double Average, int Count, string Stars) GetRating(long productId)
    {
        var reviews = reviewAccess.GetReviewsByProduct((int)productId);
        if (reviews.Count == 0)
        {
            return (0, 0, BuildStars(0));
        }

        double avg = reviews.Average(r => r.Rating);
        return (avg, reviews.Count, BuildStars(avg));
    }

    public string BuildStars(double avg)
    {
        const string fullStar = "★";
        const string halfStar = "⯨";
        const string emptyStar = "☆";

        int full = (int)avg;
        bool half = (avg - full) >= 0.5;

        string stars = "";
        for (int i = 0; i < full; i++)
            stars += fullStar;
        if (half)
            stars += halfStar;
        for (int i = full + (half ? 1 : 0); i < 5; i++)
            stars += emptyStar;

        return stars;
    }
}
