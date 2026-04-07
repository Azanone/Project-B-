public class ProductModel
{
    public Int64 ProductID {get; set;}
    public string Name {get; set;}
    public Decimal Price {get; set;}
    public string Brand {get; set;}
    public string Ingredients {get; set;}
    public Int64 CategoryID {get; set;}
    public string Category {get; set;}

    public ProductModel(Int64 productid, string name, Decimal price, string brand, string ingredients, Int64 categoryid)
    {
        ProductID = productid;
        Name = name;
        Price = price;
        Brand = brand;
        Ingredients = ingredients;
        CategoryID = categoryid;
    }

    public ProductModel() { }
}