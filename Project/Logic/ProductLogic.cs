public class ProductLogic
{
    private readonly ProductAccess _dataAccess = new();

    public List<ProductModel> GetProducts()
    {
        return _dataAccess.GetAll();
    }

    public static bool IsOldEnoughForProduct(ProductModel product, int userAge)
    {
        return userAge >= product.MinAge;
    }
}