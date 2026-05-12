public static class ShowReceipt
{
    static public void Start()
    {
        ShowAll();
        MenuHelpers.PromptReturnToMenu("Enter 1 to return to Admin Menu", AdminMenu.Start);
    }

    public static void ShowAll()
    {
        ReceiptLogic logic = new();
        List<ReceiptModel> receipts = logic.GetPurchases();

        var grouped = from r in receipts group r by r.ReceiptID;

        MenuHelpers.Announce("--- ALL RECEIPTS ---\n");

        foreach (var group in grouped)
        {
            ReceiptModel first = group.First();

            MenuHelpers.Confirm(@"  ___________________________________________");
            MenuHelpers.Confirm(@" /                                           \");
            MenuHelpers.Confirm(@"|   *************************************** |");
            MenuHelpers.Confirm(@"\_  * BabylonMarkt                        * |");
            MenuHelpers.Confirm(@"  | *************************************** |");
            MenuHelpers.Confirm($"  |  Date: {first.CreatedAt:dd-MM-yyyy}                       |");
            MenuHelpers.Confirm($"  |  Receipt No: #{first.ReceiptID,-23}   |");
            MenuHelpers.Confirm(@"  | --------------------------------------- |");
            MenuHelpers.Confirm($"  |  {"ITEM",-27}{"PRICE",-11} |");
            MenuHelpers.Confirm(@"  | ---------------------------------------  |");

            foreach (ReceiptModel item in group)
            {
                string name = item.ProductName.Length > 25 ? item.ProductName.Substring(0, 25) : item.ProductName;
                string qtyLabel = item.Quantity > 1 ? $"x{item.Quantity} " : "";
                decimal lineTotal = item.ProductPrice * item.Quantity;
                MenuHelpers.Confirm($"  |  {name,-27}{qtyLabel + lineTotal + " EUR",-11}  |");
            }

            MenuHelpers.Confirm(@"  | ---------------------------------------  |");
            MenuHelpers.Confirm($"  |  {"SUBTOTAL:",-27}{first.TotalPrice + " EUR",-11}  |");
            MenuHelpers.Confirm($"  |  {"TAX (VAT):",-27}{first.VAT + " EUR",-11}  |");
            MenuHelpers.Confirm(@"  | ---------------------------------------  |");
            MenuHelpers.Confirm($"  |  {"TOTAL:",-27}{first.TotalPrice + first.VAT + " EUR",-11}  |");
            MenuHelpers.Confirm(@"  | ---------------------------------------  |");
            MenuHelpers.Confirm(@"  |         THANK YOU FOR VISITING!           |");
            MenuHelpers.Confirm(@"  | __________________________________________|___");
            MenuHelpers.Confirm(@"  | /                                            /");
            MenuHelpers.Confirm(@"  \_/___________________________________________/");
        }
    }
}
