using System.Text;

namespace RedirectFilesCreator
{
	public static class GitExplorer
    {
        // Constants for filenaming
        private const string RdrExt = ".redir";

        // Open a redirect file and check if the file redirected to exist
        public static bool RedirectToFile(string redirPath, string realPathRoot)
        {
	        using StreamReader file = new(redirPath);
	        string? ln = file.ReadLine();
	        if (ln == null || !File.Exists(realPathRoot + "\\" + ln)) 
		        return false;
	        return true;
        }

        public static bool VerifyRedirectIntegrity(string pathToRedir, string repoRoot)
        {
            DirectoryInfo di = new(pathToRedir);

            try
            {
				FileInfo[] _ = di.GetFiles("*.*");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
				DirectoryInfo[] dirs = di.GetDirectories();
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
                DirectoryInfo origInfo = new(origPath);
                if (!origInfo.Exists) return false;

                // Verifying if the destination exists and deleting it in the affirmative and creating a new one
                DirectoryInfo destInfo = new(destPath);
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

        public static bool DirectoryWalk(DirectoryInfo origDir, DirectoryInfo destDir, string? pathSoFar = null)
        {
            FileInfo[]? files = null;
            try
            {
                files = origDir.GetFiles("*.*");
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
	            foreach (FileInfo fi in files)
	            {
		            string redirFilename = fi.Name + RdrExt;
		            string redirFilepath = Path.Combine(destDir.FullName, redirFilename);
		            string gitPath = (pathSoFar != null) ? pathSoFar + "\\" + fi.Name : fi.Name;
		            Console.WriteLine(gitPath);
		            using FileStream fs = File.Create(redirFilepath);
		            byte[] buff = new UTF8Encoding(true).GetBytes(gitPath);
		            fs.Write(buff);
		            fs.Close();
	            }
			}

            try
            {
				DirectoryInfo[] dirs = origDir.GetDirectories();
                foreach (DirectoryInfo di in dirs)
                {
                    string redirPath = destDir.FullName + "\\" + di.Name;
                    Directory.CreateDirectory(redirPath);
                    string thisPath = (pathSoFar != null) ? pathSoFar + "\\" + di.Name : di.Name;
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
