using System.Text.RegularExpressions;

public static class MenuHelpers
{
    public static void Warn(string input)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    public static void Error(string input)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    public static void Announce(string input)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    public static void Confirm(string input)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(input);
        Console.ResetColor();
    }
    public static string? Prompt(string input)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(input);
        Console.ResetColor();
        return Console.ReadLine();
     }
    public static string PromptSecret(string input)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(input);
        Console.ResetColor();

        string value = string.Empty;
        ConsoleKeyInfo keyInfo;
        while (true)
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (value.Length > 0)
                {
                    value = value.Substring(0, value.Length - 1);
                    if (Console.CursorLeft > 0)
                    {
                        Console.Write("\b \b");
                    }
                }
                continue;
            }

            if (!char.IsControl(keyInfo.KeyChar))
            {
                value += keyInfo.KeyChar;
                Console.Write("*");
            }
        }

        return value;
    }
    public static int PromptInt(string strin)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(strin);
        string? input = Console.ReadLine();
        Console.ResetColor();
        if (int.TryParse(input, out int id))
        {
            return id;
        }
        return 0;
     }

     public static void Pause()
    {
        Prompt("Press Enter to continue");
    }
    public static void PromptReturnToMenu(string prompt, Action onReturn)
    {
        string? input = Prompt(prompt);
        if (input == "1")
        {
            onReturn();
            return;
        }

        Warn("Invalid input");
    }

    public static void WriteColor(string str, ConsoleColor color)
         {
        var matches = Regex.Matches(str, @"\[(.*?)\]");

        int lastIndex = 0;

        foreach (Match match in matches)
        {
            Console.ResetColor();
            Console.Write(str.Substring(lastIndex, match.Index - lastIndex));

            Console.ForegroundColor = color;
            Console.Write(match.Groups[1].Value);

            lastIndex = match.Index + match.Length;
        }

        Console.ResetColor();
        Console.WriteLine(str.Substring(lastIndex));
    }

    public static string PromptUntilValid(string prompt, Func<string, bool> validate)
{
    string? input;
    bool error;
    do
    {
        input = Prompt(prompt);
        error = input != null && validate(input);
        if (!error) Warn($"Invalid input: {error}");
        System.Threading.Thread.Sleep(1000);
        Console.Clear();

    }
    while (!error);

    return input ?? string.Empty;
}
}