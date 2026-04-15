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

            Console.WriteLine("********************************");
            Console.WriteLine("*        STORE NAME           *");
            Console.WriteLine("*      OFFICIAL RECEIPT       *");
            Console.WriteLine("********************************");
            Console.WriteLine($" Date: {first.CreatedAt:yyyy-MM-dd}");
            Console.WriteLine($" Receipt No: #{first.ReceiptID}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($" {"ITEM",-33}{"PRICE"}");
            Console.WriteLine("----------------------------------------");

            foreach (ReceiptModel item in group)
            {
                Console.WriteLine($" {item.ProductName,-33}{item.ProductPrice}");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($" {"SUBTOTAL:",-33}{first.TotalPrice}");
            Console.WriteLine($" {"TAX (VAT):",-33}{first.VAT}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($" {"TOTAL:",-33}{first.TotalPrice + first.VAT}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("     THANK YOU FOR VISITING!");
            Console.WriteLine("****************************************\n");
        }
    }
}