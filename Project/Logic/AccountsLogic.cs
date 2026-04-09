

//This class is not static so later on we can use inheritance and interfaces
using System.Security.Cryptography.X509Certificates;
using System.Net.Mail;

public class AccountsLogic
{

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //public set, so this can only be set by the class itself
    public static AccountModel? CurrentAccount { get; private set; }
    public AccountAccess _access = new();

    public AccountsLogic()
    {
        // Could do something here

    }

    public AccountModel? CheckLogin(string? email, string? password)
    {


        AccountModel? acc = _access.GetByEmail(email);
        if (acc != null && acc.Password == password)
        {
            CurrentAccount = acc;
            return acc;
        }
        return null;
    }


    public bool ValidatePassword(string field)
    {
        return field.Length >= 7;
    }
    public bool ValidateEmail(string field)
    {
        try
        {
            // int AtPosition = 0;
            // int DotPosition = 0;
            // bool containsDigitsAfterAt = false;

            // for (int i = 0; i <= field.Length - 1; i++)
            // {
            //     if (field[i] == '@')
            //     {
            //         AtPosition = i;
            //     }
            //     else if (field[i] == '.')
            //     {
            //         DotPosition = i;
            //     }
            //     else if ( i > AtPosition &&  AtPosition < DotPosition && Char.IsDigit(field[i]))
            //     {
            //         containsDigitsAfterAt = true;
            //     }
            // }
            // return AtPosition < DotPosition && !containsDigitsAfterAt;
            AccountAccess AA = new();
            MailAddress address = new MailAddress(field);
            if (AA.GetByEmail(field) != null)
            {
                MenuHelpers.Error($"An account already exists with this email: {field}");
                return false;
            }
            return address.Address == field;
        }
        catch (Exception e)
        {
            MenuHelpers.Error($"Exception caught at ValidateEmail: {e.ToString()}");
            return false;
        }
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
        AccountAccess AA = new();
        if (AA.GetByUsername(field) != null)
        {
            MenuHelpers.Error($"An account already exists with this Username: {field}");
            return false;
        }
        return !field.Any(x => Char.IsDigit(x));
    }
    public void Register(string un, string em, string pass, string phone)
    {
        AccountModel account = new(em.ToLower(), pass.ToLower(), un.ToLower(), phone);
        AccountAccess AA = new();
        AA.Write(account);
    }
}




