public class AccountModel
{

    public Int64 Id { get; set; }

    public String Role { get; set; } = "User";

    public string Username { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Postcode { get; set; } = string.Empty;

    public int HouseNumber { get; set; }

    public string phoneNumber { get; set; } = string.Empty;


    public AccountModel()
    {
    }

    public AccountModel(Int64 id, string email, string password, string fullname)
        : this(id, string.Empty, email, password, fullname, string.Empty, 0, string.Empty)
    {
    }

    public AccountModel(Int64 id, string username, string email, string password, string fullname, string postcode, int houseNumber, string phoneNumber)
    {
        Id = id;
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



