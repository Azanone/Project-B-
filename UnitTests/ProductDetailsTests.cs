namespace UnitTests;

[TestClass]
public sealed class ProductDetailsTests
{
    private readonly ProductDetailsLogic _logic = new();

    // ---------- Happy paths ----------

    // H1: Known category (exact case) returns its store-area label.
    [TestMethod]
    public void H1_GetLocationLabel_KnownCategoryReturnsLabel()
    {
        Assert.AreEqual("Bakery aisle (top-left)", _logic.GetLocationLabel("Bakery"));
        Assert.AreEqual("Dairy aisle (top-center)", _logic.GetLocationLabel("Dairy"));
        Assert.AreEqual("Frozen aisle (top-right)", _logic.GetLocationLabel("Frozen"));
    }

    // H2: Category lookup is case-insensitive (DB values and user input may differ in case).
    [TestMethod]
    public void H2_GetLocationLabel_IsCaseInsensitive()
    {
        Assert.AreEqual("Bakery aisle (top-left)", _logic.GetLocationLabel("bakery"));
        Assert.AreEqual("Dairy aisle (top-center)", _logic.GetLocationLabel("DAIRY"));
        Assert.AreEqual("Fresh Produce section (bottom-left)", _logic.GetLocationLabel("fresh produce"));
    }

    // H3: Category aliases ("Goods", "Dry Goods") map to the same aisle as their canonical category.
    [TestMethod]
    public void H3_GetLocationLabel_AliasesShareLabel()
    {
        Assert.AreEqual(_logic.GetLocationLabel("Snacks & Goods"), _logic.GetLocationLabel("Goods"));
        Assert.AreEqual(_logic.GetLocationLabel("Canned & Dry Food"), _logic.GetLocationLabel("Dry Goods"));
    }

    // ---------- Sad paths ----------

    // S1: Unknown category returns null.
    [TestMethod]
    public void S1_GetLocationLabel_UnknownReturnsNull()
    {
        Assert.IsNull(_logic.GetLocationLabel("Nonexistent"));
        Assert.IsNull(_logic.GetLocationLabel("Pharmacy"));
    }

    // S2: Null, empty or whitespace category returns null.
    [TestMethod]
    public void S2_GetLocationLabel_EmptyReturnsNull()
    {
        Assert.IsNull(_logic.GetLocationLabel(""));
        Assert.IsNull(_logic.GetLocationLabel("   "));
    }
}
