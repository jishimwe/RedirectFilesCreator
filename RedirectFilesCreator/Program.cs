// See https://aka.ms/new-console-template for more information

using RedirectFilesCreator;

class MainClass
{
    static void Main(string[] args)
    {
        if (args == null)
        {
            Console.WriteLine("No argument. Launching tests instances");
            DefaultExec();
        }
        if (args.Length == 0) DefaultExec();

        Console.WriteLine(args.ToString());
        Array.ForEach(args, Console.WriteLine);
        string sourceRepo = null;
        string targetRepo = null;
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

                default:
                    sourceRepo = GitExplorer.PathToRepo;
                    targetRepo = GitExplorer.PathToRedirectRepo;
                    break;
            }
        }
        ResultCreateRedirectRepo(GitExplorer.CreateRedirectRepo(sourceRepo, targetRepo)) ;
    }

    static void DefaultExec()
    { 
        Console.WriteLine("Creating Redirect Repo ... \n [Source {0}] \t [Redirect {1}]", GitExplorer.PathToRepo, GitExplorer.PathToRedirectRepo);
         ResultCreateRedirectRepo(GitExplorer.CreateRedirectRepo(GitExplorer.PathToRepo, GitExplorer.PathToRedirectRepo));
    }

    static void ResultCreateRedirectRepo(bool res)
    {
        string message = res ? "Redirect Repo Created" : "Error Creating Redirect Repo";
        Console.WriteLine(message);
    }
}