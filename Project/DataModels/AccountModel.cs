public class AccountModel
{

    public int UserId { get; set; }

    public String Role { get; set; } = "User";

    public string Username { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Postcode { get; set; } = string.Empty;

    public string HouseNumber { get; set; }

    public string phoneNumber { get; set; } = string.Empty;


    public AccountModel()
    {
    }

    public AccountModel(int UserId, string email, string password, string fullname)
        : this(UserId, string.Empty, email, password, fullname, string.Empty, "0", string.Empty)
    {
    }

    public AccountModel(int userId, string username, string email, string password, string fullname, string postcode, string houseNumber, string phoneNumber)
    {
        UserId = userId;
        Username = username;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Postcode = postcode;
        HouseNumber = houseNumber;
        this.phoneNumber = phoneNumber;
        Role = "User";
    }
    public AccountModel(string username, string email, string password, string fullname, string postcode, string houseNumber, string phoneNumber)
    {
        Username = username;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Postcode = postcode;
        HouseNumber = houseNumber;
        this.phoneNumber = phoneNumber;
        Role = "User";
    }


}



