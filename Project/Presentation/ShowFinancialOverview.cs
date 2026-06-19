public static class ShowFinancialOverview
{
    public static void Start()
    {
        Console.Clear();
        MenuHelpers.Announce("--- FINANCIAL OVERVIEW ---");
        MenuHelpers.Line("Pick a START date and an END date to see the sales in that range.\n");

        DateTime startDate = DatePicker.PickDate("START DATE");
        DateTime endDate = DatePicker.PickDate("END DATE");

        if (endDate < startDate)
        {
            (startDate, endDate) = (endDate, startDate);
        }

        Console.Clear();
        MenuHelpers.Announce("--- FINANCIAL OVERVIEW ---");
        MenuHelpers.Line($"Range: {startDate:dd-MM-yyyy}  ->  {endDate:dd-MM-yyyy}   (sorted by most sold)\n");

        ShowFinancialOverviewByDate(startDate, endDate);

        MenuHelpers.Prompt("\nPress Enter to return to Admin Menu");
    }

    public static void ShowFinancialOverviewByDate(DateTime startDate, DateTime endDate)
    {
        PurchaseItemLogic logic = new();
        List<PurchaseItemModel> filteredItems = logic.GetDateRange(startDate, endDate);
        List<PurchaseItemModel> sortedItems = logic.SortMostSolditems(filteredItems);

        if (sortedItems.Count == 0)
        {
            MenuHelpers.Warn("No sales found in this date range.");
            return;
        }

        MenuHelpers.Line($"{"PRODUCT",-25}{"SOLD",6}{"REVENUE",17}");
        MenuHelpers.Line(new string('-', 48));

        decimal totalRevenue = 0;
        foreach (PurchaseItemModel item in sortedItems)
        {
            decimal revenue = item.PriceAtPurchase * item.Quantity;
            totalRevenue += revenue;
            string name = item.ProductName.Length > 24 ? item.ProductName.Substring(0, 24) : item.ProductName;
            MenuHelpers.Line($"{name,-25}{item.Quantity,6}{revenue,13:0.00} EUR");
        }

        MenuHelpers.Line(new string('-', 48));
        MenuHelpers.Announce($"{"TOTAL",-25}{"",6}{totalRevenue,13:0.00} EUR");
    }
}
