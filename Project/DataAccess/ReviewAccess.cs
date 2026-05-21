using Microsoft.Data.Sqlite;
using Dapper;
using Project.Models;

namespace Project.DataAccess;

public class ReviewAccess
{
    private SqliteConnection _connection = DBconnection._c;
    
    public void AddReview(int productId, string review, int starRating)
    {
        string sql = "INSERT INTO REVIEW (ProductID, Review, Rating) VALUES (@productId, @review, @starRating);";
        _connection.Execute(sql, new { productId, review, starRating });
    }
}