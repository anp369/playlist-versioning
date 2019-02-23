using System;
using System.Collections.Generic;
using SpotifyAPI.Web.Models;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Schema;

namespace SpotifyVersioning
{
    /// <summary>
    /// class for writing Playlists into a file
    /// </summary>
    public static class PlayListIO
    {
        public static void WritePlaylistToFile(OwnPlaylist pl,string path)
        {
            File.WriteAllLines(path+pl.Name+".txt", pl.ConvertToString());
        }

        public static OwnPlaylist ReadPlaylistFromFile(string path)
        {
            OwnPlaylist pl = new OwnPlaylist();
            string[] songs = File.ReadAllLines(path);
            foreach (string str in songs)
            {
                pl.AddSong(new Song(str));   
            }

            return pl;
        }

        public static List<OwnPlaylist> ReadAllPlaylistsFromDir(string dir)
        {
            List<OwnPlaylist> playlists = new List<OwnPlaylist>();
            foreach (string f in Directory.EnumerateFiles(dir).Where(s => Path.GetExtension(s) == ".txt"))
            {
                playlists.Add(ReadPlaylistFromFile(f));
            }

            return playlists;
        }
    }
}