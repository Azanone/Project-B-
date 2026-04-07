

//This class is not static so later on we can use inheritance and interfaces
using System.Security.Cryptography.X509Certificates;

public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //public set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    public AccountsAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public AccountModel CheckLogin(string email, string password)
    {


        AccountModel acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }


    public bool ValidatePassword(string field)
    {
        return field.Length > 7;
    }
    public bool ValidateEmail(string field)
    {
        try
        {
            
            int AtPosition = 0;
            int DotPosition = 0;
            bool containsDigitsAfterAt = false;
            
            for (int i = 0; i <= field.Length; i++)
            {
                if (field[i] == '@')
                {
                    AtPosition = i;
                }
                else if (field[i] == '.')
                {
                    DotPosition = i;
                }
                else if ( i > AtPosition &&  AtPosition < DotPosition && Char.IsDigit(field[i]))
                {
                    containsDigitsAfterAt = true;
                }
            }
            return AtPosition < DotPosition && !containsDigitsAfterAt;
        }
        catch (Exception e)
        {
            MenuHelpers.Error("Wrong email format");
            return false;
        }
    }
    public bool ValidatePhonenumber(string field)
    {
        try
        {
            return (field.Substring(0, 2) == "06" && field.Substring(2, 10).All(x => Char.IsDigit(x)) && field.Length == 11) || ( field.Length == 13 && field.Substring(0, 4) == "+316" && field.Substring(4, 12).All(x => Char.IsDigit(x)) );        
        }
        catch (Exception e)
        {
            MenuHelpers.Error("Wrong phone format");
            return false;
        }
    }
    public bool ValidatePostalcode(string field)
    {
        return field.Trim().Substring(0,4).All(x => Char.IsDigit(x)) && field.Trim().Substring(4, -1).All(x => !Char.IsDigit(x)) && field.Trim().Length == 6;
    }
    public bool ValidateHousenumber(string field)
    {
        return field.Any(x => Char.IsDigit(x));
    }
    public bool ValidateUsername(string field)
    {
        return !field.Any(x => Char.IsDigit(x));
    }
    public void Register(string un, string em, string pass, string phone)
    {
        return;
    }
}




