using System.ComponentModel;
using Project.DataAccess;
using Project.Models;
using Project.Presentation;

namespace Project.Logic;

public class ReviewLogic
{
    public readonly ReviewAccess reviewAccess = new();
    public readonly ProductLogic productLogic = new();

    public ProductModel? GetProductById (string inputId)
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

    public void AddReview (string productId)
    {
        var product = GetProductById(productId);

        if (product == null)
        {
            Console.WriteLine("Product niet gevonden");
            return;
        }

        string review;
        int rating;
        
        do
        {
            MenuHelpers.WriteColor($"Leave a review for [\"{product.Name}\"] (Max 250 characters):", ConsoleColor.DarkRed);
            
            review = Console.ReadLine();
            
            if (ReviewCheck(review, r => r.Length >= 250))
            {
                Console.WriteLine("Review is to long");
            }
        } while (ReviewCheck(review, r => r.Length >= 250));
        
        do
        {
            MenuHelpers.WriteColor(
                $"How many stars do you give [\"{product.Name}\"] (Max 5)",
                ConsoleColor.DarkRed
            );

            string starRating = Console.ReadLine();

            if (!int.TryParse(starRating, out rating) || rating < 0 || rating > 5)
            {
                Console.WriteLine("Ongeldige rating (0 - 5)");
            }

        } while (rating < 0 || rating > 5);
        
        reviewAccess.AddReview(product.ProductID, review, rating);
        Console.WriteLine("Review toegevoegd!");
        
    }
    
    public string StarCalculate()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        string fullStar = "★";
        string halfStar = "⯨";
        string emptyStar = "☆";

        var all = reviewAccess.GetAllReviews();

        Func<IEnumerable<ReviewModel>, double> rating = r => r.Average(p => p.Rating);

        var grouped = all.GroupBy(r => r.ProductId);

        foreach (var group in grouped)
        {
            double avg = rating(group);

            int full = (int)avg;
            bool half = (avg - full) >= 0.5;

            string stars = "";

            for (int i = 0; i < full; i++)
                stars += fullStar;

            if (half)
                stars += halfStar;

            for (int i = full + (half ? 1 : 0); i < 5; i++)
                stars += emptyStar;

            Console.WriteLine($"{group.Key}: {stars} | {avg:F1}");
        }

        return "all";
    }

    public bool ReviewCheck (string review, Func<string, bool> reviewLenghtCheck)
    {
        return reviewLenghtCheck(review);
    }
}