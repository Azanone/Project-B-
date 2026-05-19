public class AccountModel
{

    public Int64 Id { get; set; }

    public String Role { get; set; } = "User";

    public string Username { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Postcode { get; set; } = string.Empty;

    public string HouseNumber { get; set; }
    public string Birthdate { get; set; }
    public string phoneNumber { get; set; } = string.Empty;


    public AccountModel()
    {
    }

    public AccountModel(string email, string password, string fullname)
        : this(string.Empty, email, password, fullname, string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
    public AccountModel(string username, string email, string password, string fullname, string postcode, string houseNumber, string phoneNumber, string bdate)
    {
        Username = username;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Postcode = postcode;
        HouseNumber = houseNumber;
        this.phoneNumber = phoneNumber;
        Role = "User";
        Birthdate = bdate;
    }


}



