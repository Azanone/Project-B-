using Microsoft.Data.Sqlite;
using Dapper;

public class ReceiptAccess
{
    private SqliteConnection _connection = DBconnection._c;
    public List<ReceiptModel> GetAll()
    {
        string sql = @"SELECT RECEIPT.ReceiptID,
        RECEIPT.PurchaseID,
        RECEIPT.CreatedAt,
        PRODUCT.Name AS ProductName,
        PURCHASE_ITEM.Quantity,
        PURCHASE_ITEM.PriceAtPurchase AS ProductPrice,
        PURCHASE.VAT,
        PURCHASE.TotalAmount AS TotalPrice
FROM RECEIPT
JOIN PURCHASE       ON RECEIPT.PurchaseID     = PURCHASE.PurchaseID
JOIN PURCHASE_ITEM  ON PURCHASE.PurchaseID    = PURCHASE_ITEM.PurchaseID
JOIN PRODUCT        ON PURCHASE_ITEM.ProductID = PRODUCT.ProductID";
        return _connection.Query<ReceiptModel>(sql).ToList();
    }

    public List<ReceiptModel> GetPurchasesByAccountID(int accountID)
    {
        string sql = @"SELECT RECEIPT.ReceiptID,
        RECEIPT.PurchaseID,
        RECEIPT.CreatedAt,
        PRODUCT.Name AS ProductName,
        PURCHASE_ITEM.Quantity,
        PURCHASE_ITEM.PriceAtPurchase AS ProductPrice,
        PURCHASE.VAT,
        PURCHASE.TotalAmount AS TotalPrice
FROM RECEIPT
JOIN PURCHASE       ON RECEIPT.PurchaseID     = PURCHASE.PurchaseID
JOIN PURCHASE_ITEM  ON PURCHASE.PurchaseID    = PURCHASE_ITEM.PurchaseID
JOIN PRODUCT        ON PURCHASE_ITEM.ProductID = PRODUCT.ProductID
WHERE PURCHASE.UserID = @AccountID";
        return _connection.Query<ReceiptModel>(sql, new { AccountID = accountID }).ToList();
    }

}