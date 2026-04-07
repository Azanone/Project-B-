namespace Project.DataModels;

public class ShoppingListModel
{
    private readonly List<ShoppingListModel> _items = new();

    public void AddItem(ShoppingListModel item)
    {
        _items.Add(item);
    }

    public void RemoveItem(ShoppingListModel item)
    {
        _items.Remove(item);
    }

    public List<ShoppingListModel> GetAllItems()
    {
        return _items;
    }
}