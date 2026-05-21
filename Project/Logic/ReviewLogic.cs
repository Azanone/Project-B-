using Project.DataAccess;
using Project.Presentation;

namespace Project.Logic;

public class ReviewLogic
{
    public readonly ReviewAccess reviewAccess = new();
    public readonly ProductLogic productLogic = new();

    public ProductModel? GetProductById(string inputId)
    {
        var all = productLogic.GetProducts();

        if (!int.TryParse(inputId, out int parsedId))
        {
            return null;
        }

        var product = all.FirstOrDefault(p => p.ProductID == parsedId);

        if (product == null)
        {
            Console.WriteLine("Product niet gevonden");
        }

        return product;
    }
    
    public void AddReview(string productId)
    {
        var product = GetProductById(productId);

        if (product == null)
        {
            Console.WriteLine("Product niet gevonden");
            return;
        }

        MenuHelpers.WriteColor($"Leave a review for [\"{product.Name}\"]:", ConsoleColor.DarkRed);
        string review = Console.ReadLine();

        MenuHelpers.WriteColor($"How many stars do you give [\"{product.Name}\"]", ConsoleColor.DarkRed);
        string starRating = Console.ReadLine();

        int parsedStarRating = int.Parse(starRating);

        reviewAccess.AddReview(product.ProductID, review, parsedStarRating);

        Console.WriteLine("Review toegevoegd!");
    }
}