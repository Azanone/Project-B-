public static class ShowReceipt
{
    static public void Start()
    {
        ShowAll();
        Console.WriteLine("Enter 1 to return to Admin Menu");
        string input = Console.ReadLine();
        if (input == "1")
        {
            AdminMenu.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }

    public static void ShowAll()
    {
        ReceiptLogic logic = new();
        List<ReceiptModel> receipts = logic.GetPurchases();

        var grouped = from r in receipts group r by r.ReceiptID;

        Console.WriteLine("--- ALL RECEIPTS ---\n");

        foreach (var group in grouped)
        {
            ReceiptModel first = group.First();

            Console.WriteLine(@"  ___________________________________________");
            Console.WriteLine(@" /                                           \");
            Console.WriteLine(@"|   *************************************** |");
            Console.WriteLine(@"\_  * BabylonMarkt                        * |");
            Console.WriteLine(@"  | *************************************** |");
            Console.WriteLine($"  |  Date: {first.CreatedAt:dd-MM-yyyy}                       |");
            Console.WriteLine($"  |  Receipt No: #{first.ReceiptID,-23}   |");
            Console.WriteLine(@"  | --------------------------------------- |");
            Console.WriteLine($"  |  {"ITEM",-27}{"PRICE",-11} |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            
            foreach (ReceiptModel item in group)
            {
                string name = item.ProductName.Length > 25 ? item.ProductName.Substring(0, 25) : item.ProductName;
                Console.WriteLine($"  |  {name,-27}{item.ProductPrice + " EUR",-11}  |");
            }
            
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine($"  |  {"SUBTOTAL:",-27}{first.TotalPrice + " EUR",-11}  |");
            Console.WriteLine($"  |  {"TAX (VAT):",-27}{first.VAT + " EUR",-11}  |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine($"  |  {"TOTAL:",-27}{first.TotalPrice + first.VAT + " EUR",-11}  |");
            Console.WriteLine(@"  | ---------------------------------------  |");
            Console.WriteLine(@"  |         THANK YOU FOR VISITING!           |");
            Console.WriteLine(@"  | __________________________________________|___");
            Console.WriteLine(@"  | /                                            /");
            Console.WriteLine(@"  \_/___________________________________________/");
        }
    }
}