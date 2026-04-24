using Project.Models;

static class Login
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();
        MenuHelpers.Announce("Welcome to the login page");
        string identifier = MenuHelpers.Prompt("Please enter your email address or username") ?? string.Empty;
        string password = MenuHelpers.Prompt("Please enter your password") ?? string.Empty;

        AccountModel? account = accountsLogic.CheckLogin(identifier, password);
        if (account != null)
        {
            if (string.Equals(account.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminMenu.Start();
            }
            else
            {
                Dashboard.Start();
            }
        }
        
        if (account != null)
        {
            CurrentSession.User = account;

            Dashboard.Start();
        }
        
        else
        {
            if (accountsLogic.IdentifierExists(identifier))
            {
                MenuHelpers.Warn("Wrong password");
            }
            else
            {
                MenuHelpers.Warn("Wrong email address or username");
            }

            // Menu.Start();
        }
    }
}