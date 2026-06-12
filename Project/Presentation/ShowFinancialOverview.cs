public static class ShowFinancialOverview
{
    public static void Start()
    {
        Console.WriteLine("--- FINANCIAL OVERVIEW ---\n");
        Console.WriteLine("Choose date range (DD-MM-YYYY): ");
        Console.WriteLine("Enter 1 to return to Admin Menu");
       
        try
        {
            DateTime startDate = DatePicker.PickDate();
            DateTime endDate = DatePicker.PickDate();
            Console.WriteLine($"From {startDate} to {endDate} (Sorted by most sold items):\n");
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