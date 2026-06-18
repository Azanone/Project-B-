using Project.Logic;

namespace Project.Presentation;

public static class ProductDetailsView
{
    private static readonly ProductLogic _productLogic = new();
    private static readonly ProductDetailsLogic _logic = new();
    private static readonly ReviewLogic _reviewLogic = new();

    public static void Start()
    {
        Console.Clear();
        var products = _productLogic.GetProducts();
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

        MenuNavigation menu = new MenuNavigation(options, "--- SHOW PRODUCT DETAILS ---");
        int selection = menu.Start();

        if (selection < 0 || selection >= products.Count)
        {
            return;
        }

        DetailMenu(products[selection]);
    }

    public static void Show(ProductModel product)
    {
        DetailMenu(product);
    }

    private static void DetailMenu(ProductModel product)
    {
        while (true)
        {
            List<string> options = new()
            {
                "Location",
                "Ingredients and nutrition",
                "Reviews",
                "Back"
            };
            var rating = _reviewLogic.GetRating(product.ProductID);
            string title = $"--- {product.Name} ---  {rating.Stars} {rating.Average:F1} ({rating.Count})";
            MenuNavigation menu = new MenuNavigation(options, title);
            int sel = menu.Start();

            if (sel == 0)
            {
                ShowLocation(product);
            }
            else if (sel == 1)
            {
                ShowIngredients(product);
            }
            else if (sel == 2)
            {
                ShowReviews(product);
            }
            else
            {
                return;
            }
        }
    }

    private const string MapText = @"╔══════════════╦══════════════════╦═══════════════════╗
║              ║                  ║                   ║
║   BAKERY     ║    DAIRY         ║     FROZEN        ║
║              ║                  ║                   ║
╠══════════════╩══════════════════╣                   ║
║                                 ╚═══════════════════╣
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌─────────┐         ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │         ║
║  │ Deli  │ │ Dry Food │ │rage │ │  And    │         ║
║  │       │ │          │ │     │ │  Goods  │         ║
║  └───────┘ └──────────┘ └─────┘ └─────────┘         ║
╠════════════════════╦════════════════════════════════╣
║                    ║                                ║
║   FRESH PRODUCE    ║    CASHOUT /                   ║
║                    ║    CUSTOMER SERVICE            ║
║                    ║                                ║
╚════════════════════╝        ↑           ╚═══════════╝
                         ENTRANCE / EXIT";

    private static void ShowLocation(ProductModel product)
    {
        Console.Clear();
        MenuHelpers.Announce($"--- LOCATION OF {product.Name} ---");

        string? token = _logic.GetHighlightToken(product.Category);
        foreach (string line in MapText.Split('\n'))
        {
            int idx = token != null ? line.IndexOf(token, StringComparison.Ordinal) : -1;
            if (idx < 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(line);
                Console.ResetColor();
                continue;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(line.Substring(0, idx));
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(line.Substring(idx, token!.Length));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(line.Substring(idx + token.Length));
            Console.ResetColor();
        }

        string? label = _logic.GetLocationLabel(product.Category);
        if (label == null)
        {
            MenuHelpers.Warn($"\nLocation for category '{product.Category}' is not mapped.");
        }
        else
        {
            MenuHelpers.Announce($"\n>>> {label} <<<");
        }

        MenuHelpers.Pause();
    }

    private static void ShowReviews(ProductModel product)
    {
        while (true)
        {
            Console.Clear();
            var rating = _reviewLogic.GetRating(product.ProductID);
            MenuHelpers.Announce($"--- REVIEWS FOR {product.Name} ---");

            if (rating.Count == 0)
            {
                MenuHelpers.Warn("No reviews yet. Be the first to leave one!");
            }
            else
            {
                MenuHelpers.Announce($"Average: {rating.Stars} {rating.Average:F1} ({rating.Count} review(s))\n");
                foreach (ReviewModel r in _reviewLogic.GetReviews(product.ProductID))
                {
                    MenuHelpers.Line($"{r.Username ?? "Guest"}  {_reviewLogic.BuildStars(r.Rating)} {r.Rating:F1}  -  {r.Review}");
                }
            }

            List<string> options = new()
            {
                "Add a review",
                "Back"
            };
            MenuNavigation menu = new MenuNavigation(options, "\n--- REVIEW OPTIONS ---");
            int sel = menu.Start();

            if (sel == 0)
            {
                Console.Clear();
                _reviewLogic.AddReview(product);
                MenuHelpers.Pause();
            }
            else
            {
                return;
            }
        }
    }

    private static void ShowIngredients(ProductModel product)
    {
        Console.Clear();

        MenuHelpers.Line(@"+-------------------------------------------+");
        MenuHelpers.Line($"| {("INGREDIENTS"),-41} |");
        MenuHelpers.Line(@"+-------------------------------------------+");
        foreach (string line in WrapText(product.Ingredients ?? "(no ingredients listed)", 41))
        {
            MenuHelpers.Line($"| {line,-41} |");
        }
        MenuHelpers.Line(@"+-------------------------------------------+");
        MenuHelpers.Line($"| {("NUTRITION FACTS (per 100g)"),-41} |");
        MenuHelpers.Line(@"+-------------------------------------------+");
        MenuHelpers.Line($"| {"Energy",-16}{product.Calories,7} kcal{"",13} |");
        MenuHelpers.Line($"| {"Fats",-16}{product.Fats,7:F1} g{"",17} |");
        MenuHelpers.Line($"| {"Carbohydrates",-16}{product.Carbs,7:F1} g{"",17} |");
        MenuHelpers.Line($"| {"Fiber",-16}{product.Fiber,7:F1} g{"",17} |");
        MenuHelpers.Line($"| {"Protein",-16}{product.Protein,7:F1} g{"",17} |");
        MenuHelpers.Line($"| {"Salt",-16}{product.Salt,7:F2} g{"",17} |");
        MenuHelpers.Line(@"+-------------------------------------------+");

        MenuHelpers.Pause();
    }

    private static List<string> WrapText(string text, int width)
    {
        List<string> lines = new();
        string[] words = text.Split(' ');
        string current = "";
        foreach (string word in words)
        {
            if ((current + " " + word).Trim().Length > width)
            {
                if (current.Length > 0)
                {
                    lines.Add(current);
                }
                current = word;
            }
            else
            {
                current = (current + " " + word).Trim();
            }
        }
        if (current.Length > 0)
        {
            lines.Add(current);
        }
        return lines;
    }
}
