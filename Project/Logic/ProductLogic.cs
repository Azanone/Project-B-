public class ProductLogic
{
    private readonly ProductAccess _dataAccess = new();

    public List<ProductModel> GetProducts()
    {
        return _dataAccess.GetAll();
    }

    public List<ProductModel> GetProductsForAge(int userAge)
    {
        var allProducts = _dataAccess.GetAll();
        List<ProductModel> filteredProducts = [];
        foreach (var product in allProducts)
        {
            if (product.MinAge <= userAge)
                filteredProducts.Add(product);
        }
        return filteredProducts;
    }
}