using System;
using System.Diagnostics;
using System.IO;

namespace SpotifyVersioning
{
    /// <summary>
    /// class that handles all the git stuff
    /// 1) creating repositories
    /// 2) adding and committing playlistupdates and new playlists
    /// </summary>
    public class GitHandler
    {
       
        /// <summary>
        /// assumes, that a repo is in the current folder and
        /// tracks all changes to playlist text files
        /// </summary>
        public static void CheckForChanges()
        {
       
            Process.Start("git", "add --all");
            var time = DateTime.Now;
            
            Process status = new Process();
            status.StartInfo.FileName = "git";
            status.StartInfo.Arguments= "status -s -uno";
            status.StartInfo.UseShellExecute = false;
            status.StartInfo.CreateNoWindow = true;
            status.StartInfo.RedirectStandardOutput = true;
            status.Start();
            var results = status.StandardOutput.ReadToEnd();
            status.WaitForExit();
            status.Dispose();
            
            
            Console.WriteLine("{0} : git status: {1}",DateTime.Now.ToString("yyyMMdd"), results);
            var commit = Process.Start("git","commit -m \""+time.ToString("yyyy-MM-dd")+"\"");
            commit.WaitForExit();
            
            
        }
        
        
        /// <summary>
        /// initialises an empty git repository in the current folder and creates an .gitignore
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void FirstStart()
        {
            if (Directory.Exists("./.git"))
            {
                throw new Exception("Git Repository bereits vorhanden");
            }
            
            string[] gitignore = {"*","!*.txt","conf.json"};

            var init = Process.Start("git", "init");
            init.WaitForExit();
            File.WriteAllLines("./.gitignore",gitignore);
        }
    }
}