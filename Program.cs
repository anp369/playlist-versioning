﻿using System;
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
            
                Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    try
                    {
                        //load config file and init program components with it
                        if (o.Verbose) Console.WriteLine("loading Config file: {0}", o.ConfigPath);
                        ConfigFile cnf = ConfigFile.DeserializeFile(o.ConfigPath);
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
                    }
                    catch (InvalidPathException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (SpotifyException e)
                    {
                        Console.WriteLine("Fehler beim Zugriff auf die Spotify API: {0}", e.Message);
                        if (o.Verbose)
                        {
                            Console.WriteLine("Error Code: {0}", e.Data["ErrorCode"]);
                            Console.WriteLine("Error Description: {0}", e.Data["ErrorDesc"]);
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("IO Error: {0}", e.Message);
                        if (o.Verbose) Console.WriteLine(e.StackTrace);
                    }
                });
            }
            
        }
    }
