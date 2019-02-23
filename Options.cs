using System;
using CommandLine;
using CommandLine.Text;

namespace SpotifyVersioning
{
    [Verb("cron", HelpText = "Queries all playlists of a given config file, usable wit cronjobs")]
    class CronOptions : Options
    {
                
    }

    [Verb("diff", HelpText = "Select two versions of a playlist and compare them")]
    class DiffOptions : Options
    {
        [Option("versions",HelpText = "shows all dates on which changes are saved")]
        public bool Versions { get; set; }
        
        [Value(0,HelpText = "Name of the Playlist to be compared")]
        public string FileName { get; set; }
        
        [Value(1,HelpText = "Date which should be compared. Enter in YYYY/MM/DD")]
        public DateTime Time { get; set; }
        
        
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