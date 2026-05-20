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

    public static string PromptPassword(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(message);
        Console.ResetColor();

        string password = string.Empty;
        ConsoleKeyInfo key;

        while ((key = Console.ReadKey(true)).Key != ConsoleKey.Enter)
        {
            if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }

                continue;
            }

            if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }

        Console.WriteLine();
        return password;
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
    public static string PromptUntilValid(string prompt, Func<string, bool> validate)
    {
        while (true)
        {
            string? input = Prompt(prompt);
            string value = input ?? string.Empty;

            if (validate(value))
            {
                return value;
            }

            Warn("Invalid input");
        }
    }

    public static void PromptReturnToMenu(string message, Action onReturn)
    {
        string? input = Prompt(message);
        if (input == "1")
        {
            onReturn();
            return;
        }

        Warn("Invalid input");
        PromptReturnToMenu(message, onReturn);
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
}