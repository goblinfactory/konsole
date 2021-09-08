
# experiment-01-scroll-concept

## set the scene

- two windows, splitLeft, splitRight
- in left window
- have a slow thread that prints a new line every 1 second
- display the buffer line number that represents the real offset of the top of the screen, i.e. history(buffer? check naming) 
- until the thread gets to the bottom of the screen, the offset should remain at 0.
- as printing causes the screen to scroll, the offset should start to increase to represent the total number of lines scrolled "off" the top of the window that are no longer visible.


## add some test behavior

- handle spacebar, start and stop the timer, show "running" or "paused" in status bar on bottom left.
- handle up and down arrow keyboard presses

##  when offset > 0 and up or down arrow is pressed

then "on up or down press"

- set window to clip mode. should still keep writing to the buffer but should not "echo" to parent window.
- window should render to parent all lines in `buffer` from [offset] to [offset] + [windowheight] : this should "scroll" the window.
- print "echo:stopped" in status bar 
    
##  when down press causes offset to = 0

- disable echo, i.e. resume normal print and window scrolling.

