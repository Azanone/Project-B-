namespace UnitTests;

[TestClass]
public sealed class RegistrationTests
{
    private readonly AccountsLogic _logic = new();
    private readonly AccountsAccess _access = new();

    [TestMethod]
    public void RegisterCreatesAccountInDatabase()
    {
        string username = $"reg_test_{Guid.NewGuid():N}";
        string email = $"{username}@example.com";
        const string password = "register123";
        const string phoneNumber = "0612345678";

        AccountModel? storedByUsername = null;

        try
        {
            _logic.Register(username, email, password, phoneNumber);

            storedByUsername = _access.GetByIdentifier(username);
            AccountModel? storedByEmail = _access.GetByIdentifier(email);

            Assert.IsNotNull(storedByUsername);
            Assert.IsNotNull(storedByEmail);
            Assert.AreEqual(username, storedByUsername.Username);
            Assert.AreEqual(email, storedByUsername.EmailAddress);
            Assert.AreEqual(password, storedByUsername.Password);
            Assert.AreEqual(phoneNumber, storedByUsername.phoneNumber);
            Assert.AreEqual(username, storedByUsername.FullName);
            Assert.AreEqual(username, storedByEmail.Username);
        }
        finally
        {
            if (storedByUsername != null)
            {
                _access.Delete(storedByUsername);
            }
            else
            {
                AccountModel? fallback = _access.GetByIdentifier(username);
                if (fallback != null)
                {
                    _access.Delete(fallback);
                }
            }
        }
    }

    [TestMethod]
    public void RegisterThenLoginWithUsernameAndEmailSucceeds()
    {
        string username = $"reg_login_{Guid.NewGuid():N}";
        string email = $"{username}@example.com";
        const string password = "register123";

        AccountModel? stored = null;

        try
        {
            _logic.Register(username, email, password, "0612345678");

            AccountModel? byUsername = _logic.CheckLogin(username, password);
            AccountModel? byEmail = _logic.CheckLogin(email, password);

            Assert.IsNotNull(byUsername);
            Assert.IsNotNull(byEmail);
            Assert.AreEqual(username, byUsername.Username);
            Assert.AreEqual(email, byEmail.EmailAddress);

            stored = _access.GetByIdentifier(username);
        }
        finally
        {
            if (stored == null)
            {
                stored = _access.GetByIdentifier(username);
            }

            if (stored != null)
            {
                _access.Delete(stored);
            }
        }
    }
}
