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

    public void OrderProductByID(int productID, int amount)
    {
        _dataAccess.UpdateStock(productID,amount);
    }
    public bool OrderProductByName(string productName, int amount)
    {
        int productID = _dataAccess.IDSearchByName(productName);
        if (productID == 0)
        {
            return false;
        }
        _dataAccess.UpdateStock(productID,amount);
        return true;
    }
}