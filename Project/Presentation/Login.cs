static class Login
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();

        List<string> labels = new List<string>
        {
            "Email or Username",
            "Password"
        };

        List<bool> requiresInput = new List<bool>
        {
            true,
            true
        };

        MenuNavigation form = new MenuNavigation(labels, requiresInput, @" ____   __   ____  _  _  __     __   __ _    ____  ____  __  ____  ____ 
(  _ ╲ ╱ _╲ (  _ ╲( ╲╱ )(  )   ╱  ╲ (  ( ╲  ╱ ___)(_  _)╱  ╲(  _ ╲(  __)
 ) _ (╱    ╲ ) _ ( )  ╱ ╱ (_╱╲(  O )╱    ╱  ╲___ ╲  )( (  O ))   ╱ ) _) 
(____╱╲_╱╲_╱(____╱(__╱  ╲____╱ ╲__╱ ╲_)__)  (____╱ (__) ╲__╱(__╲_)(____)

Welcome to the login page");
        form.Start();
        List<string> results = form.GetValues();

        string identifier = results[0];
        string password = results[1];

        AccountModel account = accountsLogic.CheckLogin(identifier, password);
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
            
            System.Threading.Thread.Sleep(1500);
            Login.Start();
        }
    }
}