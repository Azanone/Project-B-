static class Dashboard
{
    public static void Start()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            Console.WriteLine("Welcome guest");
        }
        else
        {
            Console.WriteLine("Welcome back " + account.FullName);
        }

        Console.WriteLine("Enter 1 to logout");

        string input = Console.ReadLine() ?? string.Empty;
        if (input == "1")
        {
            AccountsLogic accountsLogic = new AccountsLogic();
            accountsLogic.Logout();
            Menu.Start();
        }
        else
        {
            Start();
        }
    }
}