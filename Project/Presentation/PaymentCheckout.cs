using System;
using System.Collections.Generic;
using System.Globalization;
using Project.Logic;

public static class PaymentCheckout
{
    private static readonly PaymentLogic _logic = new();
    private static readonly string[] IdealBanks = { "ABN AMRO", "ING", "Rabobank", "SNS" };

    public static string? Start(decimal total)
    {
        List<string> methods = _logic.GetPaymentMethods();
        
        List<string> options = new List<string>(methods);
        options.Add("Cancel");

        while (true)
        {
            Console.Clear();
            string headerTitle = $"--- SELECT PAYMENT METHOD ---\nAmount due: {total:0.00} EUR";
            MenuNavigation menu = new MenuNavigation(options, headerTitle);
            int selection = menu.Start();

            if (selection == options.Count - 1)
            {
                return null;
            }

            string backendSelectionString = (selection + 1).ToString();
            string? method = _logic.SelectMethod(backendSelectionString);
            
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
            return method;
        }
    }

    private static bool HandleCard()
    {
        Console.Clear();
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
        Console.Clear();
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
        Console.Clear();
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

        MenuHelpers.Confirm($"Change due: {change:0.00} EUR");
        return true;
    }

    private static bool HandleIdeal()
    {
        Console.Clear();
        List<string> bankOptions = new List<string>(IdealBanks);
        bankOptions.Add("Cancel payment validation");

        MenuNavigation bankMenu = new MenuNavigation(bankOptions, "--- SELECT YOUR BANK ---");
        int bankSelection = bankMenu.Start();

        if (bankSelection == bankOptions.Count - 1)
        {
            MenuHelpers.Error("iDEAL authentication aborted.");
            return false;
        }

        MenuHelpers.Confirm($"Redirecting to {IdealBanks[bankSelection]}...");
        return true;
    }
}