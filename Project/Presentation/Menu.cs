static class Menu
{
    static public void Start()
    {
        Console.Clear();
        string Logo = @" ____   __   ____  _  _  __     __   __ _    ____  ____  __  ____  ____ 
(  _ ╲ ╱ _╲ (  _ ╲( ╲╱ )(  )   ╱  ╲ (  ( ╲  ╱ ___)(_  _)╱  ╲(  _ ╲(  __)
 ) _ (╱    ╲ ) _ ( )  ╱ ╱ (_╱╲(  O )╱    ╱  ╲___ ╲  )( (  O ))   ╱ ) _) 
(____╱╲_╱╲_╱(____╱(__╱  ╲____╱ ╲__╱ ╲_)__)  (____╱ (__) ╲__╱(__╲_)(____)";
        List<string> options = new List<string>
        {
            "Login",
            "Register",
            "Continue as guest"
        };

        MenuNavigation menu = new MenuNavigation(options, Logo);
        int selection = menu.Start();

        switch (selection)
        {
            case 0:
                Login.Start();
                break;
            case 1:
                UserRegister.Start();
                break;
            case 2:
                Dashboard.Start();
                break;
        }
    }
}