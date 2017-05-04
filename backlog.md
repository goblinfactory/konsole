# Roadmap

## [2.1]

### fix

- (I think this is done???) proper multithreaded support across windows. (if you have two windows and two threads, different colors) then it fails to synchronise the writes.
- menu will crash if you give it two menuItems with the same shortcut key. (work it out, not rocket science!)
- manually test Konsole.Sample, currently giving very odd behavior! (if you resize window it dies, if your window is not wide enough, or, if the cursor is already near the bottom of the screen and scrolling kicks in, you're stuffed.)
- Konsole sample - demo must make sure window is wide enough.
- fix : allow user to specify exit character, e.g. `q` or `x` in addition to `ConsoleKey.Escape`.
- allow menu to run code witout having to send it to a 'window', i.e. can send to the default console.


### Adding

- Menus : `Menu`, `MenuItem`
- new 1 liner `ProgressBar`.

### breaking changes

- refactor ProgressBar Constructors : move IConsole to front and make sure all parameters are in order.
- ProgressBar to switch to 1 liner
- support extended style from `v2` by passing in `PbStyle.Simple`, `PbStyle.Extended` options.

# backlog

### Backlog 
- progress bar to have background color and border width, for simple funky PBs!
- be able to print ColorStrings (so that they can easily be passed around) 
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
