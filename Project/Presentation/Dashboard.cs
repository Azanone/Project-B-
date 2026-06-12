using Project.Logic;
using Project.Models;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    public static readonly ShoppingCartLogic ShoppingCart = new();
    private static readonly ReceiptLogic ReceiptLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;
            string userGreeting = "Welcome guest";
            if (account != null)
            {
                userGreeting = "Welcome back " + account.FullName;
            }

            List<string> options = new List<string>
            {
                "Products",
                "Offers",
                "Layout",
                "View cart",
                "Profile"
            };

            MenuNavigation menu = new MenuNavigation(options, userGreeting);
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    AddProductToShoppingCart();
                    break;
                case 1:
                    ShowOffers();
                    break;
                case 2:
                    ShowLayout();
                    break;
                case 3:
                    ShowShoppingCart();
                    break;
                case 4:
                    ShowProfileMenu();
                    break;
            }
        }
    }

    private static void ShowOffers()
    {
        Console.Clear();
        var list = OfferLogic.GetOffers();
        MenuHelpers.Announce("--- ALL OFFERS ---");
        foreach (var item in list)
        {
            MenuHelpers.Confirm($"ID: {item.OfferID} | Description: {item.Description} | Begin: {item.StartDate} | End: {item.EndDate} | Price: {item.RegularPrice} EUR | Discount: {item.DiscountPercentage}% | Discount-price: {item.DiscountPrice} EUR");
        }
        WaitForContinue();
    }

    private static void AddProductToShoppingCart()
{
    Console.Clear();
    var products = ProductLogic.GetProducts();
    var offers = OfferLogic.GetOffers();

    if (products == null || !products.Any())
    {
        MenuHelpers.Warn("No products available");
        WaitForContinue();
        return;
    }

    List<string> options = new List<string>();
    foreach (var item in products)
    {
        OfferModel offer = null;
        var productOfferMap = OfferLogic.GetProductToOfferMapping();
        DateTime today = DateTime.Today;

        if (productOfferMap.TryGetValue((int) item.ProductID, out int offerId))
        {
            offer = offers.FirstOrDefault(o => o.OfferID == offerId && 
                                               today >= o.StartDate.Date && 
                                               today <= o.EndDate.Date);
        }

        if (offer != null)
        {
            options.Add($"discount! Name: {item.Name} | Category: {item.Category} | Price: {offer.DiscountPrice} EUR (Old Price: {offer.RegularPrice} EUR)");
        }
        else
        {
            options.Add($"Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR");
        }
    }
    options.Add("Cancel");

    MenuNavigation menu = new MenuNavigation(options, "--- SELECT A PRODUCT TO ADD ---", true);
    int selection = menu.Start();
    int quantity = menu.GetQuantity();

    if (selection == options.Count - 1)
    {
        return;
    }

    ProductModel selectedProduct = products[selection];

    if (selectedProduct.Stock <= 0)
    {
        MenuHelpers.Warn("Selected product is out of stock");
        WaitForContinue();
        return;
    }
    if (quantity <= 0)
    {
        MenuHelpers.Warn("Please select a quantity greater than 0");
        WaitForContinue();
        return;
    }
    if (quantity > selectedProduct.Stock)
    {
        MenuHelpers.Warn($"Amount selected too high. Only {selectedProduct.Stock} items in stock");
        WaitForContinue();
        return;
    }

    OfferModel finalOffer = null;
    var finalMap = OfferLogic.GetProductToOfferMapping();
    DateTime currentDay = DateTime.Today;
    if (finalMap.TryGetValue((int) selectedProduct.ProductID, out int finalOfferId))
    {
        finalOffer = offers.FirstOrDefault(o => o.OfferID == finalOfferId && 
                                           currentDay >= o.StartDate.Date && 
                                           currentDay <= o.EndDate.Date);
    }

    decimal finalPrice = finalOffer != null ? (decimal)finalOffer.DiscountPrice : selectedProduct.Price;

    var cartItem = new ShoppingCartItem(selectedProduct, quantity, finalPrice);
    ShoppingCart.AddItem(cartItem);

    MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping cart");
    WaitForContinue();
}    private static void ShowShoppingCart()
    {
        while (true)
        {
            Console.Clear();
            MenuHelpers.Announce("--- YOUR SHOPPING CART ---");

            var items = ShoppingCart.GetAllItems();
            if (items.Count == 0)
            {
                MenuHelpers.Warn("Shopping CART is empty");
                WaitForContinue();
                return;
            }

            decimal total = 0;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                decimal lineTotal = (decimal)item.Product.Price * item.Quantity;
                total += lineTotal;

                MenuHelpers.Confirm($"{i + 1}. {item.Product.Name} | Category: {item.Product.Category} | Brand: {item.Product.Brand} | Qty: {item.Quantity} | Price: {item.Product.Price} EUR (Total: {lineTotal} EUR)");
            }

            MenuHelpers.Announce($"Total (preview): {total} EUR");
            Console.WriteLine();

            List<string> options = new List<string>
            {
                "Checkout and pay",
                "Clear shopping cart",
                "Back to Dashboard"
            };

            MenuNavigation cartMenu = new MenuNavigation(options, "--- CART OPTIONS ---");
            int selection = cartMenu.Start();

            if (selection == 0)
            {
                PurchaseShoppingCart.PurchaseChoice();
                break;
            }
            else if (selection == 1)
            {
                ShoppingCart.GetAllItems().Clear();
                MenuHelpers.Confirm("Shopping cart cleared");
                WaitForContinue();
                break;
            }
            else if (selection == 2)
            {
                break;
            }
        }
    }

    private static void ShowProfileMenu()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;

            List<string> options = new List<string>();
            if (account == null)
            {
                options.Add("Login");
                options.Add("Back to Dashboard");
            }
            else
            {
                options.Add("Wishlist");
                options.Add("Show purchase history");
                options.Add("Logout");
                options.Add("Back to Dashboard");
            }

            MenuNavigation profileMenu = new MenuNavigation(options, "--- PROFILE ---");
            int selection = profileMenu.Start();

            if (account == null)
            {
                if (selection == 0)
                {
                    Menu.Start();
                    return;
                }
                else if (selection == 1)
                {
                    break;
                }
            }
            else
            {
                if (selection == 0)
                {
                    ShowWishlist.Start();
                }
                else if (selection == 1)
                {
                    ShowPurchaseHistory();
                }
                else if (selection == 2)
                {
                    AccountsLogic.Logout();
                    Menu.Start();
                    return;
                }
                else if (selection == 3)
                {
                    break;
                }
            }
        }
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
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌_________┐         ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │         ║
║  │ Deli  │ │ Dry Food │ │rage │ │ And     │         ║
║  │       │ │          │ │     │ │ Goods   │         ║
║  └───────┘ └──────────┘ └─────┘ └_________┘         ║
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

    private static void ShowPurchaseHistory()
    {
        Console.Clear();
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in to view purchase history");
            WaitForContinue();
            return;
        }

        var receipts = ReceiptLogic.GetPurchasesByAccountID((int)account.Id);
        MenuHelpers.Announce("--- YOUR PURCHASE HISTORY ---");

        if (receipts.Count == 0)
        {
            MenuHelpers.Warn("No purchases found");
            WaitForContinue();
            return;
        }

        var grouped = receipts.GroupBy(r => r.PurchaseID);
        foreach (var group in grouped)
        {
            var first = group.First();
            MenuHelpers.Confirm("----------------------------------------");
            MenuHelpers.Announce($"  Purchase #{group.Key}  |  {first.CreatedAt:dd-MM-yyyy}");
            MenuHelpers.Confirm("----------------------------------------");
            foreach (var item in group)
            {
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                MenuHelpers.Confirm($"  {item.ProductName,-22} {qtyLabel}{lineTotal:F2}");
            }
            MenuHelpers.Confirm("----------------------------------------");
            MenuHelpers.Confirm($"  Total:                    {first.TotalPrice:F2}");
            Console.WriteLine();
        }

        WaitForContinue();
    }
}