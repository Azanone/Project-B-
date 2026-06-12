namespace UnitTests;

[TestClass]
public sealed class PaymentTests
{
    private readonly PaymentLogic _logic = new();

    // ---------- Happy paths ----------

    // H1: Payment options are shown clearly and in a stable order.
    [TestMethod]
    public void H1_GetPaymentMethods_ReturnsAllOptionsInOrder()
    {
        List<string> methods = _logic.GetPaymentMethods();

        Assert.AreEqual(4, methods.Count);
        CollectionAssert.AreEqual(
            new[] { "iDEAL", "Credit Card", "PayPal", "Cash" },
            methods);
    }

    // H2: A valid 1-based menu choice maps to the matching method.
    [TestMethod]
    public void H2_SelectMethod_ValidChoicesMapCorrectly()
    {
        Assert.AreEqual("iDEAL", _logic.SelectMethod("1"));
        Assert.AreEqual("Credit Card", _logic.SelectMethod("2"));
        Assert.AreEqual("PayPal", _logic.SelectMethod("3"));
        Assert.AreEqual("Cash", _logic.SelectMethod("4"));
    }

    // H3: Well-formed credit card details are accepted.
    [TestMethod]
    public void H3_CreditCard_ValidDetailsAccepted()
    {
        string futureExpiry = $"12/{(DateTime.Today.Year % 100) + 1:D2}";

        Assert.IsTrue(_logic.ValidateCardNumber("4111111111111111"));
        Assert.IsTrue(_logic.ValidateCardNumber("4912 7305 8562 1497")); // spaces accepted
        Assert.IsTrue(_logic.ValidateCvv("123"));
        Assert.IsTrue(_logic.ValidateExpiry(futureExpiry));
    }

    // H4: Cash that covers the total yields correct change, including exact payment.
    [TestMethod]
    public void H4_Cash_SufficientReturnsChange()
    {
        Assert.AreEqual(5.50m, _logic.CalculateChange(44.50m, 50.00m));
        Assert.AreEqual(0m, _logic.CalculateChange(50.00m, 50.00m)); // exact payment edge case
    }

    // ---------- Sad paths ----------

    // S1: Empty or non-numeric selection is rejected.
    [TestMethod]
    public void S1_SelectMethod_EmptyOrNonNumericRejected()
    {
        Assert.IsNull(_logic.SelectMethod(""));
        Assert.IsNull(_logic.SelectMethod("abc"));
        Assert.IsNull(_logic.SelectMethod(" "));
    }

    // S2: Out-of-range menu numbers are rejected (no crash on upper bound).
    [TestMethod]
    public void S2_SelectMethod_OutOfRangeRejected()
    {
        Assert.IsNull(_logic.SelectMethod("0"));
        Assert.IsNull(_logic.SelectMethod("5"));
        Assert.IsNull(_logic.SelectMethod("-1"));
    }

    // S3: Card numbers of the wrong length or with non-digits are rejected.
    [TestMethod]
    public void S3_CardNumber_InvalidRejected()
    {
        Assert.IsFalse(_logic.ValidateCardNumber("12345"));            // too short
        Assert.IsFalse(_logic.ValidateCardNumber("41111111111111111")); // 17 digits
        Assert.IsFalse(_logic.ValidateCardNumber("4111abcd11111111")); // non-digit
        Assert.IsFalse(_logic.ValidateCardNumber(""));                 // empty
    }

    // S4: Expired or malformed expiry dates are rejected.
    [TestMethod]
    public void S4_Expiry_InvalidRejected()
    {
        Assert.IsFalse(_logic.ValidateExpiry("01/20")); // past
        Assert.IsFalse(_logic.ValidateExpiry("13/30")); // month out of range
        Assert.IsFalse(_logic.ValidateExpiry("2030"));  // wrong format
        Assert.IsFalse(_logic.ValidateExpiry(""));      // empty
    }

    // S5: Insufficient cash and invalid PayPal emails are rejected.
    [TestMethod]
    public void S5_InsufficientCashAndBadEmailRejected()
    {
        Assert.IsNull(_logic.CalculateChange(50.00m, 49.99m)); // not enough
        Assert.IsFalse(_logic.ValidatePayPalEmail("not-an-email"));
        Assert.IsFalse(_logic.ValidatePayPalEmail("user@nodot"));
        Assert.IsFalse(_logic.ValidatePayPalEmail(""));
    }
}
