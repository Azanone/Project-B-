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
    public int MinAge { get; set; }

    public ProductModel(Int64 productid, string name, Decimal price, string brand, string ingredients, Int64 categoryid, Int64 stock, int minAge)
    {
        ProductID = (int)productid;
        Name = name;
        Price = price;
        Brand = brand;
        Ingredients = ingredients;
        CategoryID = categoryid;
        Stock = stock;
        MinAge = minAge;
    }

    public ProductModel() { }
}