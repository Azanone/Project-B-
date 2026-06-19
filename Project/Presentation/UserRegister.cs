using System;
using System.Collections.Generic;

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

        while (true)
        {
            form.Start();
            List<string> results = form.GetValues();

            string username = results[0].Trim();
            string email = results[1].Trim();
            string password = results[2].Trim();
            string phoneNumber = results[3].Trim();
            string bDate = results[4].Trim();

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

            if (!isValid)
            {
                MenuHelpers.Pause();
                continue;
            }

            Console.Clear();
            string confirmEmail = MenuHelpers.Prompt("Confirm email") ?? string.Empty;
            if (!string.Equals(confirmEmail.Trim(), email, StringComparison.OrdinalIgnoreCase))
            {
                MenuHelpers.Error("Email addresses do not match.");
                MenuHelpers.Prompt("Press Enter to try again");
                continue;
            }

            List<string> verifyOptions = new List<string>
            {
                "Verify my email now (recommended)",
                "Skip email verification for now"
            };
            MenuNavigation verifyMenu = new MenuNavigation(verifyOptions, "Do you want to verify your email?");
            int verifyChoice = verifyMenu.Start();

            if (verifyChoice == 0)
            {
                string verificationCode = AL.GenerateVerificationCode();

                try
                {
                    AL.SendVerificationEmail(email, verificationCode);
                }
                catch (Exception ex)
                {
                    MenuHelpers.Error(ex.Message);
                    MenuHelpers.Prompt("Press Enter to try again");
                    continue;
                }

                MenuHelpers.Confirm("A verification code has been sent to your email.");
                string enteredCode = MenuHelpers.Prompt("Enter the 6-digit verification code") ?? string.Empty;

                if (!string.Equals(enteredCode.Trim(), verificationCode, StringComparison.Ordinal))
                {
                    MenuHelpers.Error("Email verification failed. Incorrect code.");
                    MenuHelpers.Prompt("Press Enter to try again");
                    continue;
                }
            }
            else
            {
                MenuHelpers.Warn("Skipping email verification. You can verify your email later.");
            }

            AL.Register(username, email, password, phoneNumber, bDate);
            MenuHelpers.Confirm($"Successfully registered as {username}");
            System.Threading.Thread.Sleep(1000);
            Dashboard.Start();
            return;
        }
    }
}