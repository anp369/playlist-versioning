using System;

namespace SpotifyVersioning
{
    public class InvalidPathException: Exception
    {
        public InvalidPathException() : base () {}
        public InvalidPathException(string message) : base (message) {}
        public InvalidPathException(string message, Exception innerException) : base (message, innerException) {}
    }

    public class GitException : Exception
    {
        public GitException() : base() {}
        public GitException(string message) : base(message) {}
        public GitException(string message, Exception innerException) : base(message, innerException) {}
    }
}