public class MenuNavigation
{
    private List<List<string>> labels2D;
    private List<string> labels;
    private List<string> values;
    private List<bool>? requiresInput;
    private int currentquantities;
    private int selectedRow;
    private int selectedCol;
    private int selectedIndex;
    private string Title;
    private bool QuantityManipulation;
    private bool is2D;

    public MenuNavigation(List<List<string>> menuLabels2D, string title)
    {
        labels2D = menuLabels2D;
        is2D = true;
        requiresInput = new List<bool>();
        values = new List<string>();
        currentquantities = 0;
        Title = title;
        selectedRow = 0;
        selectedCol = 0;
        QuantityManipulation = false;
    }

    public MenuNavigation(List<string> menuLabels, List<bool> menuRequiresInput, string title)
    {
        labels = menuLabels;
        is2D = false;
        requiresInput = menuRequiresInput;
        values = new List<string>();
        
        int i = 0;
        while (i < labels.Count)
        {
            values.Add("");
            i = i + 1;
        }
        currentquantities = 0;
        selectedIndex = 0;
        Title = title;
        QuantityManipulation = false;
    }

    public MenuNavigation(List<string> menuLabels, string title)
    {
        labels = menuLabels;
        is2D = false;
        requiresInput = new List<bool>();
        values = new List<string>();
        
        int i = 0;
        while (i < labels.Count)
        {
            values.Add("");
            i = i + 1;
        }
        currentquantities = 0;
        Title = title;
        selectedIndex = 0;
        QuantityManipulation = false;
    }

    public MenuNavigation(List<string> menuLabels, string title, bool quantitymanipulation)
    {
        labels = menuLabels;
        is2D = false;
        requiresInput = new List<bool>();
        values = new List<string>();
        
        int i = 0;
        while (i < labels.Count)
        {
            values.Add("");
            i = i + 1;
        }
        currentquantities = 0;
        Title = title;
        selectedIndex = 0;
        QuantityManipulation = quantitymanipulation;
    }

    public int GetQuantity()
    {
        return currentquantities;
    }

    public int[] Start2D()
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

        return new int[] { selectedRow, selectedCol };
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
        if (is2D) return;

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
        if (is2D)
        {
            int r = 0;
            foreach (var row in labels2D)
            {
                int c = 0;
                foreach (string label in row)
                {
                    if (r == selectedRow && c == selectedCol)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write($"  [{label}]  ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"   {label}   ");
                    }
                    c = c + 1;
                }
                Console.WriteLine("\n");
                r = r + 1;
            }
        }
        else
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
    }

    private void DrawSelectedLine(string label, int index)
    {
        if (requiresInput != null && index < requiresInput.Count && requiresInput[index])
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
            Console.ForegroundColor = ConsoleColor.Blue;
            if (!(QuantityManipulation && index == selectedIndex && index != labels.Count - 1))
            {
                MenuHelpers.Announce($"   {label}");
            }
            else
            {
                if (label.Contains("discount! "))
                {
                    MenuHelpers.GOLD($"   {label} | Qty: {currentquantities}");
                }
                else
                {
                    Console.WriteLine($"   {label} | Qty: {currentquantities}");
                }
            }
            Console.ResetColor();
        }
    }

    private void DrawUnselectedLine(string label, int index)
    {
        if (requiresInput != null && index < requiresInput.Count && requiresInput[index])
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
            if (is2D) MoveSelectionUp2D();
            else MoveSelectionUp();
        }
        else if (key == ConsoleKey.DownArrow)
        {
            if (is2D) MoveSelectionDown2D();
            else MoveSelectionDown();
        }
        else if (is2D && key == ConsoleKey.LeftArrow)
        {
            MoveSelectionLeft2D();
        }
        else if (is2D && key == ConsoleKey.RightArrow)
        {
            MoveSelectionRight2D();
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
        else if (!is2D && QuantityManipulation == true && key == ConsoleKey.LeftArrow)
        {
            DecreaseQuantity();
        }
        else if (!is2D && QuantityManipulation == true && key == ConsoleKey.RightArrow)
        {
            IncreaseQuantity();
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

    private void MoveSelectionUp2D()
    {
        selectedRow = selectedRow - 1;
        if (selectedRow < 0)
        {
            selectedRow = labels2D.Count - 1;
        }
        if (selectedCol >= labels2D[selectedRow].Count)
        {
            selectedCol = labels2D[selectedRow].Count - 1;
        }
    }

    private void MoveSelectionDown2D()
    {
        selectedRow = selectedRow + 1;
        if (selectedRow >= labels2D.Count)
        {
            selectedRow = 0;
        }
        if (selectedCol >= labels2D[selectedRow].Count)
        {
            selectedCol = labels2D[selectedRow].Count - 1;
        }
    }

    private void MoveSelectionLeft2D()
    {
        selectedCol = selectedCol - 1;
        if (selectedCol < 0)
        {
            selectedCol = labels2D[selectedRow].Count - 1;
        }
    }

    private void MoveSelectionRight2D()
    {
        selectedCol = selectedCol + 1;
        if (selectedCol >= labels2D[selectedRow].Count)
        {
            selectedCol = 0;
        }
    }

    private void DecreaseQuantity()
    {
        if (selectedIndex == labels.Count - 1)
        {
            return;
        }
        
        if (currentquantities > 0)
        {
            currentquantities--;
        }
    }

    private void IncreaseQuantity()
    {
        if (selectedIndex == labels.Count - 1)
        {
            return;
        }
        currentquantities++;
    }

    private void HandleBackspace()
    {
        if (is2D) return;
        if (requiresInput != null && selectedIndex < requiresInput.Count && requiresInput[selectedIndex])
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
        if (is2D) return;
        if (requiresInput != null && selectedIndex < requiresInput.Count && requiresInput[selectedIndex])
        {
            values[selectedIndex] = values[selectedIndex] + inputChar;
        }
    }
}