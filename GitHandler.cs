using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using LibGit2Sharp;

namespace SpotifyVersioning
{
    /// <summary>
    /// class that handles all the git stuff
    /// 1) creating repositories
    /// 2) adding and committing playlistupdates and new playlists
    /// </summary>
    public class GitHandler
    {
        //this is kind of useless but git wants some kind of publishing information
        private static Signature _signature = new Signature("versioner","1234@567.de",DateTimeOffset.Now);
        
        /// <summary>
        /// assumes, that a repo is in the current folder and
        /// tracks all changes to playlist text files
        /// </summary>
        public static void CheckForChanges()
        {
            try
            {
                Repository repo = new Repository("./");
                Commands.Stage(repo,"*");
                var time = DateTime.Now;
                var status = repo.RetrieveStatus(new StatusOptions());
                Console.WriteLine("{1} : Geänderte Dateien: {0}", status.Staged.Count().ToString(),DateTime.Now.ToString("yyyMMdd"));
                repo.Commit(time.ToString("yyyy-MM-dd"),_signature,_signature);
            }
            catch (EmptyCommitException e)
            {
                Console.WriteLine("{0} :Keine Änderungen", DateTime.Now.ToString("yyyMMdd"));
            }
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

            Repository.Init("./");
            File.WriteAllLines("./.gitignore",gitignore);
        }
    }
}