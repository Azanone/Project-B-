public static class PastTransactions
{
    public static void Start()
    {
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
}