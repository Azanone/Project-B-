public class ProductLogic
{
    private readonly ProductAccess _dataAccess = new();

    public List<ProductModel> GetProducts()
    {
        var allProducts = _dataAccess.GetAll();
        return allProducts;
    }
}