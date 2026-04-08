public static class ShowOffers
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

    static public void ShowAll()
    {
        
    }
}