using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedirectFilesCreator
{
    public static class GitExplorer
    {
        private const string pathToRepo = @"This PC\Documents\Repo";
        private const string pathToRedirectRepo = @"This PC\\Documents\\Redirect";
        private const string testRepo = "https://github.com/jishimwe/PlayMusic.git";

        public static string PathToRepo => pathToRepo;
        public static string PathToRedirectRepo => pathToRedirectRepo;
        public static string TestRepo => testRepo;

        public static bool ExploreRepo(string repo )
        {
            if (repo == null) return false;
            var co = new CloneOptions();

            // TODO: Ask for credentials to avoid hardcoded password
            co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = "jpishimwe", Password = "ISHjp1160" };
            Repository.Clone(TestRepo, PathToRepo, co);

            return true;
        }  
    }
}
