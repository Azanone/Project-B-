static class Login
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address or username");
        string identifier = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine() ?? string.Empty;

        AccountModel? account = accountsLogic.CheckLogin(identifier, password);
        if (account != null)
        {
            Dashboard.Start();
        }
        else
        {
            if (accountsLogic.IdentifierExists(identifier))
            {
                Console.WriteLine("Wrong password");
            }
            else
            {
                Console.WriteLine("Wrong email address or username");
            }

            Menu.Start();
        }
    }
}