static class AdminMenu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to see all past transactions");
        Console.WriteLine("Enter 2 to see a list of all the products");
        Console.WriteLine("Enter 3 to see the store layout");
        Console.WriteLine("Enter 4 to see all the sales");
        Console.WriteLine("Enter 5 to see the stock of the products");
        Console.WriteLine("Enter 6 to log-out");

        string input = Console.ReadLine();
        if (input == "1")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else if (input == "2")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else if (input == "3")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else if (input == "4")
        {
            Console.WriteLine("This feature is not yet implemented");
        }
        else if (input == "5")
        {
            Console.WriteLine("This feature is not yet implemented");
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