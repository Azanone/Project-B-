static class AdminMenu
{
    static public void Start()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;
        if (account != null)
        {
            Console.WriteLine("Welcome back " + account.FullName);
        }

        Console.WriteLine("Enter 1 to see all past transactions");
        Console.WriteLine("Enter 2 to see a list of all the products");
        Console.WriteLine("Enter 3 to see the store layout");
        Console.WriteLine("Enter 4 to see all the offers");
        Console.WriteLine("Enter 5 to see the stock of the products");
        Console.WriteLine("Enter 6 to log-out");

        string input = Console.ReadLine();
        if (input == "1")
        {
            PastTransactions.Start();
        }
        else if (input == "2")
        {
            ShowProducts.Start();
        }
        else if (input == "3")
        {
            ShowLayout.Start();
        }
        else if (input == "4")
        {
            ShowOffers.Start();
        }
        else if (input == "5")
        {
            ShowStock.Start();
        }
        else if (input == "6")
        {
            Menu.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }

}