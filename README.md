# SpotifySongNameApp
Quick and easy way for streamers to show their currently playing track from Spotify in OBS
I wrote this after hearing some streamers describe their frustration with other plugins that are pay for play or having to jump through a lot of hoops to get the song name to display on screen.

### Now updated to version 1.03

[Download the app already!](https://github.com/erikpt/SpotifySongNameApp/releases "Releases")

## Using SpotifySongNameApp with OBS
### To get this going
1. Launch OBS
2. Launch SpotifySongNameApp.exe
3. Move the app where you'd like. It's currently set to be the top-most window, so if you don't want it to interfere, move it to the edge of your monitor or to a secondary screen. Note: You'll have to click and drag the black background.  Don't try to drag by the Spotify logo or the text.
4. Add a WindowCapture object for [SpotifySongNameApp.exe]: Form1
5. Profit!

#### Here's a preview of the app's default appearance
![Image of app while running](https://github.com/erikpt/SpotifySongNameApp/blob/master/docs/defaultView.png?raw=true)

## Advanced Usage aka Editing the SpotifySongNameApp.exe.config file
This version includes the ability to change the following settings using an XML file. If this app gains traction I'll add a user interface to configure these settings, but in the meantime editing the config file is the way to go.  The SpotifySongNameApp.exe.config file should stay in the same folder as SpotifySongNameApp.exe.
![Image of XML Settings](https://github.com/erikpt/SpotifySongNameApp/blob/master/docs/Options.PNG?raw=true)
### Settings Reference
#### BlankOnNotPlaying
- Type: True/False
- Behavior: Setting this value to false will make "Nothing is Playing" show when there is no song playing.  The default behavior is to show nothing when there is no song playing
#### FontName
- Type: String of Text
- Behavior: Put the name of any Font that's installed on your system... Just make sure it has a "bold" version as that's the one that will be displayed
#### FontSize
- Type: Number
- Behavior: The default size is 15. Making this number larger will increase the size of song name text in the app, while making this number smaller will make the song name text appear smaller. I suggest using a value of 14 to 20 depending on which font name you specify. This must be a whole number (no decimals) and always 1/2 the WindowHeight or smaller.
#### FontColor
- Type: String of Text
- Behavior: Sets the color of the song name to be the HTML color code shown here. The default value is #32CD32 aka LimeGreen. Be sure to include the hashtag character in the color reference. I suggest using the [Google HTML Color Picker](https://www.google.com/search?q=HTML+color+picker) to find the hex value for the color you like.
#### ShowLogo
- Type: True/False
- Behavior: Setting this value to false will hide the Spotify logo
#### AllowScrollingText
- Type: True/False
- Behavior: Default value is true, which will scroll song names that dont fit in the available window space. The scrolling isn't smooth but works. If the song name fits within the window bounds, then nothing scrolls because it's unnecessary. Set this to False if you find the scrolling text annoying.
#### WindowHeight
- Type: Number
- Behavior: Change this to make the window larger. This will allow larger FontSize to be used. Remember the FontSize property must always be less than 1/2 of the WindowHeight. Maximum height is 400 to not be obnoxious.
#### WindowWidth
- Type: Number
- Behavior: Change this to make the window wider. This will allow you to fit more of a song name on the screen.  Maximum width is 3840, which is the width of a 4K screen.

### To quit the app
Right-click the taskbar icon and choose Close or right-click anywhere in the app and choose Exit.

## If you find this app useful
[Send some coffee money](https://paypal.me/erikpt?locale.x=en_US "PayPal.me")
