# Screencap :: Video screen capture for Windows C#
Screen video capture software for Windows written in dotNet c#

This will record video of your screen in windows. 

Features
- FPS control
- Video Quality Control (0 - 100) ; 100 being highest quality. 
- Basic save functionality 

# Notes :: TODO
- This will always output a .avi file at the moment for its container, more work will be done later.
- No built in audio is supported at the moment 
- This does NOT record the mouse location however will be added as optional extra.
- Exceptions will happen — This is certainly a work in progress.
- As of 21-12-2016 the output directory needs to exist. 
- There are bugs at the moment, lots of them.

# Instructions
Should be straight-forward enough. Open the solution with Visual Studio, VS2013 was used for the production of this however there are no obscure dependancies so it should be fine even from an msbuild command.

Opneing the project there is a dependancy on `SharpAvi`, this should be included in the repo, if not just get the latest version from nuGet. 

Start recording and you will see an output: 
![alt tag](http://i.imgur.com/HWGX2T5.png)

Stop the recording and save to finalise: 
![alt tag](http://i.imgur.com/HWGX2T5.png)

Enjoy, feel free to expand and submit issues. There will be limited support for this however not extensive as this was only written to serve an immediate purpose without paying for screen capture software loaded with adWare. 
