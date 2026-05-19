using Project.Logic;
using Project.Models;

public static class AdminUserManagement
{
    private static readonly AdminLogic _adminLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            ShowUsers();

            List<string> options = new List<string>
            {
                "Refresh user list",
                "Change a user's role",
                "Remove a user",
                "Return to Admin Menu"
            };

            MenuNavigation menu = new MenuNavigation(options, "User Management");
            int selection = menu.Start();

            switch (selection)
            {
                case 0:
                    continue;
                case 1:
                    UpdateUserRoleFlow();
                    break;
                case 2:
                    RemoveUserFlow();
                    break;
                case 3:
                    AdminMenu.Start();
                    return;
            }
        }
    }

    private static void ShowUsers()
    {
        List<AccountModel> users = _adminLogic.GetUsers();
        if (users.Count == 0)
        {
            MenuHelpers.Warn("No users found.");
            return;
        }

        MenuHelpers.Announce("--- CURRENT USERS ---");
        foreach (AccountModel user in users)
        {
            MenuHelpers.Confirm($"ID: {user.Id} | Username: {user.Username} | Name: {user.FullName} | Email: {user.EmailAddress} | Role: {user.Role}");
        }

        Console.WriteLine();
    }

    private static void UpdateUserRoleFlow()
    {
        string userIdInput = PromptRequiredText("User ID to update:");
        string newRoleInput = PromptRequiredText("New role (Admin/User):");

        var result = _adminLogic.UpdateUserRole(userIdInput, newRoleInput);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
        }
        else
        {
            MenuHelpers.Warn(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");;
    }

    private static void RemoveUserFlow()
    {
        string userIdInput = PromptRequiredText("User ID to remove:");
        string? confirmation = MenuHelpers.Prompt("Type REMOVE to confirm:");
        if (!string.Equals(confirmation, "REMOVE", StringComparison.Ordinal))
        {
            MenuHelpers.Warn("Removal cancelled.");
            MenuHelpers.Prompt("Press Enter to continue");;
            return;
        }

        var result = _adminLogic.RemoveUser(userIdInput);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
        }
        else
        {
            MenuHelpers.Warn(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");;
    }

    private static string PromptRequiredText(string prompt)
    {
        while (true)
        {
            string? input = MenuHelpers.Prompt(prompt);
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            MenuHelpers.Warn("Input is required.");
        }
    }
}