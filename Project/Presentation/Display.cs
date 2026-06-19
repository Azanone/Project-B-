public static class Display
{
    public static void List<T>(IEnumerable<T> items, Func<T, string> format, string title)
    {
        MenuHelpers.Announce(title);
        foreach (var item in items) MenuHelpers.Line(format(item));
    }
}
