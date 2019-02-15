using System;
using System.Collections.Generic;
using CommandLine;
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
            [Option("cron",HelpText = "Use for checking on playlist-updates automatically", Required = false)]
            public bool Cron { get; set; }
        
            [Option("init",HelpText = "initialises the git repo")]
            public bool Init { get; set; }
        }
        
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
            {
                
                if (o.Init)
                {
                    GitHandler.FirstStart();
                }

                else // all following parts require API access
                {
                    //load config file and init program components with it
                    ConfigFile  cnf = ConfigFile.DeserializeFile();
                    
                    List<string> pll = cnf.Playlists.ToList();
                    PlaylistHandler pl = new PlaylistHandler(cnf, Scope.PlaylistReadPrivate);
                    
                    if (o.Cron)
                    {
                        if (!Directory.Exists("./.git"))
                        {
                            throw new Exception("Kein Git-Repo gefunden, bite mit --init erstellen");
                        }
                        pl.RunCron();
                    }
                }
            });
        }
    }
}