public class ReceiptModel
{
    public int ReceiptID {get; set;}
    public int PurchaseID {get; set;}
    public DateTime CreatedAt {get; set;}
    public string ProductName {get; set;}
    public Decimal ProductPrice {get; set;}
    public Decimal VAT {get; set;}
    public Decimal TotalPrice {get; set;}
    

}