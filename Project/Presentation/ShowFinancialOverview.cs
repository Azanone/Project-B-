public static class ShowFinancialOverview
{
    public static void Start()
    {
        MenuHelpers.Announce("--- FINANCIAL OVERVIEW ---\n");
        MenuHelpers.Confirm("Choose date range (DD-MM-YYYY): ");
        MenuHelpers.Confirm("Enter 1 to return to Admin Menu");
        string? firstDate = MenuHelpers.Prompt("");
        if (firstDate == "1")
        {
            AdminMenu.Start();
            return;
        }
        string[] dates = firstDate.Split('-');
        string? secondDate = MenuHelpers.Prompt("");
        string[] dates2 = secondDate.Split('-');
        if (dates.Length != 3 || dates2.Length != 3)
        {
            MenuHelpers.Warn("Invalid date format. Please use DD-MM-YYYY.");
            Start();
            return;
        }
        try
        {
            DateTime startDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
            DateTime endDate = new DateTime(int.Parse(dates2[2]), int.Parse(dates2[1]), int.Parse(dates2[0]));
            MenuHelpers.Announce($"From {firstDate} to {secondDate} (Sorted by most sold items):\n");
            ShowFinancialOverviewByDate(startDate, endDate);
            MenuHelpers.Confirm("\nEnter 1 to return to Admin Menu");
            string? input = MenuHelpers.Prompt("");
            if (input == "1")
            {
                AdminMenu.Start();
            }
        }
        catch (FormatException)
        {
            MenuHelpers.Warn("Invalid date format. Please use DD-MM-YYYY.");
            Start();
        }
    }

    public static void ShowFinancialOverviewByDate(DateTime startDate, DateTime endDate)
    {
        PurchaseItemLogic logic = new();
        List<PurchaseItemModel> filteredItems = logic.GetDateRange(startDate, endDate);
        List<PurchaseItemModel> sortedItems = logic.SortMostSolditems(filteredItems);

        foreach(PurchaseItemModel item in sortedItems)
        {
            MenuHelpers.Confirm($"Product: {item.ProductName}, Quantity Sold: {item.Quantity}, Total Revenue: {(item.PriceAtPurchase * item.Quantity):0.00} EUR");
        }
    }
}
