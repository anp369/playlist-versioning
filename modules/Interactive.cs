using System;
using System.Collections.Generic;
using System.Globalization;
using CommandLine;
using SpotifyAPI.Web.Models;
using SpotifyVersioning.types;

namespace SpotifyVersioning
{
    public class Interactive
    {
        private ConfigFile _config;
        private bool _quit = false;
        private string _selectedPl = "";
        

        public Interactive(ConfigFile cfg)
        {
            _config = cfg;
        }
        
        
        public void StartSession()
        {
            Console.WriteLine("type 'quit' or 'exit' to leave the interactive mode.");
            
            while (!_quit)
            {
                PrintLineStart();
                string[] input = Console.ReadLine().Split(' ');

                switch (input[0])
                {
                    case "quit":
                        _quit = true;
                        break;
                    case "exit":
                        _quit = true;
                        break;
                    case "help":
                        ShowHelp();
                        break;
                    
                    case "versions":
                        if (input.Length > 1) PrintVersions(
                            GitHandler.ListVersions(_config.GitRepoPath, input[1], false)
                        );
                        else Console.WriteLine("missing parameter");
                        break;
                    
                    case "diff":
                        if (input.Length > 2) PrintDiff(input[1], input[2]);
                        break;
                }
                

            }
        }

        private void PrintLineStart()
        {
            if (_selectedPl == "") Console.Write("None > ");
            else Console.Write(_selectedPl + ">  ");
        }

        private void ShowHelp()
        {
            Console.WriteLine("Available commands: ");
            Console.WriteLine("versions [playlist name] - lists all available versions of a playlist");
            Console.WriteLine("diff [playlist name] [entry number/date] - prints diff of current version and the selected version or the given date");
        }

        private void PrintVersions(List<string> processString)
        {
            int index = 0;
            processString.RemoveAt(1);
            processString.RemoveAt(0);
            foreach (string line in processString)
            {
                Console.WriteLine("{0}) {1}",index, line);
                index++;
            }
        }

        private void PrintDiff(string name, string date)
        {
            DateTime target;
            if (DateTime.TryParse(date, out target))
            {
                GitHandler.PrintDiffs(GitHandler.GetDiff(_config.GitRepoPath,name,false,target));
            }
            else
            {
                var versions = GitHandler.ListVersions(_config.GitRepoPath, name, false);
                GitHandler.PrintDiffs(GitHandler.GetDiff(_config.GitRepoPath, name, false, DateTime.Parse(versions[Convert.ToInt32(date)+2])));
                Console.ForegroundColor = ConsoleColor.Black;
            }
        }
    }
}