#AlbumRecorder software to record vinyl or tape to digital format

##Installation

* Extract the zip file to a folder of your choice
* Open the folder in Windows Explorer, find AlbumRecorder.exe, right click it, and choose "Pin to Start" and/or "Pin to Task Bar".

##Running the program

Start the program (from the Start Menu, Task Bar, or by double-clicking on AlbumRecorder.exe), and you will see this screen:

![Startup Screen](/Images/MainScreenBlank.jpg)

###![Record button](/Resources/imgRecord.png) Recording

To record, press the record button ![Record button](/Resources/imgRecord.png), when you will see this screen:

![Recording Screen](/Images/Recording.jpg)

Choose your recording device in the dropdown at the top, cue up your album, and press the Start button. The volume slider adjusts the recording volume (you may need to have a few tries to get this set correctly - ideally the peaks of the recording should nearly, but not quite, reach the edges of the blue area.

When side A is finished, press the Pause button, turn the album over, then press the Pause button again to restart.

When the recording is finished, press the Stop button, and you will be taken to the Normalize dialog, and then the Album Info Search dialog.

###![Normalize button](/Resources/Normalize.png) Normalizing

Normalizing adjusts the volume of the whole recording so the peak level is at the specified level (usually 1). This means all your recordings will come out at a similar volume when split into tracks and saved to your Music folder.

![Normalizing Screen](/Images/Normalize.jpg)

Just set the maximum volume you want (between 0 - silent and 1 - loudest) and press the OK button.

###![Album Info Search button](/Resources/imgSearch.png) Album Info Search

![Album Info Search Screen](/Images/Normalize.jpg)

Type in the album artists and title, and press the Search button. The program will search the MusicBrainz database for details of the album tracks. The search results appear in the list.

Click on the correct album, and press the Select button (the Cancel button changes to Select when you have chosen an album).

If no information is found in the MusicBrainz database, the program will automatically search the Gracenote database. Unfortunately the Gracenote database does not contain track length information, so in this case you will automatically be taken to the Split Album screen.

###![Split Album button](/Resources/imgSplit.png) Split Album into tracks

![Split Album Screen](/Images/SplitAlbum.jpg)

If the album details were not in the MusicBrainz database, so there is no track length information, you can use this screen to try to split the album into tracks automatically. Just enter the correct number of tracks, and press the Split button.

The program uses a BiQuad filter to filter out high and low frequencies, to get rid of any hiss, crackle and rumble from the recording, and looks for silences. You can adjust the minimum track length, silence threshold, music threshold, and filter parameters before pressing the Split button.

###Main track split editing screen

![Main Screen](/Images/MainScreen.jpg)

Once the album has been split into tracks, the main screen shows the start and end of each track. You can adjust the start and end points by dragging the red cursors, or by altering the values on the right (the little buttons increase or decrease the values by 0.1 sec).

Note the lock button in the toolbar. If it shows ![Locked button](/Resources/imgLocked.png), then moving a track start or end will keep the following gaps and track lengths the same, so the cursors will all move together. If it shows ![Unlocked button](/Resources/imgUnlocked.png), then each cursor moves independently. You can temporarily override the lock button by holding the shift key as you drag.

If you hold the Ctrl key as you drag, the all the gaps between tracks will alter by the same amount as the one you are dragging.

If you click anywhere on the music, that section of music will play (up to the next red cursor, or the end of the section). This way you can fine tune the track starts and ends, so you don't miss very quiet fade-outs or fade-ins.

If you right-click anywhere on the music, a pop-up will show you the position (in seconds), and the filtered volume level. You can use this to help you set suitable values for the silence and music thresholds (see Options and/or Split Album screens).

###![Save button](/Resources/imgSave.png) Save button

When you press the Save button, you are first shown the Track Details screen (see below), and then the music is split into tracks and saved to your music folder, in a folder structure My Music\Artis Name\Album Name\Track Name.

The details of the tracks split, volume, etc. are also saved into a project file, which you can later open to tweak anything, before resaving the album.

###![Track Details button](/Resources/imgTracks.png) Track Details screen

![Track Details Screen](/Images/TrackDetails.jpg)

Ensure all the fields are filled in correctly before saving. The image at the top left is the album cover - this is found by searching the MusicBrainz and Gracenote databases for the album artist and title. If no album cover is found automatically, you can click on the image to start a search. If nothing is found even then, you can search the Web for a picture, and drag it onto the form.

###![Save project button](/Resources/imgSaveProject.png) Save Project button

The details of the tracks split, volume, etc. are saved into a project file, which you can later open to tweak anything, before resaving the album.

###![Options button](/Resources/imgOptions.png) Options screen

![Options Screen](/Images/Options.jpg)

You can set the Music Folder (default is "My Music") and the Recordings folder (default is your temporary folder), and the silence filter parameters here.

###![Undo button](/Resources/imgUndo.png) ![Redo button](/Resources/imgRedo.png) Undo and Redo buttons

These buttons will undo and redo any actions you take in the program.
