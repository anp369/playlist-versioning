# playlist-versioning

## What is the playlist-versioning tool?  
Unfortunately Spotify lacks the option to see older versions of playlists,
so this queries the Spotify API for Playlists the user wants to keep track of
and manages changes with git.  
This tool provides an simple command line interface to the user to:  
* automatically store the state of a playlist and add this version to a git repo
* ~~get a diff between 2 versions~~
* ~~automatically create an old version of a playlist in your account~~
*not ready yet*

## Installing 
1) Clone the repository, compile it using the dotnet tool.  
2) Drop the compiled .dll in an empty folder along with the conf.json template
3) tune the settings in the ```conf.json```
4) if you want to check for playlist changes on a regular basis setup an cron job, calling the program with the ```--cron``` flag

## Configuration 
1) open the ```conf.json``` file
2) enter your spotify username and the spotify api credentials. You can get those 
in the Spotify Developer [Dashboard](https://developer.spotify.com/dashboard)
3) enter the URIs of the playlist you want to keep track of into the "Playlists list"
4) Save the file and put it in the folder with the executable
5) call ```dotnet SpotifyVersioning.dll --init```
You're now ready to go!

## Used libraries 
this project uses the following libraries: 
* [libgit2sharp](https://github.com/libgit2/libgit2sharp)
* [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET)
* [commandlineparser](https://github.com/commandlineparser/commandline)
* [Newtonsoft JSON](https://github.com/JamesNK/Newtonsoft.Json)

### This is still in the beginning, so far only the cron feature works

