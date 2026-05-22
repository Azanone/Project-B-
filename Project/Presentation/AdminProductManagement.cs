using System;
using System.Collections.Generic;
using System.Linq;

public static class AdminProductManagement
{
    private static readonly AdminLogic _adminLogic = new();

    public static void Start()
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
                    AdminInformationOverview.ShowProducts();
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
                    AdminMenu.Start();
                    return;
            }
        }
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
                Console.WriteLine($"  [{category.CategoryID}] {category.Name}");
            }
            Console.WriteLine("\nUse UP/DOWN arrows to navigate fields, type text directly, and press Enter on an action option.\n");

            int selection = menu.Start();
            List<string> values = menu.GetValues();

            if (selection == 8)
            {
                return;
            }

            if (selection == 7)
            {
                string name = values[0].Trim();
                string priceInput = values[1].Trim();
                string brand = values[2].Trim();
                string ingredients = values[3].Trim();
                string categoryInput = values[4].Trim();
                string stockInput = values[5].Trim();
                string minAgeInput = values[6].Trim();

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(priceInput) ||
                    string.IsNullOrWhiteSpace(brand) ||
                    string.IsNullOrWhiteSpace(ingredients) ||
                    string.IsNullOrWhiteSpace(categoryInput) ||
                    string.IsNullOrWhiteSpace(stockInput) ||
                    string.IsNullOrWhiteSpace(minAgeInput))
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

                var result = _adminLogic.AddProduct(name, priceInput, brand, ingredients, categoryInput, stockInput, minAgeInput);
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
            false, // Save button
            false  // Cancel button
        };

        while (true)
        {
            MenuNavigation menu = new MenuNavigation(labels, requiresInput, $"--- EDITING PRODUCT: {existingProduct.Name} (ID: {existingProduct.ProductID}) ---");

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
            }

            Console.Clear();
            MenuHelpers.Announce("Available Categories for Reference:");
            foreach (var category in categories)
            {
                string marker = category.CategoryID == existingProduct.CategoryID ? " (current)" : "";
                Console.WriteLine($"  [{category.CategoryID}] {category.Name}{marker}");
            }
            Console.WriteLine("\nModify any text field directly, then press Enter on 'Save Changes' to update.\n");

            int selection = menu.Start();
            List<string> values = menu.GetValues();

            if (selection == 8)
            {
                return;
            }

            if (selection == 7)
            {
                string name = values[0].Trim();
                string priceInput = values[1].Trim();
                string brand = values[2].Trim();
                string ingredients = values[3].Trim();
                string categoryInput = values[4].Trim();
                string stockInput = values[5].Trim();
                string minAgeInput = values[6].Trim();

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(priceInput) ||
                    string.IsNullOrWhiteSpace(brand) ||
                    string.IsNullOrWhiteSpace(ingredients) ||
                    string.IsNullOrWhiteSpace(categoryInput) ||
                    string.IsNullOrWhiteSpace(stockInput) ||
                    string.IsNullOrWhiteSpace(minAgeInput))
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

                var result = _adminLogic.UpdateProduct(productId, name, priceInput, brand, ingredients, categoryInput, stockInput, minAgeInput);
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

        MenuNavigation confirmationMenu = new MenuNavigation(options, $"Are you completely sure you want to remove {targetProduct.Name} (ID: {productId})?");
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
}