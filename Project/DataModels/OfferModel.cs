public class OfferModel
{
    public int OfferID {get ; set;}
    public string Description {get; set;}
    public float DiscountPercentage {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public Decimal RegularPrice {get; set;}
    public Decimal DiscountPrice {get {return RegularPrice - Math.Round(RegularPrice * (decimal)DiscountPercentage / 100, 2);}}
    public Decimal TotalRevenue {get; set;}
    public OfferModel() { }
    public OfferModel(int offerid, string description, float discountpercentage, DateTime startdate, DateTime enddate)
    {
        OfferID = offerid;
        Description = description;
        DiscountPercentage = discountpercentage;
        StartDate = startdate;
        EndDate = enddate;
    }

}