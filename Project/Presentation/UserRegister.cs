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
        string birthInput = MenuHelpers.PromptUntilValid("What's your birthdate? (dd-mm-yyyy)", AL.ValidateBirthDate);
        DateTime birthDate = AL.ParseBirthDate(birthInput)!.Value;
        string password;
        do
        {
            password = MenuHelpers.PromptPassword("What's your Password (atleast 7 characters)");
        }
        while (!AL.ValidatePassword(password));
        string phoneNumber = MenuHelpers.PromptUntilValid("What's your phone number (only Dutch numbers)", AL.ValidatePhonenumber);
        AL.Register(username, email, password, phoneNumber, birthDate);
        MenuHelpers.Confirm($"Successfully registered as {username}");
        System.Threading.Thread.Sleep(1000);
        Menu.Start();
    }

}
