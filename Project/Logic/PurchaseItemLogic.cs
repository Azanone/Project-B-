public class PurchaseItemLogic
{
    private readonly PurchaseItemAccess _dataAccess = new();

    public List<PurchaseItemModel> GetDateRange(DateTime startDate, DateTime endDate)
    {
        var allItems = _dataAccess.GetDateRange(startDate, endDate);
        return allItems;
    }

    public List<PurchaseItemModel> SortMostSolditems(List<PurchaseItemModel> items)
    {
        return items.OrderByDescending(item => item.TotalRevenue).ToList();
    }
}