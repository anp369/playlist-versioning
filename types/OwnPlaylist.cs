using System.Collections.Generic;
using SpotifyAPI.Web.Models;

namespace SpotifyVersioning.types
{
    /// <summary>
    /// provides an easier interface for creating the textfiles of a playlist
    /// Holds name and all tracks(simple string) of a playlist
    /// </summary>
    public class OwnPlaylist
    {
       
        public string Name;
        public List<Song> Songs;
        public Paging<PlaylistTrack> CurrentPage;
        public int Count;
        
        public OwnPlaylist()
        {
            Songs = new List<Song>();
        }
        
        public OwnPlaylist(string name, Paging<PlaylistTrack> page) : this()
        {
            Name = name;
            CurrentPage = page;
            Count = page.Total;
        }

        public IEnumerable<string> ConvertToString()
        {
            foreach (Song song in Songs)
            {
                yield return song.ToString();
            }
        }
        
        public void AddSong(Song song){Songs.Add(song);}
    }
}