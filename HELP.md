## How To use this tool
####1) How to use the cron feature
the cron feature is fairly easy to use. Setup your config file as described in README.
Then call the program like this ```playlist-versioning cron --config *path to conf*```
If no errors occur, this single command queries all of your setup playlists and keeps
track of changes automatically. You can view the git repository by hand if you want
####2) How to use the diff feature
The diff feature is for quickly look for changes of a playlist. You can select a date
and the program will show all changes from this date on to now.
1) call the program like this: ```playlist-versioning diff *playlistname* --versions --config *path to conf*```  
    This will give you an overview of all dates changes were tracked.  
    Sample output: 
    ```
    Versions of playlist test  
    =========================  
    2019-02-25 23:08:48  
    2019-02-25 22:01:39  
    2019-02-25 21:36:06 
    ```
2) Now you can compare the latest version with any of the other versions.  
Just type ```playlist-versioning diff *playlist name* *date* --config *path to conf*```
The program is ok if you only supply the day, in case of multiple changes per day it will take the latest version
of that day. If you want to specify you can add a time by simply putting it into quotes: ```"2019-02-25 21:36"```
Seconds are ignored.  
Sample output(if your terminal supports color, even in color):
```
Changes in playlist: test
-spotify:track:0Ra3DxdKaqmMhUy2enFfb4:Drop Dead Cynical:Amaranthe
-spotify:track:2zzUpxEBD9O6OuhY4pqJuE:Fake It:Seether
-spotify:track:68FhagAoZr9Ld8oCp9JoYP:Temptation:Diana Krall
-spotify:track:2CpETKcs2iOHIG0bj0bApJ:Train Song:Holly Cole
```
"-" stands for removed files  
"+" stands for added files