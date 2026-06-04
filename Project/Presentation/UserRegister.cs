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
            "Phone number",
            "Birthdate"
        };

        List<bool> requiresInput = new List<bool>
        {
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
        string phoneNumber = results[2];
        string bDate = results[3];

        bool isValid = true;

        if (!AL.ValidateUsername(username))
        {
            isValid = false;
        }
        else if (!AL.ValidateEmail(email))
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

        if (!isValid)
        {
            MenuHelpers.Prompt("Press Enter to try again");
            UserRegister.Start();
            return;
        }

        string confirmEmail = PromptMatchingEmail("Confirm email", email);
        _ = confirmEmail;

        string verificationCode = AL.GenerateVerificationCode();

        try
        {
            AL.SendVerificationEmail(email, verificationCode);
        }
        catch (Exception ex)
        {
            MenuHelpers.Error(ex.Message);
            MenuHelpers.Prompt("Press Enter to try again");
            UserRegister.Start();
            return;
        }

        MenuHelpers.Confirm("A verification code has been sent to your email.");

        string enteredCode = PromptUntilValid("Enter the 6-digit verification code", code => code == verificationCode);
        if (!string.Equals(enteredCode, verificationCode, StringComparison.Ordinal))
        {
            MenuHelpers.Error("Email verification failed.");
            MenuHelpers.Prompt("Press Enter to try again");
            UserRegister.Start();
            return;
        }

        string password = PromptPasswordWithConfirmation(AL);

        AL.Register(username, email, password, phoneNumber, bDate);
        MenuHelpers.Confirm($"Successfully registered as {username}");
        System.Threading.Thread.Sleep(1000);
        Menu.Start();
    }

    private static string PromptUntilValid(string prompt, Func<string, bool> validate)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                string value = input.Trim();
                if (validate(value))
                {
                    return value;
                }
            }

            MenuHelpers.Warn("Input is required.");
        }
    }

    private static string PromptMatchingEmail(string prompt, string email)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input) && string.Equals(input.Trim(), email, StringComparison.OrdinalIgnoreCase))
            {
                return input.Trim();
            }

            MenuHelpers.Error("Email addresses do not match.");
        }
    }

    private static string PromptPasswordWithConfirmation(AccountsLogic accountsLogic)
    {
        while (true)
        {
            string password = MenuHelpers.PromptSecret("Password");
            if (!accountsLogic.ValidatePassword(password))
            {
                continue;
            }

            string confirmPassword = MenuHelpers.PromptSecret("Confirm password");
            if (string.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                return password;
            }

            MenuHelpers.Error("Passwords do not match.");
        }
    }
}