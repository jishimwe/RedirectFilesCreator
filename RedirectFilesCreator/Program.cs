// See https://aka.ms/new-console-template for more information

using RedirectFilesCreator;

Console.WriteLine("Cloning Repo from {0} to {1}", GitExplorer.TestRepo, GitExplorer.PathToRepo);
if (GitExplorer.ExploreRepo(GitExplorer.PathToRepo))
{
    Console.WriteLine("Repo Cloned");
} else
{
    Console.WriteLine("Error Cloning Repo");
}
