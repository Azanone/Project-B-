public class PurchaseItemModel
{
    public int PurchaseItemID { get; set; }
    public int PurchaseID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public Decimal TotalRevenue { get; set; }

    public PurchaseItemModel(int purchaseItemID, int purchaseID, int productID, string productName, decimal productPrice, int quantity, decimal priceAtPurchase)
    {
        PurchaseItemID = purchaseItemID;
        PurchaseID = purchaseID;
        ProductID = productID;
        ProductName = productName;
        ProductPrice = productPrice;
    }

    public PurchaseItemModel() { }
}