using System.Globalization;

public static class PaymentCheckout
{
    private static readonly PaymentLogic _logic = new();
    private static readonly string[] IdealBanks = { "ABN AMRO", "ING", "Rabobank", "SNS" };

    public static string? Start(decimal total)
    {
        List<string> methods = _logic.GetPaymentMethods();
        int cancelOption = methods.Count + 1;

        while (true)
        {
            Console.Clear();
            MenuHelpers.Announce("--- SELECT PAYMENT METHOD ---");
            MenuHelpers.Confirm($"Amount due: {total} EUR\n");

            for (int i = 0; i < methods.Count; i++)
            {
                MenuHelpers.Confirm($"{i + 1}. {methods[i]}");
            }
            MenuHelpers.Confirm($"{cancelOption}. Cancel");

            string input = MenuHelpers.Prompt("Choose a payment method:") ?? string.Empty;
            if (input.Trim() == cancelOption.ToString())
            {
                return null;
            }

            string? method = _logic.SelectMethod(input.Trim());
            if (method == null)
            {
                MenuHelpers.Error("Invalid payment method selection.");
                MenuHelpers.Pause();
                continue;
            }

            bool paid = method switch
            {
                "Credit Card" => HandleCard(),
                "PayPal" => HandlePayPal(),
                "Cash" => HandleCash(total),
                "iDEAL" => HandleIdeal(),
                _ => false
            };

            if (!paid)
            {
                MenuHelpers.Pause();
                continue;
            }

            MenuHelpers.Confirm($"Payment via {method} accepted.");
            MenuHelpers.Pause();
            return method;
        }
    }

    private static bool HandleCard()
    {
        string number = MenuHelpers.Prompt("Card number (16 digits):") ?? string.Empty;
        if (!_logic.ValidateCardNumber(number.Trim()))
        {
            MenuHelpers.Error("Card number must be exactly 16 digits.");
            return false;
        }

        string expiry = MenuHelpers.Prompt("Expiry (MM/YY):") ?? string.Empty;
        if (!_logic.ValidateExpiry(expiry.Trim()))
        {
            MenuHelpers.Error("Expiry must be MM/YY and not in the past.");
            return false;
        }

        string cvv = MenuHelpers.Prompt("CVV (3 digits):") ?? string.Empty;
        if (!_logic.ValidateCvv(cvv.Trim()))
        {
            MenuHelpers.Error("CVV must be 3 digits.");
            return false;
        }

        return true;
    }

    private static bool HandlePayPal()
    {
        string email = MenuHelpers.Prompt("PayPal email:") ?? string.Empty;
        if (!_logic.ValidatePayPalEmail(email.Trim()))
        {
            MenuHelpers.Error("Invalid PayPal email.");
            return false;
        }
        return true;
    }

    private static bool HandleCash(decimal total)
    {
        string tenderedInput = MenuHelpers.Prompt("Cash tendered (EUR):") ?? string.Empty;
        if (!decimal.TryParse(tenderedInput.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal tendered))
        {
            MenuHelpers.Error("Enter a valid amount.");
            return false;
        }

        decimal? change = _logic.CalculateChange(total, tendered);
        if (change == null)
        {
            MenuHelpers.Error("Insufficient cash for the total.");
            return false;
        }

        MenuHelpers.Confirm($"Change due: {change} EUR");
        return true;
    }

    private static bool HandleIdeal()
    {
        MenuHelpers.Announce("Select your bank:");
        for (int i = 0; i < IdealBanks.Length; i++)
        {
            MenuHelpers.Confirm($"{i + 1}. {IdealBanks[i]}");
        }

        string input = MenuHelpers.Prompt("Bank:") ?? string.Empty;
        if (!int.TryParse(input.Trim(), out int choice) || choice < 1 || choice > IdealBanks.Length)
        {
            MenuHelpers.Error("Invalid bank selection.");
            return false;
        }

        MenuHelpers.Confirm($"Redirecting to {IdealBanks[choice - 1]}...");
        return true;
    }
}
