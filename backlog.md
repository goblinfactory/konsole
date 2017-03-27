# backlog

### Backlog 
- fix the window constructors, second parameter is sometimes height, and sometimes not!
- Move TestData to seperate nuget `Goblinfactory.TestData`
- support progressbar with text to left, not below. (for more compact displays when using lots of progress bars.)
- multiplatform
- overlapping windows and scrolling.
- clipping when printing on overlapping windows - this may not be possible without being very slow, or using native window calls. This might also contradict the nature of the 'window' as being a thing that allows you to quickly print to your window, so a sub window is just a shortcut to actually print to that window.
- spawn multiple new Console windows for "remote window"
- print to remoteWindows e.g. system monitoring. (needs to be add-on `Konsole.Remote`)
- appharbor CI/CD testing with fake or cake.
- (not for a while!) make Window class thread safe?
- add more tests for out of range values
- move Draw to Window. 
- Add title to box drawing and window open.
- Auto-documentation. (full api documentation)
- simple menu system.
- Merge two window borders to create neat single border.
- fluent window syntax to open multiple connected windows, e.g. ` Window.XXX(... ).Right(20, out col1).Right(20, out col2)`
- Investigate if possible to print using native windows and Mac, Linux API's and switch out depending on platform detection, fallback to slower rendering.
