using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;

namespace SpotifyVersioning
{
    /// <summary>
    /// class that handles all the api calls and
    /// merges the paged api calls into a file
    /// </summary>
    public class PlaylistHandler
    {
        
        private Scope _Scope;
        private SpotifyWebAPI _Spotify;
        
        
        public string Username { get; private set; }
        public List<string> PlaylistIDs { get; private set; }
        public string RepoPath { get; private set; }
        
        
        public PlaylistHandler(ConfigFile cnf, Scope scope)
        {
            Username = cnf.Username;
            _Scope = scope;
            RepoPath = cnf.GitRepoPath;

            PlaylistIDs = cnf.Playlists.ToList();

            try
            {
                
                ClientCredentialsAuth auth = new ClientCredentialsAuth();
                auth.Scope = scope;
                auth.ClientId = cnf.ClientId;
                auth.ClientSecret = cnf.ClientSecret;

                Token token = auth.DoAuth();
                
                // creating the spotify api with authentication
                _Spotify = new SpotifyWebAPI(){TokenType = token.TokenType, AccessToken = token.AccessToken};
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// method that checks all playlists and stages changes to the git repository
        /// </summary>
        public void RunCron()
        {
            List<OwnPlaylist> playlists = new List<OwnPlaylist>();

            // Download the information for all playlists
            foreach (var playlist in PlaylistIDs)
            {
                string user = playlist.Split(':')[2];
                string id = playlist.Split(':')[4];

                var pl = _Spotify.GetPlaylist(user, id);
               
                var page = _Spotify.GetPlaylistTracks(user, id);
                playlists.Add(new OwnPlaylist(pl.Name, page));
               
            }

            // create a plain text of each playlist
            foreach (var pl in playlists)
            {
                while (pl.Songs.Count < pl.Count) // use this to know when to check for more pages
                {
                    pl.CurrentPage.Items.ForEach(track =>
                    {
                        string[] arr = {track.Track.Uri, track.Track.Name, track.Track.Artists[0].Name};
                        pl.Songs.Add(string.Join(":", arr));
                    });
                    if (pl.Songs.Count < pl.Count) pl.CurrentPage = _Spotify.GetNextPage(pl.CurrentPage);
                }
                
                pl.Songs.Sort((x,y) => x.Split(':')[3].CompareTo(y.Split(':')[3])); // Sort by name of the song
                PlayListIO.WritePlaylistToFile(pl.Songs,RepoPath,pl.Name);
            }
            GitHandler.CheckForChanges(RepoPath);
        }

        
    }
}