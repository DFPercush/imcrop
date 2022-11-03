# ImCrop

Image cropping tool.

This program is designed for quickly cropping a temporary directory full of images and saving them in a more permanent location, with the option to save as jpg or png.  Png will preserve 100% quality but takes a lot of space. Jpg is suitable for images where more compression is acceptable and high frequency details are not as prevalent.


## Setup

If you have Visual Studio on Windows, open the .sln file and hit F7. Or,

From the command line / terminal, cd to the source folder and run `nuget restore` followed by `msbuild` or `dotnet build.`


## Usage

First select the source directory at the top left. Then choose a location to save the results on the top right.

when you select a source folder, the list on the left should populate with all the recognizable image files in that folder. When you save, it will be removed and the list will move on to the next one. Yes, this does delete the original file. On Windows systems it will attempt to move it to the recycle bin. On linux, it's just deleted.


## Mouse / Key bindings

`Left button` - Pan, edge select and drag
`Right button` - Rectangle select (drag)
`Wheel` - Zoom
`D` - Duplicate file (make a copy)
`J` - Save as jpg
`P` - Save as png
`Delete` - Delete file

