# Roadmap


## [unreleased]

### Adding

- concurrentProgressBar


## [2.1]

### fix

- menu will crash if you give it two menuItems with the same shortcut key. (work it out, not rocket science!)
- fix : allow user to specify exit character, e.g. `q` or `x` in addition to `ConsoleKey.Escape`.
- allow menu to run code witout having to send it to a 'window', i.e. can send to the default console.

### Added

- new 1 liner `ProgressBar`.
- Window opened with `Window.Open` and `Window.OpenInline` are now ThreadSafe and each window can be written to seperately from different threads.


### breaking changes

- refactor ProgressBar Constructors : move IConsole to front and make sure all parameters are in order.
- ProgressBar to switch to 1 liner
- support extended style from `v2` by passing in `PbStyle.Simple`, `PbStyle.Extended` options.

# backlog

### Backlog 

- update change-log for new beta realeas.
- update readme and tutorials
- support simple menus without defining shortcut keys.
- fix bug in menu, not running menus with non numeric shortcuts.
- write unit test for running menus with shortcuts.
- fully async console library. Write console app using async all the way!
- bug , run demo, don't start right, start left, let finish, then start right = error.
- bug : setting progressbar refresh to (0) throws exception.
- progressbars to survive window scrolling!
- Draw line thickness to None, to draw plain text. Will require the whole merge story to be done. Should be quite straight forward but a little bit lengthy.
- progress bar to have background color and border width, for simple funky PBs!
- be able to print ColorStrings (so that they can easily be passed around) 
- fix the window constructor consistencies, second parameter is sometimes height, and sometimes not!
- Move TestData to seperate nuget `Goblinfactory.TestData`
- multiplatform
- overlapping windows and scrolling.
- clipping when printing on overlapping windows - this may not be possible without being very slow, or using native window calls. This might also contradict the nature of the 'window' as being a thing that allows you to quickly print to your window, so a sub window is just a shortcut to actually print to that window.
- appharbor CI/CD testing with fake or cake.
- add more tests for out of range values
- Add title to box drawing and window open.
- Auto-documentation. (full api documentation)
- Investigate if possible to print using native windows and Mac, Linux API's and switch out depending on platform detection, fallback to slower rendering.
- remote windows, for running console service apps, with remote console input and-or output, to combine with docker support so I can quickly and easily remote run some code, and have a semi decent local UI (with menus etc) for my admin and server monitoring.
- 