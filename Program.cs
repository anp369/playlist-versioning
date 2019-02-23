using System;
using System.Collections.Generic;
using CommandLine;
using LibGit2Sharp;
using Newtonsoft.Json;
using SpotifyAPI.Web.Enums;
using System.IO;
using System.Linq;

namespace SpotifyVersioning
{
    class Program
    {
        /// <summary>
        /// command line options for the program
        /// </summary>
        public class Options
        {
            [Option('c',"config",HelpText="specifies the path to the config file", Required=true)]
            public string ConfigPath { get; set; }
            
            [Option('v',"verbose",HelpText = "Prints extended Debug Messages and Execptions")]
            public bool Verbose { get; set; }
            
            [Option("cron",HelpText = "Use for checking on playlist-updates automatically", Required = false)]
            public bool Cron { get; set; }
        
            [Option("init",HelpText = "initialises the git repo")]
            public bool Init { get; set; }
        }
        
        static void Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    //load config file and init program components with it
                    ConfigFile  cnf = ConfigFile.DeserializeFile(o.ConfigPath);
                    if (cnf.GitRepoPath.Last() != '/')
                        throw new InvalidPathException("The path in your config file needs to end with '/'");
               
                    if (o.Init)
                    {
                        GitHandler.FirstStart(cnf.GitRepoPath);
                    }

                    else // all following parts require API access
                    {
                    
                        List<string> pll = cnf.Playlists.ToList();
                        PlaylistHandler pl = new PlaylistHandler(cnf, Scope.PlaylistReadPrivate);
                    
                        if (o.Cron)
                        {
                            if (!Repository.IsValid(cnf.GitRepoPath))
                            {
                                throw new Exception("Kein Git-Repo gefunden, bite mit --init erstellen");
                            }
                            pl.RunCron();
                        }
                    }
                });
            }
            catch (InvalidPathException e)
            {    
                Console.WriteLine(e.Message);
            }
        }
    }
}