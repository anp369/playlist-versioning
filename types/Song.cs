namespace SpotifyVersioning.types
{
    public class Song
    {
        #region Fields

        #endregion

        #region Properties

        public string Uri { get; private set; }
        public string Artist { get; private set; }
        public string SongName { get; private set; }

        #endregion

        #region Events

        #endregion

        #region Construtor

        public Song(string uri, string name, string artist)
        {
            Uri = uri;
            SongName = name;
            Artist = artist;
        }

        public Song(string savedString)
        {
            string[] tmp = savedString.Split(':');
            Uri = tmp[2];
            SongName = tmp[3];
            Artist = tmp[4];
        }

        #endregion


        #region Methods

        public override string ToString()
        {
            return string.Join(":",Uri, SongName, Artist);
        }
        #endregion
    }

}