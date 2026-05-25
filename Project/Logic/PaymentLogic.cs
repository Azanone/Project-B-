public class PaymentLogic
{
    public static readonly string[] Methods = { "iDEAL", "Credit Card", "PayPal", "Cash" };

    public List<string> GetPaymentMethods()
    {
        return Methods.ToList();
    }

    public string? SelectMethod(string input)
    {
        if (!int.TryParse(input, out int choice))
        {
            return null;
        }
        if (choice < 1 || choice > Methods.Length)
        {
            return null;
        }
        return Methods[choice - 1];
    }

    public bool ValidateCardNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return false;
        }
        string digits = number.Replace(" ", "");
        return digits.Length == 16 && digits.All(char.IsDigit);
    }

    public bool ValidateCvv(string cvv)
    {
        if (string.IsNullOrWhiteSpace(cvv))
        {
            return false;
        }
        return cvv.Length == 3 && cvv.All(char.IsDigit);
    }

    public bool ValidateExpiry(string expiry)
    {
        if (string.IsNullOrWhiteSpace(expiry))
        {
            return false;
        }
        string[] parts = expiry.Split('/');
        if (parts.Length != 2
            || !int.TryParse(parts[0], out int month)
            || !int.TryParse(parts[1], out int year))
        {
            return false;
        }
        if (month < 1 || month > 12)
        {
            return false;
        }
        int fullYear = 2000 + year;
        DateTime lastDayOfMonth = new DateTime(fullYear, month, DateTime.DaysInMonth(fullYear, month));
        return lastDayOfMonth >= DateTime.Today;
    }

    public bool ValidatePayPalEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email && address.Host.Contains(".");
        }
        catch
        {
            return false;
        }
    }

    public decimal? CalculateChange(decimal total, decimal tendered)
    {
        if (tendered <= total)
        {
            return null;
        }
        return tendered - total;
    }
}
