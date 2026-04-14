using Microsoft.Data.Sqlite;

using Dapper;

public class OfferAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    
    public List<OfferModel> GetAll()
    {
        string sql = $"SELECT OFFER.*, PURCHASE_ITEM.PriceAtPurchase AS RegularPrice, SUM(PURCHASE_ITEM.Quantity * PURCHASE_ITEM.PriceAtPurchase) AS TotalRevenue FROM OFFER JOIN PRODUCT_OFFER ON OFFER.OfferID = PRODUCT_OFFER.OfferID JOIN PURCHASE_ITEM ON PRODUCT_OFFER.ProductID = PURCHASE_ITEM.ProductID GROUP BY OFFER.OfferID";
        return _connection.Query<OfferModel>(sql).ToList();
    }

}