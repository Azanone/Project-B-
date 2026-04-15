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
        Console.WriteLine("--- ALL RECEIPTS ---");
            Console.WriteLine("********************************");
            Console.WriteLine("* STORE NAME          *");
            Console.WriteLine("* OFFICIAL RECEIPT    *");   
            Console.WriteLine("********************************");
            Console.WriteLine(" Date: 2026-04-15  Time: 14:15");
            Console.WriteLine(" Receipt No: #88291");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(" ITEM              PRICE");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(" Product A         € 10.00");
            Console.WriteLine(" Product B         € 05.50");
            Console.WriteLine(" Product C         € 12.00");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(" SUBTOTAL:         € 27.50");
            Console.WriteLine(" TAX (21%):        € 05.78");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(" TOTAL:            € 33.28");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("     THANK YOU FOR VISITING!");
            Console.WriteLine("********************************");
            Console.WriteLine();

    }
}