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
        private static readonly Signature _signature = new Signature("versioner","1234@567.de",DateTimeOffset.Now);
        
        /// <summary>
        /// assumes, that a repo is in the current folder and
        /// tracks all changes to playlist text files
        /// </summary>
        public static void CheckForChanges(string path)
        {
            try
            {
                Repository repo = new Repository(path);
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
        /// <param name="path">path in which the git repo should be initalized</param>
        /// <exception cref="Exception"></exception>
        public static void FirstStart(string path)
        {
            Console.WriteLine("Creating Repo: {0}",path);
            if (Repository.IsValid(path))
            {
                throw new GitException("Git Repository bereits vorhanden");
            }
            
            string[] gitignore = {"*","!*.txt","conf.json"};

            Repository.Init(path);
            File.WriteAllLines(path+".gitignore",gitignore);
        }
    }
}