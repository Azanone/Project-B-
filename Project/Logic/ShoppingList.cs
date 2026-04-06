namespace Project.Logic;

public class ShoppingList<T>
{
    public List<T> ShopList { get; set; } = new();
    public string Name { get; set; }
    public string Category { get; set; }

    public decimal Price { get; set; } = 0.01M;
    public decimal _MinPrice {
        get => Price <= 0 ? 0.01M : Price;
        set { if (value > 0) Price = value; }
    }  
    public string Brand { get; set; }
    public string Ingredients { get; set; }

    public ShoppingList(List<T> shopList, string name, string category, decimal price, string brand, string ingredients)
    {
        ShopList = shopList;
        Name = name;
        Category = category;
        Price = price;
        Brand = brand;
        Ingredients = ingredients;
    }
}