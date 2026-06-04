using Project.Logic;
using Project.Models;

static class DashboardGuest
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;

            MenuHelpers.Announce("Welcome " + account?.FullName);

            MenuHelpers.Confirm("Enter 1 to see all products");
            MenuHelpers.Confirm("Enter 2 to see all offers");
            MenuHelpers.Confirm("Enter 3 to see store layout");
            MenuHelpers.Confirm("Enter 4 to logout");

            string input = MenuHelpers.Prompt("Choose an option") ?? string.Empty;

            if (input == "1")
                ShowProducts();
            else if (input == "2")
                ShowOffers();
            else if (input == "3")
                ShowLayout();
            else if (input == "4")
            {
                AccountsLogic.Logout();
                Menu.Start();
                return;
            }
            else
            {
                MenuHelpers.Warn("Invalid input");
                WaitForContinue();
            }
        }
    }

    private static void ShowProducts()
    {
        Console.Clear();
        var list = ProductLogic.GetProducts();

        MenuHelpers.Announce("--- ALL PRODUCTS ---");

        foreach (var item in list)
        {
            string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";

            MenuHelpers.Confirm(
                $"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR{ageLabel}"
            );
        }

        WaitForContinue();
    }

    private static void ShowOffers()
    {
        Console.Clear();
        var list = OfferLogic.GetOffers();

        MenuHelpers.Announce("--- ALL OFFERS ---");

        if (list == null || list.Count == 0)
        {
            MenuHelpers.Warn("No offers available at the moment.");
            WaitForContinue();
            return;
        }

        foreach (var item in list)
        {
            MenuHelpers.Confirm(
                $"ID: {item.OfferID} | {item.Description} | Discount: {item.DiscountPercentage}% | Price: {item.DiscountPrice} EUR"
            );
        }

        WaitForContinue();
    }

    private static void ShowLayout()
    {
        Console.Clear();
        MenuHelpers.Confirm(@"╔══════════════╦══════════════════╦═══════════════════╗
║              ║                  ║                   ║
║   BAKERY     ║     DAIRY        ║     FROZEN        ║
║              ║                  ║                   ║
╠══════════════╩══════════════════╣                   ║
║                                 ╚═══════════════════╣
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌─────────┐        ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │        ║
║  │ Deli  │ │ Dry Food │ │rage │ │  And    │        ║
║  │       │ │          │ │     │ │  Goods  │        ║
║  └───────┘ └──────────┘ └─────┘ └─────────┘        ║
╠════════════════════╦════════════════════════════════╣
║                    ║                                ║
║   FRESH PRODUCE    ║    CASHOUT /                   ║
║                    ║    CUSTOMER SERVICE            ║
║                    ║                                ║
╚════════════════════╝        ↑           ╚═══════════╝
                        ENTRANCE / EXIT");
        WaitForContinue();
    }

    private static void WaitForContinue()
    {
        MenuHelpers.Prompt("Press Enter to continue");
    }
}