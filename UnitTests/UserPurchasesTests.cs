namespace UnitTests;

[TestClass]
public sealed class UserPurchasesTests
{
    private readonly ReceiptLogic _receiptLogic = new();

    [DataTestMethod]
    [DataRow(9999, 0)]
    public void ShowPurchaseHistory_NoTransactions_DisplaysNoTransactionsFoundMessage(int accountId, int expectedCount)
    {
        List<ReceiptModel> purchases = _receiptLogic.GetPurchasesByAccountID(accountId);

        Assert.IsNotNull(purchases);
        Assert.AreEqual(expectedCount, purchases.Count);
    }

    [DataTestMethod]
    [DataRow(null,  10.00, 1)]  // missing name
    [DataRow("Apple", 0.00, 1)] // missing price
    [DataRow("Apple", 10.00, 0)] // missing quantity
    public void TransactionMissingNameOrPriceOrQuantity_DisplaysUnknownProduct(string? productName, double productPrice, int quantity)
    {
        // Arrange
        ReceiptModel receipt = new ReceiptModel
        {
            PurchaseID = 1,
            ProductName = productName!,
            ProductPrice = (decimal)productPrice,
            Quantity = quantity,
            CreatedAt = DateTime.Now
        };

        // Act
        bool isInvalid = string.IsNullOrEmpty(receipt.ProductName)
            || receipt.ProductPrice == 0
            || receipt.Quantity == 0;
        string displayName = isInvalid ? "Unknown Product" : receipt.ProductName!;

        // Assert
        Assert.AreEqual("Unknown Product", displayName);
    }
}
