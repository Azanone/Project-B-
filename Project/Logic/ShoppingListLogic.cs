namespace Project.Logic;

public class ShoppingListLogic
{
    private readonly List<Models.ShoppingCartItem> _items = new();
    private readonly List<Models.ShoppingListModel> _FullShop = new();

    public void AddItem(Models.ShoppingCartItem item)
    {
        if (item == null)
            throw new ArgumentException("Item doesn't exist");

        var existingItem = _items.FirstOrDefault(i => i.Product == item.Product);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            _items.Add(item);
        }
    }

    public void RemoveItem(Models.ShoppingCartItem item)
    {
        _items.Remove(item);
    }

    public int ChangeCount(Models.ShoppingCartItem item, int newQuantity)
    {
        if (newQuantity <= 0)
        {
            throw new ArgumentException("Quantity cannot be negative");
        }
        item.Quantity = newQuantity;
        return item.Quantity;
    }

    public List<Models.ShoppingCartItem> GetAllItems()
    {
        return _items;
    }

    public List<Models.ShoppingListModel> GetAllShopItems()
    {
        return _FullShop;
    }
}