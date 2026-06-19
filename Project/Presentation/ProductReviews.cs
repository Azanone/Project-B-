using Project.Logic;

namespace Project.Presentation;

public static class ProductReviews
{
    public static ReviewLogic ReviewLogic = new();
    private static readonly ProductLogic ProductLogic = new();

    public static void Start()
    {
        Console.Clear();

        var products = ProductLogic.GetProducts();
        if (products.Count == 0)
        {
            MenuHelpers.Warn("No products available.");
            MenuHelpers.Pause();
            return;
        }

        List<string> options = products
            .Select(p => $"{p.Name} | {p.Brand} | {p.Category}")
            .ToList();
        options.Add("Back");

        MenuNavigation menu = new MenuNavigation(options, "--- SELECT A PRODUCT TO REVIEW ---");
        int selection = menu.Start();

        if (selection < 0 || selection >= products.Count)
        {
            return;
        }

        Console.Clear();
        ProductModel product = products[selection];
        ReviewLogic.AddReview(product);

        var rating = ReviewLogic.GetRating(product.ProductID);
        MenuHelpers.Announce($"\n--- REVIEWS FOR {product.Name} ---");
        MenuHelpers.Confirm($"Average: {rating.Stars} {rating.Average:F1} ({rating.Count} review(s))\n");
        foreach (ReviewModel r in ReviewLogic.GetReviews(product.ProductID))
        {
            MenuHelpers.Line($"{r.Username ?? "Guest"}  {ReviewLogic.BuildStars(r.Rating)} {r.Rating:F1}  -  {r.Review}");
        }
        MenuHelpers.Pause();
    }
}
