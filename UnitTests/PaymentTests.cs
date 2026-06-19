namespace UnitTests;

[TestClass]
public sealed class PaymentTests
{
    private readonly PaymentLogic _logic = new();

    // ---------- Happy paths ----------

    // H1: Payment options are shown clearly and in a stable order.
    [DataTestMethod]
    [DataRow(new[] { "iDEAL", "Credit Card", "PayPal", "Cash" })]
    public void H1_GetPaymentMethods_ReturnsAllOptionsInOrder(string[] expectedMethods)
    {
        List<string> methods = _logic.GetPaymentMethods();

        CollectionAssert.AreEqual(expectedMethods, methods);
    }

    // H2: A valid 1-based menu choice maps to the matching method.
    [DataTestMethod]
    [DataRow("1", "iDEAL")]
    [DataRow("2", "Credit Card")]
    [DataRow("3", "PayPal")]
    [DataRow("4", "Cash")]
    public void H2_SelectMethod_ValidChoicesMapCorrectly(string input, string expectedMethod)
    {
        Assert.AreEqual(expectedMethod, _logic.SelectMethod(input));
    }

    // H3: Well-formed credit card details are accepted.
    [DataTestMethod]
    [DataRow("4111111111111111")]
    [DataRow("4912 7305 8562 1497")]
    public void H3_CreditCardNumber_ValidAccepted(string cardNumber)
    {
        Assert.IsTrue(_logic.ValidateCardNumber(cardNumber));
    }

    [DataTestMethod]
    [DataRow("123")]
    public void H3_CreditCardCvv_ValidAccepted(string cvv)
    {
        Assert.IsTrue(_logic.ValidateCvv(cvv));
    }

    [DataTestMethod]
    [DataRow("12/27")]
    public void H3_CreditCardExpiry_ValidAccepted(string expiry)
    {
        Assert.IsTrue(_logic.ValidateExpiry(expiry));
    }

    // H4: Cash that covers the total yields correct change, including exact payment.
    [DataTestMethod]
    [DataRow(44.50, 50.00, 5.50)]
    [DataRow(50.00, 50.00, 0.00)]
    public void H4_Cash_SufficientReturnsChange(double amountDue, double cashGiven, double expectedChange)
    {
        Assert.AreEqual((decimal)expectedChange, _logic.CalculateChange((decimal)amountDue, (decimal)cashGiven));
    }

    // ---------- Sad paths ----------

    // S1: Empty or non-numeric selection is rejected.
    [DataTestMethod]
    [DataRow("")]
    [DataRow("abc")]
    [DataRow(" ")]
    [DataRow("0")]
    [DataRow("5")]
    [DataRow("-1")]
    public void S1_SelectMethod_InvalidRejected(string input)
    {
        Assert.IsNull(_logic.SelectMethod(input));
    }

    // S3: Card numbers of the wrong length or with non-digits are rejected.
    [DataTestMethod]
    [DataRow("12345")]
    [DataRow("41111111111111111")]
    [DataRow("4111abcd11111111")]
    [DataRow("")]
    public void S3_CardNumber_InvalidRejected(string cardNumber)
    {
        Assert.IsFalse(_logic.ValidateCardNumber(cardNumber));
    }

    // S4: Expired or malformed expiry dates are rejected.
    [DataTestMethod]
    [DataRow("01/20")]
    [DataRow("13/30")]
    [DataRow("2030")]
    [DataRow("")]
    public void S4_Expiry_InvalidRejected(string expiry)
    {
        Assert.IsFalse(_logic.ValidateExpiry(expiry));
    }

    // S5: Insufficient cash and invalid PayPal emails are rejected.
    [DataTestMethod]
    [DataRow(50.00, 49.99)]
    public void S5_InsufficientCashRejected(double amountDue, double cashGiven)
    {
        Assert.IsNull(_logic.CalculateChange((decimal)amountDue, (decimal)cashGiven));
    }

    [DataTestMethod]
    [DataRow("not-an-email")]
    [DataRow("user@nodot")]
    [DataRow("")]
    public void S5_BadPayPalEmailRejected(string email)
    {
        Assert.IsFalse(_logic.ValidatePayPalEmail(email));
    }
}
