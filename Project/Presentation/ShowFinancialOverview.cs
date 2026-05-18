public static class ShowFinancialOverview
{
    public static void Start()
    {
        MenuHelpers.Announce("--- FINANCIAL OVERVIEW ---\n");
        MenuHelpers.Confirm("Enter 1 to return to Admin Menu\n ");
        MenuHelpers.Confirm("Choose date range (DD-MM-YYYY): ");

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
        if (int.Parse(dates[1]) < 1 || int.Parse(dates[1]) > 12 || int.Parse(dates2[1]) < 1 || int.Parse(dates2[1]) > 12)
        {
            MenuHelpers.Warn("Invalid month. Please use a month between 01 and 12.");
            Start();
            return;
        }
        if (int.Parse(dates[0]) < 1 || int.Parse(dates[0]) > 31 || int.Parse(dates2[0]) < 1 || int.Parse(dates2[0]) > 31)
        {
            MenuHelpers.Warn("Invalid day. Please use a day between 01 and 31.");
            Start();
            return;
        }
        if (int.Parse(dates[2]) < 2000 || int.Parse(dates[2]) > DateTime.Now.Year || int.Parse(dates2[2]) < 2000 || int.Parse(dates2[2]) > DateTime.Now.Year)
        {
            MenuHelpers.Warn($"Invalid year. Please use a year between 2000 and {DateTime.Now.Year}.");
            Start();
            return;
        }
        
        try
        {
            DateTime startDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
            DateTime endDate = new DateTime(int.Parse(dates2[2]), int.Parse(dates2[1]), int.Parse(dates2[0]));
            MenuHelpers.Announce($"From {firstDate} to {secondDate} (Sorted by most sold items):\n");
            ShowFinancialOverviewByDate(startDate, endDate);
            MenuHelpers.PromptReturnToMenu("\nEnter 1 to return to Admin Menu", AdminMenu.Start);
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
