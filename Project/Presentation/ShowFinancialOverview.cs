public static class ShowFinancialOverview
{
    public static void Start()
    {
        Console.WriteLine("--- FINANCIAL OVERVIEW ---\n");
        Console.WriteLine("Choose date range (DD-MM-YYYY): ");
        Console.WriteLine("Enter 1 to return to Admin Menu");
        string? firstDateInput = Console.ReadLine();
        string firstDate = firstDateInput ?? string.Empty;
        if (firstDate == "1")
        {
            AdminMenu.Start();
            return;
        }
        string[] dates = firstDate.Split('-');
        string? secondDateInput = Console.ReadLine();
        string secondDate = secondDateInput ?? string.Empty;
        string[] dates2 = secondDate.Split('-');
        if (dates.Length != 3 || dates2.Length != 3)
        {
            Console.WriteLine("Invalid date format. Please use DD-MM-YYYY.");
            Start();
            return;
        }
        try
        {
            DateTime startDate = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
            DateTime endDate = new DateTime(int.Parse(dates2[2]), int.Parse(dates2[1]), int.Parse(dates2[0]));
            Console.WriteLine($"From {firstDate} to {secondDate} (Sorted by most sold items):\n");
            ShowFinancialOverviewByDate(startDate, endDate);
            MenuHelpers.Prompt("Press Enter to return to Admin Menu");
            AdminMenu.Start();
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid date format. Please use DD-MM-YYYY.");
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
            Console.WriteLine($"Product: {item.ProductName}, Quantity Sold: {item.Quantity}, Total Revenue: {(item.PriceAtPurchase * item.Quantity):0.00} EUR");
        }
    }
}