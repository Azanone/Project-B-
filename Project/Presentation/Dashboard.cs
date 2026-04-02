static class Dashboard
{
    public static void Start()
    {
        AccountModel? account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            Menu.Start();
            return;
        }

        Console.WriteLine("Welcome back " + account.FullName);
        Console.WriteLine("Enter 1 to logout");

        string input = Console.ReadLine() ?? string.Empty;
        if (input == "1")
        {
            new AccountsLogic().Logout();
            Menu.Start();
        }
        else
        {
            Start();
        }
    }
}