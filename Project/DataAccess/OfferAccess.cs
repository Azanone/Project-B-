using Microsoft.Data.Sqlite;

using Dapper;

public class OfferAccess
{
    private SqliteConnection _connection = DBconnection._c;
    
    public List<OfferModel> GetAll()
    {
        string sql = $"SELECT OFFER.*, PURCHASE_ITEM.PriceAtPurchase AS RegularPrice, SUM(PURCHASE_ITEM.Quantity * PURCHASE_ITEM.PriceAtPurchase) AS TotalRevenue FROM OFFER JOIN PRODUCT_OFFER ON OFFER.OfferID = PRODUCT_OFFER.OfferID JOIN PURCHASE_ITEM ON PRODUCT_OFFER.ProductID = PURCHASE_ITEM.ProductID GROUP BY OFFER.OfferID";
        return _connection.Query<OfferModel>(sql).ToList();
    }

    public Dictionary<int, int> GetProductToOfferMapping()
    {
        string sql = "SELECT ProductID, OfferID FROM PRODUCT_OFFER";
        return _connection.Query<(int ProductID, int OfferID)>(sql)
                        .ToDictionary(x => x.ProductID, x => x.OfferID);
    }
}