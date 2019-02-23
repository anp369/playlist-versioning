using System;

namespace SpotifyVersioning
{
    public class InvalidPathException: Exception
    {
        public InvalidPathException() : base () {}
        public InvalidPathException(string message) : base (message) {}
    }

    public class GitException : Exception
    {
        public GitException() : base() {}
        public GitException(string message) : base(message) {}
    }

    public class SpotifyException : Exception
    {
        public SpotifyException() : base() {}
        public SpotifyException(string message) : base(message) {}
    }
}