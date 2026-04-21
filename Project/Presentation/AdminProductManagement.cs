public static class AdminProductManagement
{
    private static readonly AdminLogic _adminLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            ShowProducts.ShowAll();
            MenuHelpers.Announce("--- PRODUCT MANAGEMENT ---");
            MenuHelpers.Confirm("Enter 1 to view all products");
            MenuHelpers.Confirm("Enter 2 to add a product");
            MenuHelpers.Confirm("Enter 3 to edit a product");
            MenuHelpers.Confirm("Enter 4 to remove a product");
            MenuHelpers.Confirm("Enter 5 to return to Admin Menu");

            string? input = MenuHelpers.Prompt("");
            if (input == "1")
            {
                ShowProducts.ShowAll();
                MenuHelpers.Prompt("Press Enter to continue");
            }
            else if (input == "2")
            {
                AddProductFlow();
            }
            else if (input == "3")
            {
                EditProductFlow();
            }
            else if (input == "4")
            {
                RemoveProductFlow();
            }
            else if (input == "5")
            {
                AdminMenu.Start();
                return;
            }
            else
            {
                MenuHelpers.Warn("Invalid input");
            }
        }
    }

    private static void AddProductFlow()
    {
        string name = PromptRequiredText("Product name:", "Product name is required.");
        string price = PromptValidDecimal("Price:", "Price must be a valid non-negative number.");
        string brand = PromptRequiredText("Brand:", "Brand is required.");
        string ingredients = PromptRequiredText("Ingredients:", "Ingredients are required.");

        List<CategoryModel> categories = _adminLogic.GetCategories();
        string? categoryId = PromptValidCategoryId(categories);
        if (categoryId == null)
        {
            return;
        }
        string stock = PromptValidNonNegativeWholeNumber("Stock:", "Stock must be zero or a positive whole number.");

        var result = _adminLogic.AddProduct(name, price, brand, ingredients, categoryId, stock);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
            return;
        }

        MenuHelpers.Error(result.Message);
    }

    private static string PromptRequiredText(string prompt, string errorMessage)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            MenuHelpers.Error(errorMessage);
        }
    }

    private static string PromptValidDecimal(string prompt, string errorMessage)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input)
                && decimal.TryParse(input, out decimal number)
                && number >= 0)
            {
                return input.Trim();
            }

            MenuHelpers.Error(errorMessage);
        }
    }

    private static string PromptValidNonNegativeWholeNumber(string prompt, string errorMessage)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input)
                && long.TryParse(input, out long number)
                && number >= 0)
            {
                return input.Trim();
            }

            MenuHelpers.Error(errorMessage);
        }
    }

    private static string? PromptValidCategoryId(List<CategoryModel> categories)
    {
        if (categories.Count == 0)
        {
            MenuHelpers.Error("No categories found. Cannot add product without a category.");
            return null;
        }

        MenuHelpers.Announce("Available categories:");
        foreach (var category in categories)
        {
            MenuHelpers.Confirm($"{category.CategoryID} - {category.Name}");
        }

        while (true)
        {
            string? categoryInput = MenuHelpers.Prompt("Choose category ID:");
            if (long.TryParse(categoryInput, out long categoryId)
                && categories.Any(c => c.CategoryID == categoryId))
            {
                return categoryId.ToString();
            }

            MenuHelpers.Error("Invalid category ID. Choose one from the list above.");
        }
    }

    private static void EditProductFlow()
    {
        string? productId = MenuHelpers.Prompt("Product ID to edit:");
        if (!long.TryParse(productId, out long parsedId) || parsedId <= 0)
        {
            MenuHelpers.Error("Product ID must be a positive number.");
            return;
        }

        ProductModel? existingProduct = _adminLogic.GetProductById(parsedId);
        if (existingProduct == null)
        {
            MenuHelpers.Error($"No product found with ID {parsedId}.");
            return;
        }

        MenuHelpers.Announce($"Editing {existingProduct.Name} (ID: {existingProduct.ProductID})");
        string name = PromptOptionalText($"New product name (Enter to keep: {existingProduct.Name}):", existingProduct.Name);
        string price = PromptOptionalDecimal($"New price (Enter to keep: {existingProduct.Price}):", existingProduct.Price.ToString(), "Price must be a valid non-negative number.");
        string brand = PromptOptionalText($"New brand (Enter to keep: {existingProduct.Brand}):", existingProduct.Brand);
        string ingredients = PromptOptionalText($"New ingredients (Enter to keep: {existingProduct.Ingredients}):", existingProduct.Ingredients);
        string stock = PromptOptionalNonNegativeWholeNumber($"New stock (Enter to keep: {existingProduct.Stock}):", existingProduct.Stock.ToString(), "Stock must be zero or a positive whole number.");

        List<CategoryModel> categories = _adminLogic.GetCategories();
        string categoryId = PromptValidCategoryIdForEdit(categories, existingProduct.CategoryID);

        var result = _adminLogic.UpdateProduct(productId!, name, price, brand, ingredients, categoryId, stock);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
            return;
        }

        MenuHelpers.Error(result.Message);
    }

    private static void RemoveProductFlow()
    {
        string? productId = MenuHelpers.Prompt("Product ID to remove:");
        var result = _adminLogic.RemoveProduct(productId ?? string.Empty);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
            return;
        }

        MenuHelpers.Error(result.Message);
    }

    private static string PromptOptionalText(string prompt, string currentValue)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            string trimmed = input.Trim();
            if (!string.IsNullOrWhiteSpace(trimmed))
            {
                return trimmed;
            }

            MenuHelpers.Error("Invalid input.");
        }
    }

    private static string PromptOptionalDecimal(string prompt, string currentValue, string errorMessage)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (decimal.TryParse(input, out decimal number) && number >= 0)
            {
                return input.Trim();
            }

            MenuHelpers.Error(errorMessage);
        }
    }

    private static string PromptOptionalNonNegativeWholeNumber(string prompt, string currentValue, string errorMessage)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (string.IsNullOrWhiteSpace(input))
            {
                return currentValue;
            }

            if (long.TryParse(input, out long number) && number >= 0)
            {
                return input.Trim();
            }

            MenuHelpers.Error(errorMessage);
        }
    }

    private static string PromptValidCategoryIdForEdit(List<CategoryModel> categories, long currentCategoryId)
    {
        if (categories.Count == 0)
        {
            MenuHelpers.Warn("No categories found. Keeping current category.");
            return currentCategoryId.ToString();
        }

        MenuHelpers.Announce("Available categories:");
        foreach (var category in categories)
        {
            string currentMarker = category.CategoryID == currentCategoryId ? " (current)" : string.Empty;
            MenuHelpers.Confirm($"{category.CategoryID} - {category.Name}{currentMarker}");
        }

        while (true)
        {
            string? categoryInput = MenuHelpers.Prompt($"Choose category ID (Enter to keep: {currentCategoryId}):");
            if (string.IsNullOrWhiteSpace(categoryInput))
            {
                return currentCategoryId.ToString();
            }

            if (long.TryParse(categoryInput, out long categoryId)
                && categories.Any(c => c.CategoryID == categoryId))
            {
                return categoryId.ToString();
            }

            MenuHelpers.Error("Invalid category ID. Choose one from the list above or press Enter to keep the current value.");
        }
    }
}
