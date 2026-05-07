static class Menu
{    static public void Start()
    {
        Console.Clear();
        MenuHelpers.Announce(@".·:'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''':·.
: :     ____             __               __                   __  ___                    __     __  : :
: :    / __ )  ____ _   / /_    __  __   / /  ____    ____    /  |/  /  ____ _   _____   / /__  / /_ : :
: :   / __  | / __ `/  / __ \  / / / /  / /  / __ \  / __ \  / /|_/ /  / __ `/  / ___/  / //_/ / __/ : :
: :  / /_/ / / /_/ /  / /_/ / / /_/ /  / /  / /_/ / / / / / / /  / /  / /_/ /  / /     / ,<   / /_   : :
: : /_____/  \__,_/  /_.___/  \__, /  /_/   \____/ /_/ /_/ /_/  /_/   \__,_/  /_/     /_/|_|  \__/   : :
: :                          /____/                                                                  : :
'·:..................................................................................................:·'");
        MenuHelpers.Announce("Enter 1 to login");
        MenuHelpers.Announce("Enter 2 to register");
        MenuHelpers.Announce("Enter 3 to continue as guest");


        string input = Console.ReadLine() ?? string.Empty;
        if (input == "1")
        {
            Login.Start();
        }
        else if (input == "2")
        {
            UserRegister.Start();
        }
        else if (input == "3")
        {
            Dashboard.Start();
        }
        else
        {
            MenuHelpers.Warn("Invalid input");
            Start();
        }

    }
}