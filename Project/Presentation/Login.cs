static class Login
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine() ?? string.Empty;

        AccountModel? account = accountsLogic.CheckLogin(email, password);
        if (account != null)
        {
            Dashboard.Start();
        }
        else
        {
            if (accountsLogic.EmailExists(email))
            {
                Console.WriteLine("Wrong password");
            }
            else
            {
                Console.WriteLine("Wrong email");
            }

            Menu.Start();
        }
    }
}