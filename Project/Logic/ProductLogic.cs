public class ProductLogic
{
    private readonly ProductAccess _dataAccess = new();

    public List<ProductModel> GetProducts()
    {
        var allProducts = _dataAccess.GetAll();
        return allProducts;
    }

    public void OrderProductByID(int productID, int amount)
    {
        _dataAccess.UpdateStock(productID,amount);
    }
    public void OrderProductByName(string productName, int amount)
    {
        int productID = _dataAccess.IDSearchByName(productName);
        _dataAccess.UpdateStock(productID,amount);

    }
}