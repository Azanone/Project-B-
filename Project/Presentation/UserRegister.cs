static class UserRegister
{
    public static void Start()
    {
        AccountsLogic AL = new AccountsLogic();
        Console.Clear();

        List<string> labels = new List<string>
        {
            "Username",
            "Email",
            "Password",
            "Phone number",
            "Birthdate"
        };

        List<bool> requiresInput = new List<bool>
        {
            true,
            true,
            true,
            true,
            true
        };

        MenuNavigation form = new MenuNavigation(labels, requiresInput, "Register your account");
        form.Start();
        List<string> results = form.GetValues();

        string username = results[0];
        string email = results[1];
        string password = results[2];
        string phoneNumber = results[3];
        string bDate = results[4];

        bool isValid = true;

        if (!AL.ValidateUsername(username))
        {
            isValid = false;
        }
        else if (!AL.ValidateEmail(email))
        {
            isValid = false;
        }
        else if (!AL.ValidatePassword(password))
        {
            isValid = false;
        }
        else if (!AL.ValidatePhonenumber(phoneNumber))
        {
            isValid = false;
        }
        else if (!AL.ValidateBirthday(bDate))
        {
            isValid = false;
        }

        if (isValid)
        {
            AL.Register(username, email, password, phoneNumber, bDate);
            MenuHelpers.Confirm($"Successfully registered as {username}");
            System.Threading.Thread.Sleep(1000);
            Menu.Start();
        }
        else
        {
            MenuHelpers.Prompt("Press Enter to try again");
            UserRegister.Start();
        }
    }
}