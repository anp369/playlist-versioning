using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using SpotifyAPI.Web.Enums;

namespace SpotifyVersioning
{
    internal static class Modules
    {
        internal static void Cron(CronOptions opts, ConfigFile cfg)
        {
            

            if (cfg.GitRepoPath.Last() != '/')
                throw new InvalidPathException("The path in your config file needs to end with '/'");

            // all following parts require API access

            List<string> pll = cfg.Playlists.ToList();
            PlaylistHandler pl = new PlaylistHandler(cfg, Scope.PlaylistReadPrivate);

            if (!Repository.IsValid(cfg.GitRepoPath))
            {
                throw new GitException("Kein Git-Repo gefunden, bite mit --init erstellen");
            }

            pl.RunCron();
        }

        internal static void Init(InitOptions opts, ConfigFile cfg)
        {

        }

        internal static void Diff(DiffOptions opts, ConfigFile cfg)
        {
            Repository repo = new Repository(cfg.GitRepoPath);
            
            // prints all Versions of a given playlist name
            if (opts.Versions)
            {
                List<DateTimeOffset> dates = new List<DateTimeOffset>();
                foreach (Commit commit in repo.Commits)
                {
                    foreach (var parent in commit.Parents)
                    {
                        //https://stackoverflow.com/questions/30945725/libgit2sharp-find-what-files-were-updated-added-deleted-after-pull/30967786#30967786
                        foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
                        {
                            if (change.Path.Contains(opts.FileName))dates.Add(commit.Author.When);
                            if (opts.Verbose) Console.WriteLine("{0}    : {1} : {2}", commit.Author.When.ToString(), change.Status,
                                change.Path);
                        }
                    }
                }
                Console.WriteLine("Versions of playlist {0}", opts.FileName);
                Console.WriteLine(new String('=',opts.FileName.Length+21));
                foreach (var time in dates)
                {
                    Console.WriteLine("{0}",time.DateTime.ToString("yyyy-MM-dd HH:mm:ss")); //convert to current time
                }
                if (dates.Count == 0) Console.WriteLine("No Entries found. Did you spell the playlist correctly?");
            }
            
            // print diff of a current day to today
            else
            {
                
            }
            
        }
    }
}