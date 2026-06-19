namespace UnitTests;

[TestClass]
public sealed class AdminProductManagementTests
{
    private readonly AdminLogic _logic = new();
    private readonly ProductAccess _access = new();

    [DataTestMethod]
    [DataRow("admin_test", "Test Brand", "Water, Salt", "12.50", "5", "18", "100", "1.2", "5.0", "0.5", "2.0", "0.3")]
    public void AddProductCreatesProductInDatabase(string namePrefix, string brand, string ingredients, string price, string stock, string minAge, string calories, string fats, string carbs, string fiber, string protein, string salt)
    {
        List<CategoryModel> categories = _logic.GetCategories();
        if (categories.Count == 0)
        {
            Assert.Inconclusive("No categories are available for product creation.");
        }

        CategoryModel category = categories[0];
        string uniqueName = $"{namePrefix}_{Guid.NewGuid():N}";

        ProductModel? createdProduct = null;

        try
        {
            (bool success, string message) = _logic.AddProduct(uniqueName, price, brand, ingredients, category.CategoryID.ToString(), stock, minAge, calories, fats, carbs, fiber, protein, salt);

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