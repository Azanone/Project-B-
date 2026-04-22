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

    public string phoneNumber { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; } = null;
    public int Age {
        get
        {
            if (BirthDate == null)
            {
                return 0;
            }
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Value.Year;
            if (BirthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }


    public AccountModel()
    {
    }

    public AccountModel(Int64 id, string email, string password, string fullname)
        : this(id, string.Empty, email, password, fullname, string.Empty, "0", string.Empty)
    {
    }

    public AccountModel(Int64 id, string username, string email, string password, string fullname, string postcode, string houseNumber, string phoneNumber, DateTime? birthDate = null)
    {
        Id = id;
        Username = username;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Postcode = postcode;
        HouseNumber = houseNumber;
        this.phoneNumber = phoneNumber;
        BirthDate = birthDate;
        Role = "User";
    }
    public AccountModel(string username, string email, string password, string fullname, string postcode, string houseNumber, string phoneNumber, DateTime? birthDate = null)
    {
        Username = username;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Postcode = postcode;
        HouseNumber = houseNumber;
        this.phoneNumber = phoneNumber;
        BirthDate = birthDate;
        Role = "User";
    }


}



