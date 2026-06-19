public class ReviewModel
{
    public int ReviewId { get; set; }
    public int ProductId { get; set; }
    public string Review {  get; set; }
    public double Rating { get; set; }
    public int UserId { get; set; }
    public string? Username { get; set; }

    public ReviewModel() { }

    public ReviewModel(int productId, string review, double rating, int userId = -1)
    {
        ProductId = productId;
        Review = review;
        Rating = rating;
        UserId = userId;
    }
}
