using Microsoft.Data.Sqlite;
using Dapper;
using Project.Models;
 
public class ShoppingCartAccess
{
    private readonly SqliteConnection _connection = DBconnection._c;
 
    public SqliteConnection Connection => _connection;
   
    public List<ShoppingCartItem> GetAll(int userId)
    {
        {
            string sql = @"
        SELECT
            CI.CartItemId,
            CI.CartId,
            CI.ProductId,
            CI.Quantity,
            P.ProductID,
            P.Name,
            P.Price,
            P.Brand,
            P.Ingredients
        FROM CART C
        JOIN CART_ITEM CI ON CI.CartId = C.CartId
        JOIN PRODUCT P ON P.ProductId = CI.ProductId
        WHERE C.UserId = @UserId";
 
            return _connection.Query<ShoppingCartItem, ProductModel, ShoppingCartItem>(
                sql,
                (cartItem, product) =>
                {
                    cartItem.Product = product;
                    return cartItem;
                },
                new { UserId = userId },
                splitOn: "ProductID"
            ).ToList();
        }
    }
   
    public void AddItemsToCart(int userId, int productId, int quantity)
    {
        string selectSql = "SELECT CartId FROM CART WHERE UserId = @UserId";
        var cartId = _connection.QueryFirstOrDefault<int?>(selectSql, new { UserId = userId });
 
        if (!cartId.HasValue)
        {
            string insertCartSql = @"
            INSERT INTO CART (UserId, CreatedAt)
            VALUES (@UserId, @CreatedAt);
            SELECT last_insert_rowid();";
            cartId = _connection.QuerySingle<int>(insertCartSql, new { UserId = userId, CreatedAt = DateTime.Now });
        }
 
        string insertItemSql = @"
        INSERT INTO CART_ITEM (CartId, ProductId, Quantity)
        VALUES (@CartId, @ProductId, @Quantity);";
 
        _connection.Execute(insertItemSql, new { CartId = cartId.Value, ProductId = productId, Quantity = quantity });
    }
 
    public void UpdateCart(ShoppingListModel cart)
    {
        string sql = $"UPDATE CART_ITEM SET Quantity=@Quantity WHERE CartItemID=@CartItemId";
 
        _connection.Execute(sql, cart);
    }
 
    public void RemoveItemsFromCart(int cartItemId)
    {
        string sql =
            "DELETE FROM CART_ITEM WHERE CartItemId=@CartItemId";
 
        _connection.Execute(sql, new { CartItemId = cartItemId });
    }
    public int CreatePurchase(int userId,decimal totalAmount,decimal vat,SqliteTransaction transaction)//Returns PurchaseID
    {
        string sql = @"
        INSERT INTO PURCHASE
        (UserID, PurchaseDate, TotalAmount, Vat)
        VALUES
        (@UserID, @PurchaseDate, @TotalAmount, @Vat);
 
        SELECT last_insert_rowid();";
        System.Console.WriteLine("Create Purcahse");
        return _connection.QuerySingle<int>(sql,new
        {
                UserID = userId,
                PurchaseDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                TotalAmount = totalAmount,
                Vat = vat
            },transaction);
    }
 
    public void CreatePurchaseItem(int purchaseId,int productId,int quantity,decimal priceAtPurchase,SqliteTransaction transaction)
    {
        string sql = @"
        INSERT INTO PURCHASE_ITEM
        (PurchaseID, ProductID, Quantity, PriceAtPurchase)
        VALUES
        (@PurchaseID, @ProductID, @Quantity, @PriceAtPurchase)";
        System.Console.WriteLine("Create PurcahseItem");
        _connection.Execute(sql,new
        {
                PurchaseID = purchaseId,
                ProductID = productId,
                Quantity = quantity,
                PriceAtPurchase = priceAtPurchase
            },transaction);
    }
   
    public void CreateReceipt(int purchaseId,SqliteTransaction transaction)
    {
        string sql = @"
        INSERT INTO RECEIPT
        (PurchaseID, CreatedAt)
        VALUES
        (@PurchaseID, @CreatedAt)";
        System.Console.WriteLine("Create Receipt");
        MenuHelpers.Pause();
 
        _connection.Execute(sql,new{PurchaseID = purchaseId,CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},transaction);
    }
    public void ClearCart(int userId,SqliteTransaction transaction)
    {
        string sql = @"
        DELETE FROM CART_ITEM
        WHERE CartId IN
        (
            SELECT CartId
            FROM CART
            WHERE UserId = @UserId
        )";
        _connection.Execute(sql, new { UserId = userId },transaction);
    }
}