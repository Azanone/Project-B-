public class ProductModel
{
    public int ProductID {get; set;}
    public string Name {get; set;}
    public decimal Price {get; set;}
    public string Brand {get; set;}
    public string Ingredients {get; set;}
    public Int64 CategoryID {get; set;}
    public string Category {get; set;}
    public Int64 Stock {get; set;}

    public ProductModel(int productid, string name, Decimal price, string brand, string ingredients, int categoryid, Int64 stock)
    {
        ProductID = productid;
        Name = name;
        Price = price;
        Brand = brand;
        Ingredients = ingredients;
        CategoryID = categoryid;
        Stock = stock;
    }

    public ProductModel() { }
}