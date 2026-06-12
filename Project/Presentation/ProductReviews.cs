using Project.Logic;

namespace Project.Presentation;

public static class ProductReviews
{
    public static ReviewLogic ReviewLogic = new();

    public static void Start()
    {
        Console.Clear();

        MenuHelpers.Warn("Choose a product [ID] to leave a review for");

        var input = MenuHelpers.Prompt("Enter product ID:");

        if (!string.IsNullOrWhiteSpace(input))
        {
            ReviewLogic.AddReview(input);
        }
    }
}
