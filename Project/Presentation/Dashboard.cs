using Project.Logic;
using Project.Models;

static class Dashboard
{
    private static readonly ProductLogic ProductLogic = new();
    private static readonly OfferLogic OfferLogic = new();
    private static readonly AccountsLogic AccountsLogic = new();
    public static readonly ShoppingCartLogic ShoppingCart = new();
    private static readonly ShoppingCartLogic Wishlist = new();
    private static readonly ReceiptLogic ReceiptLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;

            if (account == null)
            {
                MenuHelpers.Announce("Welcome guest");
            }
            else
            {
                MenuHelpers.Announce("Welcome back " + account.FullName);
            }
            
            MenuHelpers.Confirm("Enter 1 to see all products");
            MenuHelpers.Confirm("Enter 2 to see all offers");
            MenuHelpers.Confirm("Enter 3 to see store layout");
            MenuHelpers.Confirm("Enter 4 to add a product to shopping list");
            MenuHelpers.Confirm("Enter 5 to view shopping list and total");
            MenuHelpers.Confirm("Enter 6 to clear shopping cart");
            MenuHelpers.Confirm("Enter 7 to Wishlist");
            MenuHelpers.Confirm("Enter 8 to show purchase history");
            MenuHelpers.Confirm("Enter 9 to logout");

            string input = MenuHelpers.Prompt("Choose an option") ?? string.Empty;
            if (input == "1")
            {
                Console.Clear();
                ShowProducts.ShowAll();
                MenuHelpers.Pause();
            }
            else if (input == "2")
            {
                Console.Clear();
                ShowOffers.ShowAll();
                MenuHelpers.Pause();
            }
            else if (input == "3")
            {
                ShowLayout.Start();
            }
            else if (input == "4")
            {
                AddProductToShoppingCart();
            }
            else if (input == "5")
            {
                ShowShoppingCart();
            }
            else if (input == "6")
            {
                if (account == null)
                {
                    MenuHelpers.Warn("You must be logged in.");
                    MenuHelpers.Pause();
                    continue;
                }

                ShoppingCart.ClearCurrentCart();
                MenuHelpers.Confirm("Shopping list cleared");
                MenuHelpers.Pause();
            }
            else if (input == "7")
            {
                ShowWishlist.Start();
            }
            else if (input == "8")
            {
                ShowPurchaseHistory();
            }
            else if (input == "9")
            {
                AccountsLogic.Logout();
                Menu.Start();
                return;
            }
            else
            {
                MenuHelpers.Warn("Invalid input");
                MenuHelpers.Pause();
            }
        }
    }

    private static void AddProductToShoppingCart()
    {
        Console.Clear();

        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            MenuHelpers.Pause();
            return;
        }

        var products = ProductLogic.GetProducts();
        MenuHelpers.Announce("--- ADD PRODUCT TO SHOPPING LIST ---");

        foreach (var item in products)
        {
            string ageLabel = item.MinAge > 0 ? $" | Age: {item.MinAge}+" : "";
            MenuHelpers.Confirm($"ID: {item.ProductID} | Name: {item.Name} | Category: {item.Category} | Price: {item.Price} EUR{ageLabel}");
        }

        string rawId = MenuHelpers.Prompt("Enter product ID") ?? string.Empty;

        if (!int.TryParse(rawId, out int productId))
        {
            MenuHelpers.Warn("Invalid product ID");
            MenuHelpers.Pause();
            return;
        }

        ProductModel? selectedProduct =
            products.FirstOrDefault(p => p.ProductID == productId);

        if (selectedProduct == null)
        {
            MenuHelpers.Warn("Product not found");
            MenuHelpers.Pause();
            return;
        }

        if (selectedProduct.MinAge > 0 && account != null && !ProductLogic.IsOldEnoughForProduct(selectedProduct, account.Age))
        {
            MenuHelpers.Warn($"You must be {selectedProduct.MinAge}+ to purchase {selectedProduct.Name}");
            MenuHelpers.Pause();
            return;
        }

        if (selectedProduct.Stock <= 0)
        {
            MenuHelpers.Warn("Selected product is out of stock");
            MenuHelpers.Pause();
            return;
        }

        // var shoppingItem = new ShoppingListModel(
        //     selectedProduct.Name,
        //     selectedProduct.Category,
        //     selectedProduct.Price,
        //     selectedProduct.Brand,
        //     selectedProduct.Ingredients
        // );

        var cartItem = new ShoppingCartItem(selectedProduct, 1);
        ShoppingCart.AddItem(cartItem);

        MenuHelpers.Confirm($"Added {selectedProduct.Name} to shopping list");

        MenuHelpers.Pause();
    }
    
    private static void RemoveItemFromCart()
    {
        Console.Clear();

        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("Account not found");
            MenuHelpers.Pause();
            return;
        }

        var cartItems = ShoppingCart.GetAllItems();

        if (!cartItems.Any())
        {
            MenuHelpers.Warn("Cart is empty");
            MenuHelpers.Pause();
            return;
        }

        MenuHelpers.Announce("--- REMOVE PRODUCT FROM SHOPPING LIST ---");

        foreach (var item in cartItems)
        {
            MenuHelpers.Confirm(
                $"ID: {item.CartItemId} | {item.Product.Name} | Qty: {item.Quantity}"
            );
        }

        string rawId = MenuHelpers.Prompt("Enter item ID") ?? "";

        if (!int.TryParse(rawId, out int cartItemId))
        {
            MenuHelpers.Warn("Invalid ID");
            MenuHelpers.Pause();
            return;
        }

        var itemToRemove = cartItems.FirstOrDefault(item => item.CartItemId == cartItemId);
        if (itemToRemove == null)
        {
            MenuHelpers.Warn("Item not found");
            MenuHelpers.Pause();
            return;
        }

        ShoppingCart.RemoveItem(itemToRemove);

        MenuHelpers.Confirm("Item removed");
        MenuHelpers.Pause();
    }

    private static void ShowShoppingCart()
    {
        Console.Clear();
        var account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in.");
            MenuHelpers.Pause();
            return;
        }

        MenuHelpers.Announce("--- YOUR SHOPPING LIST ---");

        var items = ShoppingCart.GetAllItems();

        if (items.Count == 0)
        {
            MenuHelpers.Warn("Shopping list is empty");
            MenuHelpers.Pause();
            return;
        }

        decimal total = 0;
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            total += item.Product.Price * item.Quantity;

            MenuHelpers.Confirm(
                $"{i + 1}. {item.Product.Name} | " +
                $"Category: {item.Product.Category} | " +
                $"Brand: {item.Product.Brand} | " +
                $"Qty: {item.Quantity} | " +
                $"Price: {item.Product.Price} EUR"
            );
        }

        MenuHelpers.Announce($"Total: {total} EUR");
        MenuHelpers.Pause();
    }
    
    private static void ShowPurchaseHistory()
    {
        Console.Clear();
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account == null)
        {
            MenuHelpers.Warn("You must be logged in to view purchase history");
            MenuHelpers.Pause();
            return;
        }

        var receipts = ReceiptLogic.GetPurchasesByAccountID(account.UserId);
        MenuHelpers.Announce("--- YOUR PURCHASE HISTORY ---");

        if (receipts.Count == 0)
        {
            MenuHelpers.Warn("No purchases found");
            MenuHelpers.Pause();
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

        MenuHelpers.Pause();
    }
}