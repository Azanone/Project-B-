public class ProductDetailsLogic
{
    private static readonly Dictionary<string, string> CategoryLocations = new()
    {
        { "Bakery",            "Bakery aisle (top-left)" },
        { "Dairy",             "Dairy aisle (top-center)" },
        { "Frozen",            "Frozen aisle (top-right)" },
        { "Deli",              "Deli aisle (middle-left)" },
        { "Canned & Dry Food", "Canned & Dry Food aisle (middle)" },
        { "Dry Goods",         "Canned & Dry Food aisle (middle)" },
        { "Beverages",         "Beverages aisle (middle)" },
        { "Snacks & Goods",    "Snacks & Goods aisle (middle-right)" },
        { "Goods",             "Snacks & Goods aisle (middle-right)" },
        { "Fresh Produce",     "Fresh Produce section (bottom-left)" }
    };

    private static readonly Dictionary<string, string> CategoryHighlightTokens = new(StringComparer.OrdinalIgnoreCase)
    {
        { "Bakery",            "BAKERY" },
        { "Dairy",             "DAIRY" },
        { "Frozen",            "FROZEN" },
        { "Deli",              " Deli " },
        { "Canned & Dry Food", "Dry Food" },
        { "Dry Goods",         "Dry Food" },
        { "Beverages",         "rage" },
        { "Snacks & Goods",    "Snacks" },
        { "Goods",             "Snacks" },
        { "Fresh Produce",     "FRESH PRODUCE" }
    };

    public string? GetHighlightToken(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return null;
        }
        return CategoryHighlightTokens.TryGetValue(category, out string? token) ? token : null;
    }

    public string? GetLocationLabel(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return null;
        }
        return CategoryLocations.TryGetValue(category, out string? label) ? label : null;
    }

}
