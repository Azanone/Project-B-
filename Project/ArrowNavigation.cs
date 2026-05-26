public class MenuNavigation
{
    private List<string> labels;
    private List<string> values;
    private List<bool> requiresInput;
    private int selectedIndex;
    private string Title;

    public MenuNavigation(List<string> menuLabels, List<bool> menuRequiresInput, string title)
    {
        labels = menuLabels;
        requiresInput = menuRequiresInput;
        values = new List<string>();
        
        int i = 0;
        while (i < labels.Count)
        {
            values.Add("");
            i = i + 1;
        }
        selectedIndex = 0;
        Title = title;
    }

    public MenuNavigation(List<string> menuLabels,  string title)
    {
        labels = menuLabels;
        requiresInput = null;
        values = new List<string>();
        
        int i = 0;
        while (i < labels.Count)
        {
            values.Add("");
            i = i + 1;
        }
        Title = title;
        selectedIndex = 0;
    }

    public int Start()
    {
        InitializeRequiresInput();

        bool running = true;
        while (running)
        {
            Console.Clear();
            MenuHelpers.Announce($"{Title}\n");
            DrawMenu();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            running = HandleInput(keyInfo);
        }
        return selectedIndex;
    }

    public List<string> GetValues()
    {
        return values;
    }

    private void InitializeRequiresInput()
    {
        if (requiresInput == null)
        {
            requiresInput = new List<bool>();
            int j = 0;
            while (j < labels.Count)
            {
                requiresInput.Add(false);
                j = j + 1;
            }
        }
    }

    private void DrawMenu()
    {
        int i = 0;
        foreach (string label in labels)
        {
            if (i == selectedIndex)
            {
                DrawSelectedLine(label, i);
            }
            else
            {
                DrawUnselectedLine(label, i);
            }
            i = i + 1;
        }
    }

    private void DrawSelectedLine(string label, int index)
{
    if (requiresInput[index])
    {
        string displayValue = values[index];
        if (label == "Password")
        {
            displayValue = new string('*', values[index].Length);
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"   {label}: {displayValue}");
        if (string.IsNullOrWhiteSpace(values[index]))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" [Required]");
            Console.ResetColor();
        }
        Console.WriteLine();
        Console.ResetColor();
    }
    else
    {
        MenuHelpers.Announce($"   {label}");
    }
}

private void DrawUnselectedLine(string label, int index)
{
    if (requiresInput[index])
    {
        string displayValue = values[index];
        if (label == "Password")
        {
            displayValue = new string('*', values[index].Length);
        }

        Console.WriteLine($"  {label}: {displayValue}");
    }
    else
    {
        Console.WriteLine($"  {label}");
    }
}

    private bool HandleInput(ConsoleKeyInfo keyInfo)
    {
        ConsoleKey key = keyInfo.Key;

        if (key == ConsoleKey.UpArrow)
        {
            MoveSelectionUp();
        }
        else if (key == ConsoleKey.DownArrow)
        {
            MoveSelectionDown();
        }
        else if (key == ConsoleKey.Enter)
        {
            return false;
        }
        else if (key == ConsoleKey.Backspace)
        {
            HandleBackspace();
        }
        else if (keyInfo.KeyChar >= 32 && keyInfo.KeyChar <= 126)
        {
            HandleCharacterInput(keyInfo.KeyChar);
        }

        return true;
    }

    private void MoveSelectionUp()
    {
        selectedIndex = selectedIndex - 1;
        if (selectedIndex < 0)
        {
            selectedIndex = labels.Count - 1;
        }
    }

    private void MoveSelectionDown()
    {
        selectedIndex = selectedIndex + 1;
        if (selectedIndex >= labels.Count)
        {
            selectedIndex = 0;
        }
    }

    private void HandleBackspace()
    {
        if (requiresInput[selectedIndex])
        {
            if (values[selectedIndex].Length > 0)
            {
                string currentVal = values[selectedIndex];
                values[selectedIndex] = currentVal.Substring(0, currentVal.Length - 1);
            }
        }
    }

    private void HandleCharacterInput(char inputChar)
    {
        if (requiresInput[selectedIndex])
        {
            values[selectedIndex] = values[selectedIndex] + inputChar;
        }
    }
}