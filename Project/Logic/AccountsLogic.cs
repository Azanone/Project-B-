using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

public class AccountsLogic
{

    public static AccountModel? CurrentAccount { get; private set; }
    private AccountsAccess _access = new();

    public AccountsLogic()
    {

    }

    public AccountModel? CheckLogin(string identifier, string password)
    {

        if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }


        AccountModel? acc = _access.GetByIdentifier(identifier.ToLower());
        if (acc != null && Project.Logic.PasswordSecurityLogic.VerifyPassword(password, acc.Password))
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

    public void UpdateAccount(AccountModel updatedAccount)
    {
        _access.Write(updatedAccount);
        if (CurrentAccount != null && CurrentAccount.Id == updatedAccount.Id)
        {
            CurrentAccount = updatedAccount;
        }
    }

    public DateTime ParseBirthday(string birthdayInput)
    {
        string[] parts = birthdayInput.Split('-');
        if (parts.Length == 3
            && int.TryParse(parts[0], out int day)
            && int.TryParse(parts[1], out int month)
            && int.TryParse(parts[2], out int year))
        {
            try
            {
                return new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }
        throw new FormatException("Invalid date format");
    }

    public int? CalculateAge(string? birthdate)
    {
        if (string.IsNullOrWhiteSpace(birthdate))
        {
            return null;
        }
        DateTime dob;
        try
        {
            dob = ParseBirthday(birthdate);
        }
        catch (FormatException)
        {
            return null;
        }
        DateTime today = DateTime.Today;
        int age = today.Year - dob.Year;
        if (dob.Date > today.AddYears(-age))
        {
            age--;
        }
        return age;
    }

   public bool ValidateBirthday(string birthdayInput)
    {
        if (string.IsNullOrWhiteSpace(birthdayInput))
        {
            MenuHelpers.Error("Birthday cannot be empty");
            return false;
        }
        DateTime birthday;
        try
        {
            birthday = ParseBirthday(birthdayInput);
        }
        catch (FormatException)
        {
            MenuHelpers.Error("Invalid date format (use dd-MM-yyyy)");
            return false;
        }
        if (birthday > DateTime.Today)
        {
            MenuHelpers.Error("Birthday cannot be in the future");
            return false;
        }
        return true;
    }

    public bool ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
        {
            MenuHelpers.Error("Username must be at least 3 characters long");
            return false;
        }
        if (IdentifierExists(username.ToLower()))
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
        if (string.IsNullOrWhiteSpace(password))
        {
            MenuHelpers.Error("Password cannot be empty");
            return false;
        }

        if (password.Length < 7)
        {
            MenuHelpers.Error("Password must be more than 6 characters");
            return false;
        }

        if (!password.Any(char.IsUpper))
        {
            MenuHelpers.Error("Password must contain at least one uppercase letter");
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            MenuHelpers.Error("Password must contain at least one digit");
            return false;
        }

        if (!password.Any(char.IsPunctuation) && !password.Any(char.IsSymbol))
        {
            MenuHelpers.Error("Password must contain at least one sign or symbol");
            return false;
        }

        return true;
    }

    public bool ValidatePhonenumber(string field)
    {
        if (string.IsNullOrWhiteSpace(field))
        {
            return false;
        }
        

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
            MenuHelpers.Error($"Validation error: Invalid phone number");
            return false;
        }
        catch (Exception e)
        {
            MenuHelpers.Error($"Validation error: {e.Message}");
            return false;
        }
    }

    public void Register(string username, string email, string password, string phoneNumber, string bdate)
    {
        string hashedPassword = Project.Logic.PasswordSecurityLogic.HashPassword(password);
        AccountModel newAccount = new AccountModel(username.ToLower(), email.ToLower(), hashedPassword, username, string.Empty, string.Empty, phoneNumber, bdate);
        _access.Write(newAccount);
        CurrentAccount = newAccount;
    }

    public string GenerateVerificationCode()
    {
        return RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
    }

    public void SendVerificationEmail(string email, string verificationCode)
    {
        string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? string.Empty;
        string smtpPortText = Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587";
        string smtpUser = Environment.GetEnvironmentVariable("SMTP_USER") ?? string.Empty;
        string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? string.Empty;
        string fromAddress = Environment.GetEnvironmentVariable("SMTP_FROM") ?? smtpUser;

        if (string.IsNullOrWhiteSpace(smtpHost) && fromAddress.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
        {
            smtpHost = "smtp.gmail.com";
        }

        if (string.IsNullOrWhiteSpace(smtpUser))
        {
            smtpUser = fromAddress;
        }

        if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(fromAddress))
        {
            throw new InvalidOperationException("Email verification is not configured. Set SMTP_FROM, and for non-Gmail accounts also set SMTP_HOST. If your mail server requires authentication, set SMTP_USER and SMTP_PASSWORD too.");
        }

        if (!int.TryParse(smtpPortText, out int smtpPort))
        {
            smtpPort = 587;
        }

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true
        };

        if (!string.IsNullOrWhiteSpace(smtpUser))
        {
            client.Credentials = new NetworkCredential(smtpUser, smtpPassword);
        }

        using var message = new MailMessage(fromAddress, email)
        {
            Subject = "Your verification code",
            Body = $"Your verification code is: {verificationCode}\n\nEnter this 6-digit code to complete your registration."
        };

        client.Send(message);
    }
}