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

    public string? GetLocationLabel(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return null;
        }
        return CategoryLocations.TryGetValue(category, out string? label) ? label : null;
    }

}
