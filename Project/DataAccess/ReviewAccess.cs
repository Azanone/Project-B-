using Microsoft.Data.Sqlite;
using Dapper;
using Project.Models;

namespace Project.DataAccess;

public class ReviewAccess
{
    private SqliteConnection _connection = DBconnection._c;

    private const string SelectWithUser = @"
        SELECT r.ReviewID, r.ProductID, r.Review, r.Rating, r.UserID,
               u.Username AS Username
        FROM REVIEW r
        LEFT JOIN USER u ON u.UserID = r.UserID";

    public List<ReviewModel> GetAllReviews()
    {
        return _connection.Query<ReviewModel>(SelectWithUser).ToList();
    }

    public List<ReviewModel> GetReviewsByProduct(int productId)
    {
        string sql = SelectWithUser + " WHERE r.ProductID = @productId";
        return _connection.Query<ReviewModel>(sql, new { productId }).ToList();
    }

    public List<ReviewModel> GetReviewsByUser(int userId)
    {
        string sql = SelectWithUser + " WHERE r.UserID = @userId";
        return _connection.Query<ReviewModel>(sql, new { userId }).ToList();
    }

    public void AddReview(int productId, string review, double starRating, int userId)
    {
        string sql = "INSERT INTO REVIEW (ProductID, Review, Rating, UserID) VALUES (@productId, @review, @starRating, @userId);";
        _connection.Execute(sql, new { productId, review, starRating, userId });
    }
}
