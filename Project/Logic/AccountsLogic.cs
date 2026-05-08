

//This class is not static so later on we can use inheritance and interfaces

using System.ComponentModel;
using System.Net.Mail;
using Project.Logic;
using Project.Presentation;

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

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password)) return null;
        
        AccountModel? acc = _access.GetByIdentifier(identifier.ToLower());
        
        if (acc != null && PasswordSecurityLogic.VerifyPassword(password, acc.Password))
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
    
    public bool ValidateFirstName(string firstname)
    {
        if (string.IsNullOrWhiteSpace(firstname) || firstname.Length < 3)
        {
            MenuHelpers.Error("First name must be at least 3 characters long");
            return false;
        }

        if (string.IsNullOrWhiteSpace(firstname) || firstname.Length > 32)
        {
            MenuHelpers.Error("First name can't be longer than 32 characters");
            return false;
        }
        
        return true;
    }
    
    public bool ValidateLastName(string lastname)
    {
        if (string.IsNullOrWhiteSpace(lastname) || lastname.Length < 3)
        {
            MenuHelpers.Error("Last name must be at least 3 characters long");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(lastname) || lastname.Length > 32)
        {
            MenuHelpers.Error("Last name can't be longer than 32 characters");
            return false;
        }
        
        return true;
    }
    
    public string CreateUsername(string firstname, string lastname)
    {
        if (firstname.Contains(" "))
        {
            firstname = firstname.Replace(" ", "");
        }
        
        if (lastname.Contains(" "))
        {
            lastname = lastname.Replace(" ", "");
        }
        
        firstname = char.ToUpper(firstname[0]) + firstname.Substring(1).ToLower();
        lastname = char.ToUpper(lastname[0]) + lastname.Substring(1).ToLower();
        
        string username =  $"{firstname} {lastname}";
        
        if (IdentifierExists(username.ToLower())) 
        {
            MenuHelpers.Error($"Username {username} already exists");
        }
        
        return username;
    }

    public bool ValidateEmail(string email)
    {
        try
        {
            MailAddress address = new MailAddress(email);
            if (IdentifierExists(email.ToLower()))
            {
                MenuHelpers.Error($"Email {email} already exists");
                return false;
            }
            if (!address.Host.Contains(".") || address.Host.Split('.').Last().Length < 2)
            {
                MenuHelpers.Error($"Email {email} uses invalid format");
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

    public bool ValidatePhonenumber(string field)
    {
        if (string.IsNullOrWhiteSpace(field)) return false;

        try
        {
            if (field.Length == 10 && field.StartsWith("06"))
            {
                return field.All(char.IsDigit);
            }

            if (field.Length == 12 && field.StartsWith("+316"))
            {
                return field.Substring(1).All(char.IsDigit);
            }
            return false;
        }
        catch (Exception e)
        {
            MenuHelpers.Error($"Validation error: {e.Message}");
            return false;
        }
    }

    public bool ValidateBirthDate(string date)
    {
        if (string.IsNullOrWhiteSpace(date)) return false;
        try
        {
            if(date)
        }
        
    }
    public void Register(string username, string email, string password, string phoneNumber)
    {
        string hashedPassword = PasswordSecurityLogic.HashPassword(password);
        
        AccountModel newAccount = new AccountModel(
            username.ToLower(), 
            email.ToLower(), 
            hashedPassword,
            username, 
            string.Empty, 
            "0", 
            phoneNumber);
        _access.Write(newAccount);
        CurrentAccount = newAccount;
    }
}
