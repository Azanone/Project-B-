using System.Globalization;

public class AdminLogic
{
    private readonly ProductAccess _productAccess = new();

    public List<ProductModel> GetProducts()
    {
        return _productAccess.GetAll();
    }

    public ProductModel? GetProductById(long productId)
    {
        return _productAccess.GetById(productId);
    }

    public List<CategoryModel> GetCategories()
    {
        return _productAccess.GetCategories();
    }

    public (bool Success, string Message) AddProduct(string name, string priceInput, string brand, string ingredients, string categoryIdInput, string stockInput)
    {
        if (!TryBuildProduct(name, priceInput, brand, ingredients, categoryIdInput, stockInput, out ProductModel? product, out string errorMessage))
        {
            return (false, errorMessage);
        }

        try
        {
            long insertedId = _productAccess.Write(product!);
            return (true, $"Product added successfully with ID {insertedId}.");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to add product: {ex.Message}");
        }
    }

    public (bool Success, string Message) UpdateProduct(string productIdInput, string name, string priceInput, string brand, string ingredients, string categoryIdInput, string stockInput)
    {
        if (!long.TryParse(productIdInput, out long productId) || productId <= 0)
        {
            return (false, "Product ID must be a positive number.");
        }

        try
        {
            if (_productAccess.GetById(productId) == null)
            {
                return (false, $"No product found with ID {productId}.");
            }

            if (!TryBuildProduct(name, priceInput, brand, ingredients, categoryIdInput, stockInput, out ProductModel? product, out string errorMessage))
            {
                return (false, errorMessage);
            }

            product!.ProductID = productId;
            bool updated = _productAccess.Update(product);
            return updated
                ? (true, "Product updated successfully.")
                : (false, "Product update failed.");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to update product: {ex.Message}");
        }
    }

    public (bool Success, string Message) RemoveProduct(string productIdInput)
    {
        if (!long.TryParse(productIdInput, out long productId) || productId <= 0)
        {
            return (false, "Product ID must be a positive number.");
        }

        try
        {
            bool deleted = _productAccess.Delete(productId);
            return deleted
                ? (true, "Product removed successfully.")
                : (false, $"No product found with ID {productId}.");
        }
        catch (Exception ex)
        {
            return (false, $"Failed to remove product: {ex.Message}");
        }
    }

    private bool TryBuildProduct(string name, string priceInput, string brand, string ingredients, string categoryIdInput, string stockInput, out ProductModel? product, out string errorMessage)
    {
        product = null;
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(name))
        {
            errorMessage = "Product name is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(brand))
        {
            errorMessage = "Brand is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(ingredients))
        {
            errorMessage = "Ingredients are required.";
            return false;
        }

        if (!TryParsePrice(priceInput, out decimal price))
        {
            errorMessage = "Price must be a valid number.";
            return false;
        }

        if (price < 0)
        {
            errorMessage = "Price cannot be negative.";
            return false;
        }

        if (!long.TryParse(categoryIdInput, out long categoryId) || categoryId <= 0)
        {
            errorMessage = "Category ID must be a positive number.";
            return false;
        }

        bool categoryExists = _productAccess.GetCategories().Any(c => c.CategoryID == categoryId);
        if (!categoryExists)
        {
            errorMessage = "Selected category does not exist.";
            return false;
        }

        if (!long.TryParse(stockInput, out long stock) || stock < 0)
        {
            errorMessage = "Stock must be zero or a positive number.";
            return false;
        }

        product = new ProductModel
        {
            Name = name.Trim(),
            Price = price,
            Brand = brand.Trim(),
            Ingredients = ingredients.Trim(),
            CategoryID = categoryId,
            Stock = stock
        };

        return true;
    }

    private bool TryParsePrice(string input, out decimal price)
    {
        if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out price))
        {
            return true;
        }

        return decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out price);
    }
}
