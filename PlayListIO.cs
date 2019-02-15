using System;
using System.Collections.Generic;
using SpotifyAPI.Web.Models;
using System.IO;

namespace SpotifyVersioning
{
    /// <summary>
    /// class for writing Playlists into a file
    /// </summary>
    public static class PlayListIO
    {
        public static void WritePlaylistToFile(List<string> pl, string name)
        {
            File.WriteAllLines("./"+name+".txt",pl);
        }

        public static List<string> ReadPlaylistFromFile(string name)
        {
            throw new NotImplementedException();
        }
    }
}