using Project.Logic;

namespace Project.Presentation;
using System.Linq;

public static class ProductReviews
{
    public static ReviewLogic ReviewLogic = new();

    public static void Start()
    {
        Console.Clear();

        MenuHelpers.WriteColor("Choose a product [ID] to leave a review for", ConsoleColor.Red);

        var input = Console.ReadLine();

        ReviewLogic.AddReview(input);
    }
}