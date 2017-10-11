# WinFormCAD
Uhh.. Yeah, So I needed a precise (linear) drawing program while I was at work, and couldnt install anything that did the trick.

https://imgur.com/LWi3WAx

Commands! (So far.. Cmon, it's only a day and a half old)
-Each command starts with a letter (not case sensitive)
1) "R" Relative / Global positioning toggle
2) "L" Create Line
3) "C" Move Cursor

"R" May be used on it's own,
"C" Requires two floats (one point)
"L" requires four floats while in default global positioning mode, but only two in Relative positioning mode

Acceptable delineaters are ";" "," and " " (empty space) 
-only choice type of delineater per command

The Gridsystem is in inches, and the will default to 96 PPI, with gridlines marking each 1/4".
The Gridsystem is not locked to the screen, and may be scaled by using the "+" and "-" buttons on the upper right.

Left clicking the grid will move(snap) the cursor. Cursor location is used as the starting point for all relative lines.

Right clicking the grid will move(snap) the cursor, and will ALSO Queue whatever point you just snapped the cursor to.
-Right clicking the grid a SECOND time will draw a line between the previous point, and your current point, then empty the "ClickQueue"
--Holding shift while right clicking will not empty the last point from the ClickQueue, to allow quick "path" creation.


TODO:
-Dimensioning lines (optional)
-Arcs
-Center point circles
-Improve Line metaData and selection box
-Add snap spacing (grid distance) modifiers
-Sob gently because I'm doing with with winforms
