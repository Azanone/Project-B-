namespace Project.Presentation;

static class UserRegister
{
    public static void Start()
    {
        AccountsLogic AL = new();
        Console.Clear();
        MenuHelpers.Announce("Register your account");

        string firstname = MenuHelpers.PromptUntilValid("What's your first name?", AL.ValidateFirstName);
        string lastname  = MenuHelpers.PromptUntilValid("What's your last name?", AL.ValidateLastName);
        string username = AL.CreateUsername(firstname, lastname);
        string email = MenuHelpers.PromptUntilValid("What's your Email", AL.ValidateEmail);
        string password = MenuHelpers.PromptUntilValid("What's your Password (atleast 7 characters)", AL.ValidatePassword);
        string phoneNumber = MenuHelpers.PromptUntilValid("What's your phone number (only Dutch numbers)", AL.ValidatePhonenumber);
        string birthDate = MenuHelpers.PromptUntilValid("What's your BirthDate? (DD-MM-YYYY)", AL.ValidateBirthDate);
        AL.Register(username, email, password, phoneNumber);
        MenuHelpers.Confirm($"Successfully registered as {username}");
        System.Threading.Thread.Sleep(1000);
        Menu.Start();
    }

}
