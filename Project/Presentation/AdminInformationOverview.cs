using Project.Logic;
using Project.Models;

public static class AdminInformationOverview
{
    private static readonly ProductLogic _productLogic = new();
    private static readonly OfferLogic _offerLogic = new();
    private static readonly ReceiptLogic _receiptLogic = new();
    private static readonly PurchaseLogic _purchaseLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();

            List<string> options = new List<string>
            {
                "Show Stock",
                "Show Receipts",
                "Show Products",
                "Show Offers",
                "Show Layout",
                "Show Past Transactions",
                "Return to Admin Menu"
            };

            MenuNavigation menu = new MenuNavigation(options, "--- ADMIN INFORMATION OVERVIEW ---");
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    ShowStock();
                    break;
                case 1:
                    ShowReceipts();
                    break;
                case 2:
                    ShowProducts();
                    break;
                case 3:
                    ShowOffers();
                    break;
                case 4:
                    ShowLayout();
                    break;
                case 5:
                    ShowPastTransactions();
                    break;
                case 6:
                    AdminMenu.Start();
                    return;
            }
        }
    }

    private static void ShowStock()
    {
        Console.Clear();
        var list = _productLogic.GetProducts();
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID}| Name: {item.Name}| Stock: {(item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK")}");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowReceipts()
    {
        Console.Clear();
        List<ReceiptModel> receipts = _receiptLogic.GetPurchases();
        var grouped = from r in receipts group r by r.ReceiptID;

        Console.WriteLine("--- ALL RECEIPTS ---\n");

        foreach (var group in grouped)
        {
            ReceiptModel first = group.First();

            Console.WriteLine(@"  ___________________________________________");
            Console.WriteLine(@" /                                           \");
            Console.WriteLine(@"|   *************************************** |");
            Console.WriteLine(@"\_  * BabylonMarkt                        * |");
            Console.WriteLine(@"  | *************************************** |");
            Console.WriteLine($"  |  Date: {first.CreatedAt:dd-MM-yyyy}                       |");
            Console.WriteLine($"  |  Receipt No: #{first.ReceiptID,-23}   |");
            Console.WriteLine(@"  | --------------------------------------- |");
            Console.WriteLine($"  |  {"ITEM",-27}{"PRICE",-11} |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            
            foreach (ReceiptModel item in group)
            {
                string name = item.ProductName.Length > 25 ? item.ProductName.Substring(0, 25) : item.ProductName;
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                Console.WriteLine($"  |  {name,-27}{qtyLabel + lineTotal + " EUR",-11}  |");
            }
            
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine($"  |  {"SUBTOTAL:",-27}{first.TotalPrice + " EUR",-11}  |");
            Console.WriteLine($"  |  {"TAX (VAT):",-27}{first.VAT + " EUR",-11}  |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine($"  |  {"TOTAL:",-27}{first.TotalPrice + first.VAT + " EUR",-11}  |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine(@"  |         THANK YOU FOR VISITING!           |");
            Console.WriteLine(@"  | __________________________________________|___");
            Console.WriteLine(@"  | /                                            /");
            Console.WriteLine(@"  \_/___________________________________________/");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    public static void ShowProducts()
    {
        Console.Clear();
        var list = _productLogic.GetProducts();
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.ProductID}| Name: {item.Name}| Category: {item.Category}| Price: {item.Price} EUR| Min Age: {(item.MinAge > 0 ? item.MinAge.ToString() : "None")}");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowOffers()
    {
        Console.Clear();
        var list = _offerLogic.GetOffers();
        MenuHelpers.Announce("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.OfferID}| Description: {item.Description}| Begin-Date: {item.StartDate}| End-Date: {item.EndDate}| Price: {item.RegularPrice} EUR |Discount: {item.DiscountPercentage}%| Discount-price: {item.DiscountPrice} EUR ");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowLayout()
    {
        Console.Clear();
        MenuHelpers.Confirm(@"╔══════════════╦══════════════════╦═══════════════════╗
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
                        ENTRANCE / EXIT");
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowPastTransactions()
    {
        Console.Clear();
        var list = _purchaseLogic.GetPurchases();
        MenuHelpers.Announce("--- TRANSACTION HISTORY ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.PurchaseID}| Name: {item.UserName}| Date:-{item.PurchaseDate}| Amount:{item.TotalAmount}");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }
}