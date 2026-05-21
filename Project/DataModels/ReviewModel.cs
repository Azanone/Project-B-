namespace Project.Models;

public class ReviewModel
{
    public int Id { get; set; }
    public string Review {  get; set; }
    public int ProductId { get; set; }
    public int StarRating { get; set; }

    public ReviewModel(ProductModel productModel, int id, string review, int starRating)
    {
        Id = id;
        Review = review;
        ProductId = productModel.ProductID;
        StarRating = starRating;
    }
}