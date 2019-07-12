# Scriptool
Scriptool is a Console application, written in C#, with some useful functions:



## Generate a QR code
Generate 3 types of QR codes:

- Text/Link: when scanned it will return a text or a link
- Autoconnect to a WiFi Network: when scanned it will connect to the specified SSID using the specified password(It supports only WPA and WPA2 authentication)
- Dial a phone number: when scanned it will return a phone number that you can call/send a message to


## Generate a password
Given the lenght of the password, the software will generate one. The password could contain these special characters: è,é,&,?,!,@


## Download a video from Youtube
Insert the link of a Youtube video, choose a quality from the available ones and download it.                                             The algorithm was written by me

## Others incoming...
<pre>







</pre>
## Known issues to fix:
1) Music videos can't be downloaded, this is because in the metadata requested by the app there isn't any link to the source of the video
2) Videos containing the underscore char in the ID can't be downloaded
