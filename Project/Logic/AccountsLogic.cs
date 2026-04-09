

//This class is not static so later on we can use inheritance and interfaces
using System.Net.Mail;

public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public AccountModel? CheckLogin(string identifier, string password)
    {


        AccountModel? acc = _access.GetByIdentifier(identifier);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }

    public bool IdentifierExists(string identifier)
    {
        return _access.GetByIdentifier(identifier) != null;
    }

    public void Logout()
    {
        CurrentAccount = null;
    }

    public bool ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
        {
            MenuHelpers.Error("Username must be at least 3 characters long");
            return false;
        }
        if (IdentifierExists(username))
        {
            MenuHelpers.Error($"Username {username} already exists");
            return false;
        }
        return true;
    }

    public bool ValidateEmail(string email)
    {
        try
        {
            MailAddress address = new MailAddress(email);
            if (IdentifierExists(email))
            {
                MenuHelpers.Error($"Email {email} already exists");
                return false;
            }
            return address.Address == email;
        }
        catch
        {
            MenuHelpers.Error("Invalid email format");
            return false;
        }
    }

    public bool ValidatePassword(string password)
    {
        if (password.Length < 7)
        {
            MenuHelpers.Error("Password must be at least 7 characters");
            return false;
        }
        return true;
    }

    public bool ValidatePhonenumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            MenuHelpers.Error("Phone number cannot be empty");
            return false;
        }
        return true;
    }

    public void Register(string username, string email, string password, string phoneNumber)
    {
        AccountModel newAccount = new AccountModel(0, username, email, password, username, string.Empty, 0, phoneNumber);
        _access.Write(newAccount);
        CurrentAccount = newAccount;
    }
}




