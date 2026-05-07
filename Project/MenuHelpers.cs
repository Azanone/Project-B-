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
    string? input;
    bool error;
    do
    {
        input = Prompt(prompt);
        error = validate(input);
        if (!error) Warn($"Invalid input: {error}");
        System.Threading.Thread.Sleep(1000);
        Console.Clear();

    }
    while (!error);

    return input;
}
}