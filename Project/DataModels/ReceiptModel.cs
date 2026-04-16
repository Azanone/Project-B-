public class ReceiptModel
{
    public int ReceiptID {get; set;}
    public int PurchaseID {get; set;}
    public DateTime CreatedAt {get; set;}
    public string ProductName {get; set;}
    public Decimal ProductPrice {get; set;}
    public Decimal VAT {get; set;}
    public Decimal TotalPrice {get; set;}
    
    public ReceiptModel(int receiptID, int purchaseID, DateTime createdAt, string productName, Decimal productPrice, Decimal vat, Decimal totalPrice)
    {
        ReceiptID = receiptID;
        PurchaseID = purchaseID;
        CreatedAt = createdAt;
        ProductName = productName;
        ProductPrice = productPrice;
        VAT = vat;
        TotalPrice = totalPrice;
    }

    public ReceiptModel() { }
}