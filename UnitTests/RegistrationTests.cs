namespace UnitTests;

[TestClass]
public sealed class RegistrationTests
{
    private readonly AccountsLogic _logic = new();
    private readonly AccountsAccess _access = new();

    [DataTestMethod]
    [DataRow("reg_test", "register123", "0612345678", "01-01-1990")]
    public void RegisterCreatesAccountInDatabase(string usernamePrefix, string password, string phoneNumber, string birthdate)
    {
        string username = $"{usernamePrefix}_{Guid.NewGuid():N}";
        string email = $"{username}@example.com";

        AccountModel? storedByUsername = null;

        try
        {
            _logic.Register(username, email, password, phoneNumber, birthdate);

            storedByUsername = _access.GetByIdentifier(username);
            AccountModel? storedByEmail = _access.GetByIdentifier(email);

            Assert.IsNotNull(storedByUsername);
            Assert.IsNotNull(storedByEmail);
            Assert.AreEqual(username, storedByUsername.Username);
            Assert.AreEqual(email, storedByUsername.EmailAddress);
            Assert.AreNotEqual(password, storedByUsername.Password);
            Assert.IsTrue(Project.Logic.PasswordSecurityLogic.VerifyPassword(password, storedByUsername.Password));
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

    [DataTestMethod]
    [DataRow("reg_login", "register123", "0612345678", "01-01-1990")]
    public void RegisterThenLoginWithUsernameAndEmailSucceeds(string usernamePrefix, string password, string phoneNumber, string birthdate)
    {
        string username = $"{usernamePrefix}_{Guid.NewGuid():N}";
        string email = $"{username}@example.com";

        AccountModel? stored = null;

        try
        {
            _logic.Register(username, email, password, phoneNumber, birthdate);

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
