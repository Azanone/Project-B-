
class UserRegister
{
    public void Register()
    {
        AccountsLogic AL = new();
        MenuHelpers.Announce("Register your account");

        string username = MenuHelpers.PromptUntilValid("What's your username", AL.ValidateUsername);
        string email = MenuHelpers.PromptUntilValid("What's your Email", AL.ValidateEmail);
        string password = MenuHelpers.PromptUntilValid("What's your Password (atleast 7 characters)", AL.ValidatePassword);
        string phoneNumber = MenuHelpers.PromptUntilValid("What's your phone number (only Dutch numbers)", AL.ValidatePhonenumber);
        MenuHelpers.Confirm($"Successfully registered as {username}");

        AL.Register(username, email, password, phoneNumber);
    }

}
