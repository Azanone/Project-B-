namespace UnitTests;

[TestClass]
public sealed class AdminProductManagementTests
{
    private readonly AdminLogic _logic = new();
    private readonly ProductAccess _access = new();

    [TestMethod]
    public void AddProductCreatesProductInDatabase()
    {
        List<CategoryModel> categories = _logic.GetCategories();
        if (categories.Count == 0)
        {
            Assert.Inconclusive("No categories are available for product creation.");
        }

        CategoryModel category = categories[0];
        string uniqueName = $"admin_test_{Guid.NewGuid():N}";
        string brand = "Test Brand";
        string ingredients = "Water, Salt";
        const string price = "12.50";
        const string stock = "5";
        const string minAge = "18";

        ProductModel? createdProduct = null;

        try
        {
            (bool success, string message) = _logic.AddProduct(uniqueName, price, brand, ingredients, category.CategoryID.ToString(), stock, minAge);

            Assert.IsTrue(success, message);

            createdProduct = _logic.GetProducts().FirstOrDefault(product =>
                product.Name == uniqueName &&
                product.Brand == brand &&
                product.Ingredients == ingredients);

            Assert.IsNotNull(createdProduct);
            Assert.AreEqual(category.CategoryID, createdProduct.CategoryID);
            Assert.AreEqual(12.50m, createdProduct.Price);
            Assert.AreEqual(5, createdProduct.Stock);
            Assert.AreEqual(18, createdProduct.MinAge);
        }
        finally
        {
            if (createdProduct == null)
            {
                createdProduct = _logic.GetProducts().FirstOrDefault(product =>
                    product.Name == uniqueName &&
                    product.Brand == brand &&
                    product.Ingredients == ingredients);
            }

            if (createdProduct != null)
            {
                _access.Delete(createdProduct.ProductID);
            }
        }
    }
}