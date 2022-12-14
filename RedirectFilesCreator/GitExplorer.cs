using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RedirectFilesCreator
{

    // TODO: Git integration
    public static class GitExplorer
    {
        private const string pathToRepo = @"C:\Users\ishim\Documents\Repo";
        private const string pathToRedirectRepo = @"C:\Users\ishim\Documents\Redirect";
        private const string testRepo = "https://github.com/jishimwe/PlayMusic.git";

        // Constants for filenaming
        private const string RdrExt = ".redir";

        public static string PathToRepo => pathToRepo;
        public static string PathToRedirectRepo => pathToRedirectRepo; 
        public static string TestRepo => testRepo;


        // Might be useless
        public static bool ExploreRepo(string repo )
        {
            if (repo == null) return false;
            var co = new CloneOptions();

            // TODO: Ask for credentials to avoid hardcoded password
            co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = "jpishimwe", Password = "" };
            Repository.Clone(TestRepo, PathToRepo, co);

            return true;
        }

        // Open a redirect file and check if the file redirected to exist
        public static bool RedirectToFile(string redirPath, string realPathRoot = pathToRepo)
        {
            using(StreamReader file = new StreamReader(redirPath))
            {
                string ln = file.ReadLine();
                //string realPath = realPathRoot + "\\" + ln;
                if (ln == null || !File.Exists(realPathRoot + "\\" + ln)) 
                    return false;
                return true;
            }
        }

        public static bool VerifyRedirectIntegrity(string pathToRedir, string repoRoot)
        {
            DirectoryInfo di = new DirectoryInfo(pathToRedir);
            FileInfo[] files = null;
            DirectoryInfo[] dirs = null;

            try
            {
                files = di.GetFiles("*.*");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            foreach (FileInfo fi in files)
            {
                if (!RedirectToFile(fi.FullName, repoRoot)) return false;
                Console.Write("._ ");
            }

            try
            {
                dirs = di.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    string path = dir.FullName;
                    VerifyRedirectIntegrity(path, repoRoot);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }

        public static bool CreateRedirectRepo(string  origPath, string destPath)
        {
            try
            {
                // Verifying if the source code folder exists
                DirectoryInfo origInfo = new DirectoryInfo(origPath);
                if (!origInfo.Exists) return false;

                // Verifying if the destination exists and deleting it in the affirmative and creating a new one
                DirectoryInfo destInfo = new DirectoryInfo(destPath);
                if (destInfo.Exists)
                {
                    destInfo.Delete(true);
                }
                destInfo.Create();

                // Running DirectoryWalk and creating redir files

                return DirectoryWalk(origInfo, destInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public static bool DirectoryWalk(DirectoryInfo origDir, DirectoryInfo destDir, string pathSoFar = null)
        {
            FileInfo[] files = null;
            DirectoryInfo[] dirs = null;
            try
            {
                files = origDir.GetFiles("*.*");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            foreach (FileInfo fi in files) 
            {
                string redirFilename = fi.Name + RdrExt;
                string redirFilepath = Path.Combine(destDir.FullName, redirFilename);
                string gitPath = (pathSoFar != null) ? pathSoFar + "\\" + fi.Name : fi.Name;
                Console.WriteLine(gitPath);
                using (FileStream fs = File.Create(redirFilepath))
                {
                    //byte[] data = new UTF8Encoding(true).GetBytes(fi.FullName);
                    //fs.Write(data);
                    byte[] buff = new UTF8Encoding(true).GetBytes(gitPath);
                    fs.Write(buff);
                    fs.Close();
                }
            }

            try
            {
                dirs = origDir.GetDirectories();
                foreach (DirectoryInfo di in dirs)
                {
                    string redirPath = destDir.FullName + "\\" + di.Name;
                    Directory.CreateDirectory(redirPath);
                    string thisPath = (pathSoFar != null) ? pathSoFar + "\\" + di.Name : di.Name;
                    //Console.WriteLine(pathSoFar);
                    if (!DirectoryWalk(di, new DirectoryInfo(redirPath), thisPath)) return false;
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }
    }
}
