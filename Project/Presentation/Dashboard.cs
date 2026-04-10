static class Dashboard
{
    public static void Start()
    {
        Console.Clear();
        AccountModel? account = AccountsLogic.CurrentAccount;

        if (account == null)
        {
            MenuHelpers.Announce("Welcome guest");
        }
        else
        {
            MenuHelpers.Announce("Welcome back " + account.FullName);
        }

        string input = MenuHelpers.Prompt("Enter 1 to logout") ?? string.Empty;
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