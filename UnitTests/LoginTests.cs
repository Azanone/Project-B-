namespace UnitTests;



[TestClass]
public sealed class LoginTests
{
    private readonly AccountsLogic _logic = new();
    private readonly AccountsAccess _access = new();
    private string _username = string.Empty;
    private string _email = string.Empty;
    private const string Password = "testpass123";
    DateTime birthDate = new DateTime(2000, 4, 4);

    [TestInitialize]
    public void TestInitialize()
    {
        _username = $"login_test_{Guid.NewGuid():N}";
        _email = $"{_username}@example.com";
        _logic.Register(_username, _email, Password, "0612345678", birthDate);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        AccountModel? account = _access.GetByIdentifier(_username);
        if (account != null)
        {
            _access.Delete(account);
        }
    }


    [DataTestMethod]
    [DataRow("username")]
    [DataRow("email")]
    public void LoginValidCredentials(string mode)
    {
        string identifier = mode == "email" ? _email : _username;

        AccountModel? result = _logic.CheckLogin(identifier, Password);

        Assert.IsNotNull(result);
        Assert.AreEqual("User", result.Role);
        Assert.AreEqual(_username, result.Username);
        Assert.AreEqual(_email, result.EmailAddress);
        Assert.AreEqual(Password, result.Password);
    }

    [DataTestMethod]
    [DataRow("wrong_identifier", Password)]
    [DataRow("", Password)]
    [DataRow(null, Password)]
    [DataRow("username", "wrong_password")]
    [DataRow("email", "wrong_password")]
    [DataRow("username", "")]
    [DataRow("username", null)]
    public void LoginInvalidCredentials(string? identifierInput, string? passwordInput)
    {
        string? identifier = identifierInput switch
        {
            "username" => _username,
            "email" => _email,
            _ => identifierInput
        };
        string? password = passwordInput == Password ? Password : passwordInput;

        AccountModel? result = _logic.CheckLogin(identifier!, password!);

        Assert.IsNull(result);
    }
}
