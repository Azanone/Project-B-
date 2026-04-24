namespace Project.Models;

public class ShoppingListModel
{
    public string Name { get; set; }
    public string Category { get; set; }

    public decimal Price { get; set; } = 0.01M;

    public decimal MinPrice
    {
        get => Price <= 0 ? 0.01M : Price;
        set { if (value > 0) Price = value; }
    }

    public string Brand { get; set; }
    public string Ingredients { get; set; }

    public ShoppingListModel(string name, string category, decimal price, string brand, string ingredients)
    {
        Name = name;
        Category = category;
        Price = price;
        Brand = brand;
        Ingredients = ingredients;
    }

    public override string ToString()
    {
        return $"{Name} - {Category} - {Brand} - {Ingredients} - {Price}";
    }
}