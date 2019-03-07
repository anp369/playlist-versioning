using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LibGit2Sharp;
using SpotifyAPI.Web.Enums;
using SpotifyVersioning.types;

namespace SpotifyVersioning
{
    /// <summary>
    /// contains all modules that get called by the main program
    /// one module per verb
    /// </summary>
    internal static class Modules
    {
        internal static void Cron(CronOptions opts, ConfigFile cfg)
        {

            if (cfg.GitRepoPath.Last() != '/')
                throw new InvalidPathException("The path in your config file needs to end with '/'");

            // all following parts require API access
            PlaylistHandler pl = new PlaylistHandler(cfg, Scope.PlaylistReadPrivate);

            if (!Repository.IsValid(cfg.GitRepoPath))
            {
                throw new GitException("Kein Git-Repo gefunden, bite mit --init erstellen");
            }

            pl.RunCron();
        }

        internal static void Init(InitOptions opts, ConfigFile cfg)
        {
            GitHandler.Initialise(cfg.GitRepoPath);
            Console.WriteLine("{0} initialized as repository!", cfg.GitRepoPath);
        }

        internal static void Diff(DiffOptions opts, ConfigFile cfg)
        {
            var diff = GitHandler.GetDiff(cfg.GitRepoPath,opts.FileName,opts.Verbose,opts.Time);
            GitHandler.PrintDiffs(diff);
        }

        internal static void Versions(VersionOptions opts, ConfigFile cfg)
        {
            foreach (var line in GitHandler.ListVersions(cfg.GitRepoPath,opts.FileName,opts.Verbose))
            {
                Console.WriteLine(line);
            }
        }

        internal static void Interactive(InteractiveOptions opts, ConfigFile cfg)
        {
            Interactive inter = new Interactive(cfg);
            inter.StartSession();
        }

    }
}