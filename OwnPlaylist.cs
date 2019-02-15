using System.Collections.Generic;
using SpotifyAPI.Web.Models;

namespace SpotifyVersioning
{
    /// <summary>
    /// provides an easier interface for creating the textfiles of a playlist
    /// Holds name and all tracks(simple string) of a playlist
    /// </summary>
    public class OwnPlaylist
    {
       
        public string Name;
        public List<string> Songs;
        public Paging<PlaylistTrack> CurrentPage;
        public int Count;
        
        public OwnPlaylist()
        {
            Songs = new List<string>();
        }
        
        public OwnPlaylist(string name, Paging<PlaylistTrack> page) : this()
        {
            Name = name;
            CurrentPage = page;
            Count = page.Total;
        }
    }
}