namespace Project.Models;

public class ReviewModel
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public string Review {  get; set; }
    public double Rating { get; set; }

    public ReviewModel() { }
    
    public ReviewModel(int productId, string review, double rating)
    {
        ProductId = productId;
        Review = review;
        Rating = rating;
    }
}