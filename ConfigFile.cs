using System.Dynamic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace SpotifyVersioning
{
    /// <summary>
    /// Class that holds Settings, deserialzied from the .json config file
    /// </summary>
    public class ConfigFile
    
    {
        public string Username {get; set;}
        public string ClientId {get; set;}
        public string ClientSecret {get; set;}
        public string GitRepoPath { get; set; }
        public string[] Playlists { get; set; }
        

        /// <summary>
        /// Deserializes the config file conf.json in the program's directory
        /// </summary>
        /// <returns>ConfigFile Object with all Settings</returns>
        public static ConfigFile DeserializeFile(string path)
        {
            return JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path));
        }
        
    }
}