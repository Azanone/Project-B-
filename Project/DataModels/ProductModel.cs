public class ProductModel
{
    public Int64 ProductID {get; set;}
    public string Name {get; set;}
    public Decimal Price {get; set;}
    public string Brand {get; set;}
    public string Ingredients {get; set;}
    public Int64 CategoryID {get; set;}
    public string Category {get; set;}
    public Int64 Stock {get; set;}
    public Int64 MinAge {get; set;}
    public Int64 Calories {get; set;}
    public double Fats {get; set;}
    public double Carbs {get; set;}
    public double Fiber {get; set;}
    public double Protein {get; set;}
    public double Salt {get; set;}
    public Int64 PurchaseCount {get; set;}

    public ProductModel(Int64 productid, string name, Decimal price, string brand, string ingredients, Int64 categoryid, Int64 stock)
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