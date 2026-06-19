namespace UnitTests;

[TestClass]
public sealed class ProductDetailsTests
{
    private readonly ProductDetailsLogic _logic = new();

    // ---------- Happy paths ----------

    // H1: Known category (exact case) returns its store-area label.
    [DataTestMethod]
    [DataRow("Bakery", "Bakery aisle (top-left)")]
    [DataRow("Dairy", "Dairy aisle (top-center)")]
    [DataRow("Frozen", "Frozen aisle (top-right)")]
    public void H1_GetLocationLabel_KnownCategoryReturnsLabel(string category, string expectedLabel)
    {
        Assert.AreEqual(expectedLabel, _logic.GetLocationLabel(category));
    }

    // H2: Category lookup is case-insensitive (DB values and user input may differ in case).
    [DataTestMethod]
    [DataRow("bakery", "Bakery aisle (top-left)")]
    [DataRow("DAIRY", "Dairy aisle (top-center)")]
    [DataRow("fresh produce", "Fresh Produce section (bottom-left)")]
    public void H2_GetLocationLabel_IsCaseInsensitive(string category, string expectedLabel)
    {
        Assert.AreEqual(expectedLabel, _logic.GetLocationLabel(category));
    }

    // H3: Category aliases ("Goods", "Dry Goods") map to the same aisle as their canonical category.
    [DataTestMethod]
    [DataRow("Snacks & Goods", "Goods")]
    [DataRow("Canned & Dry Food", "Dry Goods")]
    public void H3_GetLocationLabel_AliasesShareLabel(string canonicalCategory, string aliasCategory)
    {
        Assert.AreEqual(_logic.GetLocationLabel(canonicalCategory), _logic.GetLocationLabel(aliasCategory));
    }

    // ---------- Sad paths ----------

    // S1: Unknown category returns null.
    [DataTestMethod]
    [DataRow("Nonexistent")]
    [DataRow("Pharmacy")]
    public void S1_GetLocationLabel_UnknownReturnsNull(string category)
    {
        Assert.IsNull(_logic.GetLocationLabel(category));
    }

    // S2: Null, empty or whitespace category returns null.
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void S2_GetLocationLabel_EmptyReturnsNull(string? category)
    {
        Assert.IsNull(_logic.GetLocationLabel(category!));
    }
}
