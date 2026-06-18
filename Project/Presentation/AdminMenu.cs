using System;
using System.Collections.Generic;
using System.Linq;
using Project.Logic;
using Project.Models;

public static class AdminMenu
{
    private static readonly ProductLogic _productLogic = new();
    private static readonly OfferLogic _offerLogic = new();
    private static readonly ReceiptLogic _receiptLogic = new();
    private static readonly PurchaseLogic _purchaseLogic = new();
    private static readonly AdminLogic _adminLogic = new();

    public static void Start()
    {
        AccountsLogic accountsLogic = new AccountsLogic();

        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;
            if (account == null)
            {
                MenuHelpers.Error("Error: Not logged in as admin.");
                return;
            }

            List<string> options = new List<string>
            {
                "Manage products",
                "Manage users",
                "Manage finance",
                "Other",
                "Logout"
            };

            MenuNavigation menu = new MenuNavigation(options, "--- Admin Dashboard: --- \n Welcome back " + account.FullName);
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    ShowProductManagementMenu();
                    break;
                case 1:
                    ShowUserManagementMenu();
                    break;
                case 2:
                    ShowFinancialOverview.Start();
                    break;
                case 3:
                    ShowInformationOverviewMenu();
                    break;
                case 4:
                    accountsLogic.Logout();
                    Menu.Start();
                    return;
            }
        }
    }

    private static void ShowInformationOverviewMenu()
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
                    return;
            }
        }
    }

    private static void ShowProductManagementMenu()
    {
        while (true)
        {
            Console.Clear();
            List<string> options = new List<string>
            {
                "View all products",
                "Add a product",
                "Edit a product",
                "Remove a product",
                "Return to Admin Menu"
            };

            MenuNavigation menu = new MenuNavigation(options, "--- PRODUCT MANAGEMENT ---");
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    ShowProducts();
                    break;
                case 1:
                    AddProductFlow();
                    break;
                case 2:
                    EditProductFlow();
                    break;
                case 3:
                    RemoveProductFlow();
                    break;
                case 4:
                    return;
            }
        }
    }

    private static void ShowUserManagementMenu()
    {
        while (true)
        {
            Console.Clear();
            ShowUsers();

            List<string> options = new List<string>
            {
                "Refresh user list",
                "Change a user's role",
                "Remove a user",
                "Return to Admin Menu"
            };

            MenuNavigation menu = new MenuNavigation(options, "--- USER MANAGEMENT ---");
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    continue;
                case 1:
                    UpdateUserRoleFlow();
                    break;
                case 2:
                    RemoveUserFlow();
                    break;
                case 3:
                    return;
            }
        }
    }

    private static void ShowStock()
    {
        Console.Clear();
        var list = _productLogic.GetProducts();
        
        var lowStockProducts = list.Where(item => item.Stock <= 5).ToList();

        if (!lowStockProducts.Any())
        {
            MenuHelpers.Announce("--- ALL PRODUCTS HAVE SUFFICIENT STOCK ---");
            MenuHelpers.Prompt("Press Enter to continue");
            return;
        }

        List<string> options = new List<string>();
        foreach (var item in lowStockProducts)
        {
            string stockLabel = item.Stock > 0 ? item.Stock.ToString() : "OUT OF STOCK";
            options.Add($"ID: {item.ProductID} | {item.Name} (Current Stock: {stockLabel}) -> Select to Restock (+15)");
        }
        options.Add("Back to Management Menu");

        MenuNavigation menu = new MenuNavigation(options, "--- LOW STOCK ALERT (5 OR LESS) ---");
        int selection = menu.Start();

        if (selection == options.Count - 1)
        {
            return;
        }

        var selectedProduct = lowStockProducts[selection];
        int newStockValue = (int)selectedProduct.Stock + 15;
        
        _productLogic.OrderProductByID((int)selectedProduct.ProductID, newStockValue);

        MenuHelpers.Confirm($"Successfully restocked {selectedProduct.Name}. New stock database value set to: {newStockValue}");
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowReceipts()
    {
        Console.Clear();
        List<ReceiptModel> receipts = _receiptLogic.GetPurchases();
        var grouped = from r in receipts group r by r.ReceiptID;

        MenuHelpers.Announce("--- ALL RECEIPTS ---\n");

        foreach (var group in grouped)
        {
            ReceiptModel first = group.First();

            MenuHelpers.Line(@"  ___________________________________________");
            MenuHelpers.Line(@" /                                           \");
            MenuHelpers.Line(@"|   *************************************** |");
            MenuHelpers.Line(@"\_  * BabylonMarkt                        * |");
            MenuHelpers.Line(@"  | *************************************** |");
            MenuHelpers.Line($"  |  Date: {first.CreatedAt:dd-MM-yyyy}                       |");
            MenuHelpers.Line($"  |  Receipt No: #{first.ReceiptID,-23} |");
            MenuHelpers.Line(@"  | --------------------------------------- |");
            MenuHelpers.Line($"  |  {"ITEM",-27}{"PRICE",-11} |");
            MenuHelpers.Line(@"  | ---------------------------------------  |");

            foreach (ReceiptModel item in group)
            {
                string name = item.ProductName.Length > 25 ? item.ProductName.Substring(0, 25) : item.ProductName;
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                MenuHelpers.Line($"  |  {name,-27}{qtyLabel + lineTotal + " EUR",-11}  |");
            }

            MenuHelpers.Line(@"  | ---------------------------------------  |");
            MenuHelpers.Line($"  |  {"SUBTOTAL:",-27}{first.TotalPrice + " EUR",-11}  |");
            MenuHelpers.Line($"  |  {"TAX (VAT):",-27}{first.VAT + " EUR",-11}  |");
            MenuHelpers.Line(@"  | ---------------------------------------  |");
            MenuHelpers.Line($"  |  {"TOTAL:",-27}{first.TotalPrice + first.VAT + " EUR",-11}  |");
            MenuHelpers.Line(@"  | ---------------------------------------  |");
            MenuHelpers.Line(@"  |         THANK YOU FOR VISITING!           |");
            MenuHelpers.Line(@"  | __________________________________________|___");
            MenuHelpers.Line(@"  | /                                            /");
            MenuHelpers.Line(@"  \_/___________________________________________/");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowProducts()
    {
        Console.Clear();
        var list = _productLogic.GetProducts();
        MenuHelpers.Announce("--- ALL PRODUCTS ---");
        MenuHelpers.Line($"{"ID",-5}{"NAME",-26}{"CATEGORY",-20}{"PRICE",10}{"MIN AGE",10}");
        MenuHelpers.Line(new string('-', 71));
        foreach (var item in list)
        {
            string ageLabel = item.MinAge > 0 ? item.MinAge.ToString() : "None";
            string name = item.Name.Length > 24 ? item.Name.Substring(0, 24) : item.Name;
            MenuHelpers.Line($"{item.ProductID,-5}{name,-26}{item.Category,-20}{item.Price,7:0.00} EUR{ageLabel,10}");
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
            MenuHelpers.Line($"[ID {item.OfferID}] {item.Description}");
            MenuHelpers.Line($"    Valid: {item.StartDate:dd-MM-yyyy} -> {item.EndDate:dd-MM-yyyy}");
            MenuHelpers.Line($"    Price: {item.RegularPrice,7:0.00} EUR   Discount: {item.DiscountPercentage}%   Now: {item.DiscountPrice,7:0.00} EUR");
            MenuHelpers.Line(new string('-', 60));
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void ShowLayout()
    {
        Console.Clear();
        MenuHelpers.Line(@"╔══════════════╦══════════════════╦═══════════════════╗
 ║              ║                  ║                   ║
 ║   BAKERY     ║    DAIRY         ║     FROZEN        ║ 
 ║              ║                  ║                   ║ 
 ╠══════════════╩══════════════════╣                   ║ 
 ║                                 ╚═══════════════════╣ 
 ║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌─────────┐         ║ 
 ║  │       │ │ Canned & │ │Beve-│ │ Snacks  │         ║ 
 ║  │ Deli  │ │ Dry Food │ │rage │ │ And     │         ║ 
 ║  │       │ │          │ │     │ │ Goods   │         ║ 
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
        MenuHelpers.Line($"{"ID",-5}{"CUSTOMER",-22}{"DATE",-14}{"AMOUNT",12}");
        MenuHelpers.Line(new string('-', 53));
        foreach (var item in list)
        {
            string name = item.UserName.Length > 20 ? item.UserName.Substring(0, 20) : item.UserName;
            MenuHelpers.Line($"{item.PurchaseID,-5}{name,-22}{item.PurchaseDate,-14:dd-MM-yyyy}{item.TotalAmount,8:0.00} EUR");
        }
        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void AddProductFlow()
    {
        List<CategoryModel> categories = _adminLogic.GetCategories();
        if (categories.Count == 0)
        {
            MenuHelpers.Error("No categories found. Cannot add product without a category.");
            return;
        }

        List<string> labels = new List<string>
        {
            "Product Name",
            "Price (EUR)",
            "Brand",
            "Ingredients",
            "Category ID",
            "Stock",
            "Minimum Age",
            "Calories (kcal per 100g)",
            "Fats (g per 100g)",
            "Carbs (g per 100g)",
            "Fiber (g per 100g)",
            "Protein (g per 100g)",
            "Salt (g per 100g)",
            "Confirm and Save Product",
            "Cancel"
        };

        List<bool> requiresInput = new List<bool>
        {
            true,  // Product Name
            true,  // Price
            true,  // Brand
            true,  // Ingredients
            true,  // Category ID
            true,  // Stock
            true,  // Minimum Age
            true,  // Calories
            true,  // Fats
            true,  // Carbs
            true,  // Fiber
            true,  // Protein
            true,  // Salt
            false, // Confirm button
            false  // Cancel button
        };

        while (true)
        {
            MenuNavigation menu = new MenuNavigation(labels, requiresInput, "--- ADD NEW PRODUCT ---");

            Console.Clear();
            MenuHelpers.Announce("Available Categories for Reference:");
            foreach (var category in categories)
            {
                MenuHelpers.Line($"  [{category.CategoryID}] {category.Name}");
            }

            MenuHelpers.Line("\nUse UP/DOWN arrows to navigate fields, \ntype text directly, and press Enter on an action option.\n");

            int selection = menu.Start();
            List<string> values = menu.GetValues();

            if (selection == 14)
            {
                return;
            }

            if (selection == 13)
            {
                string name = values[0].Trim();
                string priceInput = values[1].Trim();
                string brand = values[2].Trim();
                string ingredients = values[3].Trim();
                string categoryInput = values[4].Trim();
                string stockInput = values[5].Trim();
                string minAgeInput = values[6].Trim();
                string caloriesInput = values[7].Trim();
                string fatsInput = values[8].Trim();
                string carbsInput = values[9].Trim();
                string fiberInput = values[10].Trim();
                string proteinInput = values[11].Trim();
                string saltInput = values[12].Trim();

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(priceInput) ||
                    string.IsNullOrWhiteSpace(brand) ||
                    string.IsNullOrWhiteSpace(ingredients) ||
                    string.IsNullOrWhiteSpace(categoryInput) ||
                    string.IsNullOrWhiteSpace(stockInput) ||
                    string.IsNullOrWhiteSpace(minAgeInput) ||
                    string.IsNullOrWhiteSpace(caloriesInput) ||
                    string.IsNullOrWhiteSpace(fatsInput) ||
                    string.IsNullOrWhiteSpace(carbsInput) ||
                    string.IsNullOrWhiteSpace(fiberInput) ||
                    string.IsNullOrWhiteSpace(proteinInput) ||
                    string.IsNullOrWhiteSpace(saltInput))
                {
                    MenuHelpers.Error("All required fields must be filled out before submitting.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!decimal.TryParse(priceInput, out decimal price) || price < 0)
                {
                    MenuHelpers.Error("Price must be a valid non-negative number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(categoryInput, out long categoryId) || !categories.Any(c => c.CategoryID == categoryId))
                {
                    MenuHelpers.Error("Invalid Category ID. Please pick an existing ID from the reference list.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(stockInput, out long stock) || stock < 0)
                {
                    MenuHelpers.Error("Stock must be zero or a positive whole number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(minAgeInput, out long minAge) || minAge < 0)
                {
                    MenuHelpers.Error("Minimum age must be zero or a positive whole number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                var result = _adminLogic.AddProduct(name, priceInput, brand, ingredients, categoryInput, stockInput, minAgeInput, caloriesInput, fatsInput, carbsInput, fiberInput, proteinInput, saltInput);
                if (result.Success)
                {
                    MenuHelpers.Confirm(result.Message);
                    return;
                }

                MenuHelpers.Error(result.Message);
                MenuHelpers.Prompt("Press Enter to return to editing.");
            }
        }
    }

    private static void EditProductFlow()
    {
        List<ProductModel> products = _adminLogic.GetProducts();
        if (products == null || products.Count == 0)
        {
            MenuHelpers.Error("No products available to edit.");
            MenuHelpers.Prompt("Press Enter to continue.");
            return;
        }

        List<string> productOptions = products.Select(p => $"[ID: {p.ProductID}] {p.Name} - €{p.Price}").ToList();
        productOptions.Add("Cancel and Return");

        MenuNavigation selectionMenu = new MenuNavigation(productOptions, "--- SELECT A PRODUCT TO EDIT ---");
        int chosenIndex = selectionMenu.Start();

        if (chosenIndex == productOptions.Count - 1)
        {
            return;
        }

        ProductModel existingProduct = products[chosenIndex];
        string productId = existingProduct.ProductID.ToString();
        List<CategoryModel> categories = _adminLogic.GetCategories();

        List<string> labels = new List<string>
        {
            "Product Name",
            "Price (EUR)",
            "Brand",
            "Ingredients",
            "Category ID",
            "Stock",
            "Minimum Age",
            "Calories (kcal per 100g)",
            "Fats (g per 100g)",
            "Carbs (g per 100g)",
            "Fiber (g per 100g)",
            "Protein (g per 100g)",
            "Salt (g per 100g)",
            "Save Changes",
            "Cancel"
        };

        List<bool> requiresInput = new List<bool>
        {
            true,  // Product Name
            true,  // Price
            true,  // Brand
            true,  // Ingredients
            true,  // Category ID
            true,  // Stock
            true,  // Minimum Age
            true,  // Calories
            true,  // Fats
            true,  // Carbs
            true,  // Fiber
            true,  // Protein
            true,  // Salt
            false, // Save button
            false  // Cancel button
        };

        while (true)
        {
            MenuNavigation menu = new MenuNavigation(labels, requiresInput, $"--- EDITING PRODUCT: {existingProduct.Name} (ID: \n{existingProduct.ProductID}) ---");

            List<string> menuValues = menu.GetValues();
            if (string.IsNullOrEmpty(menuValues[0]) && string.IsNullOrEmpty(menuValues[1]))
            {
                menuValues[0] = existingProduct.Name;
                menuValues[1] = existingProduct.Price.ToString();
                menuValues[2] = existingProduct.Brand;
                menuValues[3] = existingProduct.Ingredients;
                menuValues[4] = existingProduct.CategoryID.ToString();
                menuValues[5] = existingProduct.Stock.ToString();
                menuValues[6] = existingProduct.MinAge.ToString();
                menuValues[7] = existingProduct.Calories.ToString();
                menuValues[8] = existingProduct.Fats.ToString();
                menuValues[9] = existingProduct.Carbs.ToString();
                menuValues[10] = existingProduct.Fiber.ToString();
                menuValues[11] = existingProduct.Protein.ToString();
                menuValues[12] = existingProduct.Salt.ToString();
            }

            Console.Clear();
            MenuHelpers.Announce("Available Categories for Reference:");
            foreach (var category in categories)
            {
                string marker = category.CategoryID == existingProduct.CategoryID ? " (current)" : "";
                MenuHelpers.Line($"  [{category.CategoryID}] {category.Name}{marker}");
            }
            MenuHelpers.Line("\nModify any text field directly, then press Enter on 'Save Changes' to update.\n");

            int selection = menu.Start();
            List<string> values = menu.GetValues();

            if (selection == 14)
            {
                return;
            }

            if (selection == 13)
            {
                string name = values[0].Trim();
                string priceInput = values[1].Trim();
                string brand = values[2].Trim();
                string ingredients = values[3].Trim();
                string categoryInput = values[4].Trim();
                string stockInput = values[5].Trim();
                string minAgeInput = values[6].Trim();
                string caloriesInput = values[7].Trim();
                string fatsInput = values[8].Trim();
                string carbsInput = values[9].Trim();
                string fiberInput = values[10].Trim();
                string proteinInput = values[11].Trim();
                string saltInput = values[12].Trim();

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(priceInput) ||
                    string.IsNullOrWhiteSpace(brand) ||
                    string.IsNullOrWhiteSpace(ingredients) ||
                    string.IsNullOrWhiteSpace(categoryInput) ||
                    string.IsNullOrWhiteSpace(stockInput) ||
                    string.IsNullOrWhiteSpace(minAgeInput) ||
                    string.IsNullOrWhiteSpace(caloriesInput) ||
                    string.IsNullOrWhiteSpace(fatsInput) ||
                    string.IsNullOrWhiteSpace(carbsInput) ||
                    string.IsNullOrWhiteSpace(fiberInput) ||
                    string.IsNullOrWhiteSpace(proteinInput) ||
                    string.IsNullOrWhiteSpace(saltInput))
                {
                    MenuHelpers.Error("Fields cannot be left entirely blank.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!decimal.TryParse(priceInput, out decimal price) || price < 0)
                {
                    MenuHelpers.Error("Price must be a valid non-negative number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(categoryInput, out long categoryId) || !categories.Any(c => c.CategoryID == categoryId))
                {
                    MenuHelpers.Error("Invalid Category ID. Please pick an existing ID from the reference list.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(stockInput, out long stock) || stock < 0)
                {
                    MenuHelpers.Error("Stock must be zero or a positive whole number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                if (!long.TryParse(minAgeInput, out long minAge) || minAge < 0)
                {
                    MenuHelpers.Error("Minimum age must be zero or a positive whole number.");
                    MenuHelpers.Prompt("Press Enter to return to editing.");
                    continue;
                }

                var result = _adminLogic.UpdateProduct(productId, name, priceInput, brand, ingredients, categoryInput, stockInput, minAgeInput, caloriesInput, fatsInput, carbsInput, fiberInput, proteinInput, saltInput);
                if (result.Success)
                {
                    MenuHelpers.Confirm(result.Message);
                    return;
                }

                MenuHelpers.Error(result.Message);
                MenuHelpers.Prompt("Press Enter to return to editing.");
            }
        }
    }

    private static void RemoveProductFlow()
    {
        List<ProductModel> products = _adminLogic.GetProducts();
        if (products == null || products.Count == 0)
        {
            MenuHelpers.Error("No products available to remove.");
            MenuHelpers.Prompt("Press Enter to continue.");
            return;
        }

        List<string> productOptions = products.Select(p => $"[ID: {p.ProductID}] {p.Name} - €{p.Price}").ToList();
        productOptions.Add("Cancel and Return");

        MenuNavigation selectionMenu = new MenuNavigation(productOptions, "--- SELECT A PRODUCT TO REMOVE ---");
        int chosenIndex = selectionMenu.Start();

        if (chosenIndex == productOptions.Count - 1)
        {
            return;
        }

        ProductModel targetProduct = products[chosenIndex];
        string productId = targetProduct.ProductID.ToString();

        List<string> options = new List<string>
        {
            "No, Keep Product",
            "Yes, Permanently Remove Product"
        };

        MenuNavigation confirmationMenu = new MenuNavigation(options, $"Are you completely sure you want to remove {targetProduct.Name} (ID: \n{productId})?");
        int selection = confirmationMenu.Start();

        if (selection == 1)
        {
            var result = _adminLogic.RemoveProduct(productId);
            if (result.Success)
            {
                MenuHelpers.Confirm(result.Message);
            }
            else
            {
                MenuHelpers.Error(result.Message);
                MenuHelpers.Prompt("Press Enter to continue.");
            }
        }
    }

    private static void ShowUsers()
    {
        List<AccountModel> users = _adminLogic.GetUsers();
        if (users.Count == 0)
        {
            MenuHelpers.Warn("No users found.");
            return;
        }

        MenuHelpers.Announce("--- CURRENT USERS ---");
        MenuHelpers.Line($"{"ID",-5}{"USERNAME",-16}{"NAME",-20}{"EMAIL",-28}{"ROLE",-8}");
        MenuHelpers.Line(new string('-', 77));
        foreach (AccountModel user in users)
        {
            MenuHelpers.Line($"{user.Id,-5}{user.Username,-16}{user.FullName,-20}{user.EmailAddress,-28}{user.Role,-8}");
        }

        MenuHelpers.Line();
    }

    private static void UpdateUserRoleFlow()
    {
        string userIdInput = PromptRequiredText("User ID to update:");
        string newRoleInput = PromptRequiredText("New role (Admin/User):");

        var result = _adminLogic.UpdateUserRole(userIdInput, newRoleInput);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
        }
        else
        {
            MenuHelpers.Warn(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void RemoveUserFlow()
    {
        string userIdInput = PromptRequiredText("User ID to remove:");
        string? confirmation = MenuHelpers.Prompt("Type REMOVE to confirm:");
        if (!string.Equals(confirmation, "REMOVE", StringComparison.Ordinal))
        {
            MenuHelpers.Warn("Removal cancelled.");
            MenuHelpers.Prompt("Press Enter to continue");
            return;
        }

        var result = _adminLogic.RemoveUser(userIdInput);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
        }
        else
        {
            MenuHelpers.Warn(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static string PromptRequiredText(string prompt)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            MenuHelpers.Warn("Input is required.");
        }
    }
}