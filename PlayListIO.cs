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
        public static void WritePlaylistToFile(List<string> pl,string path ,string name)
        {
            File.WriteAllLines(path+name+".txt",pl);
        }

        public static List<string> ReadPlaylistFromFile(string name)
        {
            throw new NotImplementedException();
        }
    }
}