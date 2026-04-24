public static class AdminUserManagement
{
    private static readonly AdminLogic _adminLogic = new();

    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            ShowUsers();

            MenuHelpers.Announce("--- USER MANAGEMENT ---");
            MenuHelpers.Confirm("Enter 1 to refresh user list");
            MenuHelpers.Confirm("Enter 2 to change a user's role");
            MenuHelpers.Confirm("Enter 3 to remove a user");
            MenuHelpers.Confirm("Enter 4 to return to Admin Menu");

            string? input = MenuHelpers.Prompt("");
            if (input == "1")
            {
                continue;
            }

            if (input == "2")
            {
                UpdateUserRoleFlow();
                continue;
            }

            if (input == "3")
            {
                RemoveUserFlow();
                continue;
            }

            if (input == "4")
            {
                AdminMenu.Start();
                return;
            }

            MenuHelpers.Warn("Invalid input");
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
            MenuHelpers.Error(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");
    }

    private static void RemoveUserFlow()
    {
        string userIdInput = PromptRequiredText("User ID to remove:");
        string? confirmation = MenuHelpers.Prompt("Type REMOVE to confirm:");
        if (!string.Equals(confirmation, "REMOVE", StringComparison.Ordinal))
        {
            MenuHelpers.Warn("Removal cancelled.");
            MenuHelpers.Prompt("Press Enter to continue");
            return;
        }

        var result = _adminLogic.RemoveUser(userIdInput);
        if (result.Success)
        {
            MenuHelpers.Confirm(result.Message);
        }
        else
        {
            MenuHelpers.Error(result.Message);
        }

        MenuHelpers.Prompt("Press Enter to continue");
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

            MenuHelpers.Error("Input is required.");
        }
    }
}
