static class AdminMenu
{
    static public void Start()
    {
        AccountsLogic accountsLogic = new();

        while (true)
        {
            Console.Clear();
            AccountModel? account = AccountsLogic.CurrentAccount;
            if (account != null)
            {
                MenuHelpers.Announce("Admin Dashboard: Welcome back " + account.FullName);
            }

            MenuHelpers.Confirm("Enter 1 to see all past transactions");
            MenuHelpers.Confirm("Enter 2 to see a list of all the products");
            MenuHelpers.Confirm("Enter 3 to see the store layout");
            MenuHelpers.Confirm("Enter 4 to see all the offers");
            MenuHelpers.Confirm("Enter 5 to see the stock of the products");
            MenuHelpers.Confirm("Enter 6 to see the receipts");
            MenuHelpers.Confirm("Enter 7 to see products sold in date range");
            MenuHelpers.Confirm("Enter 8 to log-out");

            string? input = MenuHelpers.Prompt("");
            if (input == "1")
            {
                PastTransactions.Start();
                return;
            }
            else if (input == "2")
            {
                ShowProducts.Start();
                return;
            }
            else if (input == "3")
            {
                ShowLayout.Start();
                return;
            }
            else if (input == "4")
            {
                ShowOffers.Start();
                return;
            }
            else if (input == "5")
            {
                ShowStock.Start();
                return;
            }
            else if (input == "6")
            {
                ShowReceipt.Start();
                return;
            }
            else if (input == "7")
            {
                ShowFinancialOverview.Start();
                return;
            }
            else if (input == "8")
            {
                accountsLogic.Logout();
                Menu.Start();
                return;
            }
            else
            {
                MenuHelpers.Warn("Invalid input");
            }
        }
    }

}