using System;
using CommandLine;

namespace SpotifyVersioning.types
{
    [Verb("cron", HelpText = "Queries all playlists of a given config file, usable with cronjobs")]
    class CronOptions : Options
    {
                
    }

    [Verb("diff", HelpText = "compare the latest version against an earlier version")]
    class DiffOptions : Options
    {
        [Value(0,HelpText = "Name of the Playlist to be compared")]
        public string FileName { get; set; }
        
        [Value(1,HelpText = "Date which should be compared. Enter in YYYY/MM/DD")]
        public DateTime Time { get; set; }
        
        
    }

    [Verb("versions", HelpText = "get all recorded versions of one one playlist")]
    class VersionOptions : Options
    {
        [Value(0,HelpText = "Name of the playlist")]
        public string FileName { get; set; }
    }

    [Verb("interactive", HelpText = "starts an interactive CLI session to interact with saved playlists")]
    class InteractiveOptions : Options
    {
        
    }

    [Verb("init", HelpText = "initialises the git repo")]
    class InitOptions : Options
    {
                
    }
        
    /// <summary>
    /// command line options for the program
    /// </summary>
    class Options
    {
        [Option('c',"config",HelpText="specifies the path to the config file", Required=true)]
        public string ConfigPath { get; set; }
            
        [Option('v',"verbose",HelpText = "Prints extended Debug Messages and Execptions")]
        public bool Verbose { get; set; }
    }
}