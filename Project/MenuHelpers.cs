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
    public static string Prompt(string input)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(input);
        Console.ResetColor();
        string userInput = Console.ReadLine();
        return userInput;
    }
    public static string PromptUntilValid(string prompt, Func<string, bool> validate)
{
    string input;
    bool error;
    do
    {
        input = Prompt(prompt);
        error = validate(input);
        if (!error) Warn($"Invalid input: {error}");
    }
    while (!error);

    return input;
}
}