using System;
using System.Collections.Generic;
using Project.Models;

static class AdminMenu
{
    static public void Start()
    {
        AccountsLogic accountsLogic = new AccountsLogic();

        Console.Clear();
        AccountModel account = AccountsLogic.CurrentAccount;

        List<string> options = new List<string>
        {
            "Information Overview",
            "Manage products",
            "See products sold in date range",
            "Manage users",
            "Log-out"
        };

        MenuNavigation menu = new MenuNavigation(options, "--- Admin Dashboard: --- \n Welcome back " + account.FullName);
        int selection = menu.Start();

        switch (selection)
        {
            case 0:
                AdminInformationOverview.Start();
                break;
            case 1:
                AdminProductManagement.Start();
                break;
            case 2:
                ShowFinancialOverview.Start();
                break;
            case 3:
                AdminUserManagement.Start();
                break;
            case 4:
                accountsLogic.Logout();
                Menu.Start();
                break;
        }
    }
}