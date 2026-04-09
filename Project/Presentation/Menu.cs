static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to register");
        Console.WriteLine("Enter 3 to continue as guest");


        string input = Console.ReadLine() ?? string.Empty;
        if (input == "1")
        {
            Login.Start();
        }
        else if (input == "2")
        {
            UserRegister.Start();
        }
        else if (input == "3")
        {
            Dashboard.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}