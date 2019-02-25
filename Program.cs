using System;
using System.Collections.Generic;
using CommandLine;
using LibGit2Sharp;
using Newtonsoft.Json;
using SpotifyAPI.Web.Enums;
using System.IO;
using System.Linq;
using SpotifyVersioning.types;

namespace SpotifyVersioning
{
    public class Program
    {
        private static bool _verbose;
        private static ConfigFile _configFile;
        private static Logger _logger;
        
        static void Main(string[] args)
        {
            try
            {
                var result = Parser.Default.ParseArguments<CronOptions, DiffOptions, InitOptions, InteractiveOptions>(args);

                result.WithParsed<Options>(o =>
                {
                    _verbose = o.Verbose;
                    _configFile = ConfigFile.DeserializeFile(o.ConfigPath);
                    if (_configFile.LogFilePath != "") _logger = new Logger(_configFile.LogFilePath);
                });
                
                    
                result.WithParsed<CronOptions>(o => Modules.Cron(o,_configFile))
                    .WithParsed<InitOptions>(o => Modules.Init(o,_configFile))
                    .WithParsed<DiffOptions>(o => Modules.Diff(o,_configFile))
                    .WithParsed<InteractiveOptions>(o => Modules.Interactive(o,_configFile));
            }


            catch (InvalidPathException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SpotifyException e)
            {
                Console.WriteLine("Fehler beim Zugriff auf die Spotify API: {0}", e.Message);
                if (_verbose)
                {
                    Console.WriteLine("Error Code: {0}", e.Data["ErrorCode"]);
                    Console.WriteLine("Error Description: {0}", e.Data["ErrorDesc"]);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("IO Error: {0}", e.Message);
                if (_verbose) Console.WriteLine(e.StackTrace);
            }
            
        }
    }
}