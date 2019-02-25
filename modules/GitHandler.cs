using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using LibGit2Sharp;
using SpotifyVersioning.types;

namespace SpotifyVersioning
{
    /// <summary>
    /// class that handles all the git stuff
    /// 1) creating repositories
    /// 2) adding and committing playlistupdates and new playlists
    /// 3) diffing
    /// </summary>
    public static class GitHandler
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
            catch (EmptyCommitException)
            {
                Console.WriteLine("{0} :Keine Änderungen", DateTime.Now.ToString("yyyMMdd"));
            }
        }
        
        
        /// <summary>
        /// initialises an empty git repository in the current folder and creates an .gitignore
        /// </summary>
        /// <param name="path">path in which the git repo should be initalized</param>
        /// <exception cref="Exception"></exception>
        public static void Initialise(string path)
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

        /// <summary>
        /// checks for different versions of a playlist in git repository 
        /// </summary>
        /// <param name="repoPath">path of the git repo</param>
        /// <param name="playlistName">name of the playlist (case sensitive!)</param>
        /// <param name="verbose">wheter verbose messages should be printed during version check</param>
        public static void ListVersions(string repoPath,string playlistName, bool verbose)
        {
            List<DateTimeOffset> dates = new List<DateTimeOffset>();
            Repository repo = new Repository(repoPath);
            foreach (Commit commit in repo.Commits)
            {
                if (commit.Parents.Count() == 0)
                {
                    dates.Add(commit.Author.When);
                    if (verbose)
                        Console.WriteLine("{0}    : {1}", commit.Author.When.ToString(), "initial Commit");
                }
                foreach (var parent in commit.Parents)
                {
                    //https://stackoverflow.com/questions/30945725/libgit2sharp-find-what-files-were-updated-added-deleted-after-pull/30967786#30967786
                    foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
                    {
                        if (change.Path.Contains(playlistName)) dates.Add(commit.Author.When);
                        if (verbose)
                            Console.WriteLine("{0}    : {1} : {2}", commit.Author.When.ToString(), change.Status,
                                change.Path);
                    }
                }
            }

            Console.WriteLine("Versions of playlist {0}", playlistName);
            Console.WriteLine(new String('=', playlistName.Length + 21));
            foreach (var time in dates)
            {
                Console.WriteLine("{0}", time.DateTime.ToString("yyyy-MM-dd HH:mm:ss")); //convert to current time
            }

            if (dates.Count == 0) Console.WriteLine("No Entries found. Did you spell the playlist correctly?");
        }

        public static void PrintDayDiff(string repoPath, string playlistName, bool verbose, DateTime time)
        {
            Repository repo = new Repository(repoPath);
            var results = repo.Commits.QueryBy(playlistName + ".txt").ToList();
            Commit result = null;
            foreach (LogEntry log in results)
            {
                if (verbose) Console.WriteLine(log.Commit.Author.When.ToString("dd/MM/yy: hh:mm"));
                if (log.Commit.Author.When.Date.ToString("dd/MM/yy: hh:mm") == time.ToString("dd/MM/yy: hh:mm") || 
                    log.Commit.Author.When.Date.ToString("d") == time.ToString("d")
                ) //search only for queried day
                {
                    result = log.Commit;
                }
            }

            if (result == null) Console.WriteLine("No commit for this playlist found on that day");
            else
            {
                Patch diff = repo.Diff.Compare<Patch>(result.Tree, repo.Head.Tip.Tree);
                if (diff.Count() == 0) Console.WriteLine("No differences found!");
                string rawText = diff.Content;
                Console.WriteLine("Changes in playlist: {0}",playlistName);
                foreach (string line in rawText.Split('\n'))
                {
                    if (line != "" && line.Length > 1)
                    {
                        if (line[0] == '+' && line[1] != '+')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(line);
                        }

                        if (line[0] == '-' && line[1] != '-')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(line);
                        }
                    }
                        
                }
            }
        }
        
    }
}