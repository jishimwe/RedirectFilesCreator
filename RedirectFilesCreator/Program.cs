// See https://aka.ms/new-console-template for more information

using RedirectFilesCreator;

class Program
{
    /* 
     * Main arguments -s "source code directory" -d "redirect files directory"
     */
    static void Main(string[] args)
    {
        if (args == null || args.Length == 0)
        {
            PrintUsage();
            Environment.Exit(-2);
        }

        Console.WriteLine(args.ToString());
        Array.ForEach(args, Console.WriteLine);
        string? sourceRepo = null;
        string? targetRepo = null;
        bool isValid = false;
        for (int i = 0; i < args.Length; i += 2)
        {
            string s = args[i];
            switch (s)
            {
                case "-s":
                    sourceRepo = args[i + 1];
                    break;

                case "-d":
                    targetRepo = args[i + 1];
                    break;

                case "-verify":
                case "-v":
                    isValid = true;
                    break;

                default:
                    PrintUsage();
					Environment.Exit(-2);
					break;
            }
        }
        ResultCreateRedirectRepo(GitExplorer.CreateRedirectRepo(sourceRepo, targetRepo));
		Console.BackgroundColor = ConsoleColor.Green;
		if (isValid && GitExplorer.VerifyRedirectIntegrity(targetRepo, sourceRepo))
            Console.WriteLine("Redirect repo is valid");
        else if (isValid)
            Console.WriteLine("Redirect repos is invalid");
		Console.ResetColor();
	}

    static void ResultCreateRedirectRepo(bool res)
    {
        string message = res ? "Redirect Repo Created" : "Error Creating Redirect Repo";
        Console.WriteLine(message);
    }

    static void PrintUsage()
    {
        Console.WriteLine("Invalid arguments\n" +
            "Launch the program with the following arguments:\n" +
            "\t -s <path>\t: path to the source code root folder\n" +
            "\t -d <path>\t: path to the folder that will contain the redirect files");
    }
}