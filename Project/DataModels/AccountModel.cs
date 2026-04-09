public class AccountModel
{

    public Int64 Id { get; set; }

    public String Role { get; set; }
    public string EmailAddress { get; set; }

    public string Password { get; set; }

    public string Username { get; set; }
    public string Phonenumber { get; set; }

    public AccountModel(Int64 id, string email, string password, string username, string phonenumber)
    {
        Id = id;
        EmailAddress = email;
        Password = password;
        FullName = fullname;
        Role = "User";
    }


}



