# playlist-versioning

## What is the playlist-versioning tool?  
Unfortunately Spotify lacks the option to see older versions of playlists,
so this queries the Spotify API for Playlists the user wants to keep track of
and manages changes with git.  
This tool provides an simple command line interface to the user to:  
* automatically store the state of a playlist and add this version to a git repo
* get a diff between 2 versions
* ~~automatically create an old version of a playlist in your account~~
*not ready yet*

## Installing 
**Raspberry Pi:**  
Since I experienced a bug that libgit2sharp could'nt find the compiled libraries you
have to compile the yourself:
1) clone the [libgit2 Repo](https://github.com/libgit2/libgit2) to your raspberry pi
2) follow the build instructions of the README in the libgit2 repo
3) look for the ```libgit2.so``` and remember the location
4) clone this repository to your computer (the pi can't build it itself)
5) build the project with ```dotnet publish -c release - r linux-arm --self-contained```
6) copy all files of the ```bin/release/netcoreapp2.1/linux-arm/publish``` directory over to your pi
7) make sure ```playlist-versioning``` is executable
8) copy the ```libgit2.so``` file in the directory you cloned the other files to
 and rename it to ```libgit2-572e4d8.so```
Otherwise libgit2sharp can't find it
9) Follow the generic steps in the *Configuration* Section

**Other Linux**
1) Clone the repository, compile it using the dotnet tool.  
2) Drop the playlist-versioning.dll into any folder you like
3) Follow the generic steps in the *Configuration* Section

## Configuration 
1) open the ```conf.json``` file with your favorite editor
2) enter your spotify username and the spotify api credentials. You can get those 
in the Spotify Developer [Dashboard](https://developer.spotify.com/dashboard)
3) enter the URIs of the playlist you want to keep track of into the "Playlists list"
4) create a directory that holds your playlist files later
5) enter the path of that folder into the config file
6) Save the file and put it where you like
7) call ```dotnet SpotifyVersioning.dll init --config *pathtoyourconfig*``` if on non arm linux  
if you're on a raspi call ```*pathtoexec/playlist-versioning init --config *pathtoyourconfig*```
 

You're now ready to go! To query the playlists call the program with the ```cron``` verb
 along with the path to the config file. *You can automate this using crontab*

## Usage
for explanation of functions like the diff function see the [Help File](HELP.md)  
the interactive mode isn't working with all features yet

## Used libraries 
this project uses the following libraries: 
* [libgit2sharp](https://github.com/libgit2/libgit2sharp)
* [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET)
* [commandlineparser](https://github.com/commandlineparser/commandline)
* [Newtonsoft JSON](https://github.com/JamesNK/Newtonsoft.Json)

### This is still in the beginning, so far only the cron feature works

