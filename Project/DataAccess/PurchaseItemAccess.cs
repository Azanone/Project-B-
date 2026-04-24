using Microsoft.Data.Sqlite;

using Dapper;
public class PurchaseItemAccess
{
    private SqliteConnection _connection = new SqliteConnection($"Data Source=DataSources/project.db");
    public List<PurchaseItemModel> GetDateRange(DateTime startDate, DateTime endDate)
    {
        string sql = @"SELECT PURCHASE_ITEM.PurchaseItemID, PURCHASE_ITEM.PurchaseID, PURCHASE_ITEM.ProductID, PURCHASE_ITEM.Quantity, PURCHASE_ITEM.PriceAtPurchase, PRODUCT.Name AS ProductName, PURCHASE.PurchaseDate, PRODUCT.Price AS ProductPrice, (PURCHASE_ITEM.Quantity * PURCHASE_ITEM.PriceAtPurchase) AS TotalRevenue 
FROM PURCHASE_ITEM
JOIN PRODUCT ON PURCHASE_ITEM.ProductID = PRODUCT.ProductID
JOIN PURCHASE ON PURCHASE_ITEM.PurchaseID = PURCHASE.PurchaseID
WHERE PURCHASE.PurchaseDate >= @StartDate AND PURCHASE.PurchaseDate <= @EndDate";
        return _connection.Query<PurchaseItemModel>(sql, new { StartDate = startDate, EndDate = endDate }).ToList();
    }

}