public class PurchaseModel
{
    public Int64 PurchaseID { get; set; }
    public Int64 UserID { get; set; }
    public DateTime PurchaseDate { get; set; }
    public Decimal TotalAmount {get; set;}
    public Decimal VAT {get; set;}
    public string UserName {get; set;}

    public PurchaseModel(Int64 purchaseid, Int64 userid, DateTime purchasedate, Decimal totalamount, Decimal vat)
    {
        PurchaseID = purchaseid;
        UserID = userid;
        PurchaseDate = purchasedate;
        TotalAmount = totalamount;
        VAT = vat;
    }
    public PurchaseModel() { }

}