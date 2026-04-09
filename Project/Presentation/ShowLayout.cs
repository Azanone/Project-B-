public static class ShowLayout
{
    static public void Start()
    {
        Console.WriteLine(@"╔══════════════╦══════════════════╦═══════════════════╗
║              ║                  ║                   ║
║   BAKERY     ║     DAIRY        ║     FROZEN        ║
║              ║                  ║                   ║
╠══════════════╩══════════════════╣                   ║
║                                 ╚═══════════════════╣
║  ┌───────┐ ┌──────────┐ ┌─────┐ ┌─────────┐        ║
║  │       │ │ Canned & │ │Beve-│ │ Snacks  │        ║
║  │ Deli  │ │ Dry Food │ │rage │ │  And    │        ║
║  │       │ │          │ │     │ │  Goods  │        ║
║  └───────┘ └──────────┘ └─────┘ └─────────┘        ║
╠════════════════════╦════════════════════════════════╣
║                    ║                                ║
║   FRESH PRODUCE    ║    CASHOUT /                   ║
║                    ║    CUSTOMER SERVICE            ║
║                    ║                                ║
╚════════════════════╝        ↑           ╚═══════════╝
                        ENTRANCE / EXIT");
        Console.WriteLine("Enter 1 to return to Admin Menu");
        string input = Console.ReadLine();
        if (input == "1")
        {
            AdminMenu.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }

}