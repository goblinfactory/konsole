# Change Log

The format is based on [Keep a Changelog](http://keepachangelog.com/) 

## [6.2.0]

### Added

- Form Write now supports fields as well as properties.

## [6.1.0]

### Added

- `Konsole.Platform.Windows.HighSpeedWriter` (native windows driver) now included and no longer an external package.

## [6.0.0]

### Added

- new `IConsole` interfaces for simpler abstractions, see new contract docs [here]

### fixed

- #40 WriteLine conflicts with String.Format, support string containing {0} {json} tokens.
- #41 If a `Write` ends exactly on the last column, then the cursor is not advanced to the next line. 

## [5.4.4]

### Added

- Printing objects with nullable fields now prints "Null" for any nullable fields.

### Fixed

- #39 Add double to NumericTypes in FieldReader.

## [5.4.3]

### Fixed

- #43 Form.Write(null) throws exception, new Form().Write(null); should not throw exception. Instead it should write "Null"

## [5.4.2]

### Fixed

- `WhenOpeningInlineShould_open_window_at_current_cursorTop()` : OpenBox (inline) when supplying only a width and a height was not opening the new inline window at the current cursor position.

## [5.4.1]

### Fixed

- `WhenOpeningInlineShould_open_window_at_current_cursorTop()` : OpenBox (inline) when supplying only a width and a height was not opening the new inline window at the current cursor position.

## [5.4.0]

### Added

- `Window.OpenBox` : Open a styled floating or inline window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double. Returns a window instance representing the window inside the box. The returned instance is threadsafe.
- #39 Add double to NumericTypes in FieldReader.

## [5.3.3]

### Fixed

- when not supplying the x,y start position for a window, the window was supposed to be created inline, instead the window was being created at 0, 0. Fixed, now it's created at the starting Y position and left = 0.

## [5.3.2]

### Fixed

- `Window.Open` now returns a threadsafe window. (`ConcurrentWindow` wrapping the newly created window) All static constructors now return threadsafe windows.

## [5.3.1]

### Fixed

- fixed nuget tags

## [5.3.0]

### Added

- `new ConcurrentWriter()` now returns a thread safe concurrent writer that continues writing to the current console as if it were the console. Previously the concurrentwriter required a window instance. That still exists but you can now create a `ConcurrentWriter` without needing to first create a window. This allows for thread safe writing to the console without needing a window. See the new section in the readme under `Threading` for more information and for examples.

## [5.2.0]

### fixed (busy)

- when rendering a window 3 lines tall the second row line char is incorrect.

```
    ┌──── headline ────┐       ┌──── headline ────┐
    ─ content here     ─  =>   │ content here     |
    └──────────────────┘       └──────────────────┘
```

### added

* `myWindow.SplitTopBottom()`
  
- : Split the current window Top and bottom and return a tuple. Default to use a single middle line, instead of the older double. (Was missing from previous release)


## [5.1.0]

### fixed

- `stackoverflow` bug when calling SplitColumns.

### added

- `myWindow.SplitLeftRight()` : Split the current window left and right and return a tuple. Default to use a single middle line, instead of the older double.

```csharp
    void Fill(IConsole con) => { 
        con.WriteLine("one");
        con.WriteLine("two");
        con.WriteLine("three");
        con.WriteLine("four");
    }
    (var left, var right) = win.SplitLeftRight("left", "right");
    
    Fill(left);
    Fill(right);
    
    // gives you   ...

    ┌ left ─┬─ right ┐
    │two    │two     │
    │three  │three   │
    │four   │four    │
    └───────┴────────┘    
```


## [5.0.1]

- more tests and small stabilisations.

## [5.0.0]

- full .NET core release! (Konsole is a .NET standard 2.0 package) and (Konsole.Platform.Windows is a .NET framework 4.6.1 package)
- package is built and tested on both Ubuntu and Windows on Github using github actions for devops. (see dotnetcore.yml)

## [4.2]

- #32 : speed, speed, speed! (speed improvements)
- Window.DoCommand made static

## [4.1]

- #33 : add "Disable screen resizing".Available as new method `.LockConsoleResizing()` on `Window`.

## [4.0.1]

- upgrade devops builds to use new Github pipelines, convert to new .net core project format and use dotnet test? possibly include this as key 4.0 update.
- Added SplitRows with wildcard support for creating any shape UI, using rows and columns.
- Move Konsole Layouts and Konsole Forms to main Konsole namespace, so that it's available out of the box and more discoverable, so that users can use SplitLeft, SplitRight, SplitTop, SplitBottom.
- Moved drawing into main Konsole Namespace.

## [3.4.1]

- #28 : fix crash after resize (rough Proof of concept without tests, manual testing only due to complications of testing window resizing, and want to get a patch out quickly)

## [3.4.0]

### Fixed

- #26 : fix bug where ProgressBar and ProgressBarSlim would report incorrect percentage (out by 1%) for some values.

## [3.3.0]

### Fixed

- #18 : Nested floating windows dont scroll properly.

### Added

- New attributes, `AbsoluteX` and `AbsoluteY` to `IConsole` so that you can know the absolute position of a window, in addition to it's relative position to it's parent.
- Added nested window demo code to the sample project.

## [3.2.1]

- had a problem uploading package to nuget, so created a new release to see if I can get past it with a fresh upload. (previous upload was hanging at 17 hours, doh!)

## [3.2.0]

### Fixed

- Correct version number.

##[3.1.0]

- deleted. (the Konsole.dll was given the incorrect version number.)

### Fixed

- [Fixed serious bug write and write-line when scrolling results in first Write's text being lost #12](https://github.com/goblinfactory/konsole/issues/12)
- Accidentally included `Konsole 0.0.2` as a dependancy in the [3.0.1] packages. Removed that.

## [3.0.0] & [3.0.1]

- quite a lot, leaving this blank-ish and will update shortly.
- highlight changes
    - threadsafe progressbars and windows, with tests for thread-safe-ness.
    - new class `ConcurrentWriter` to handle thread sychnonisation. 
- More or less, added menus, lots of breaking changes of signatures (cleanups to make easier to understand)
- Highlight change is a new slimline itemless `ProgressBar` with option to revert to old behavior.
- New draft `Menu` with option to have menu items that run as background tasks. (useful for writing simple client-server demo projects with client and server each outputting to different windows.) 

### Added

- Menus
- ProgressBarSlim

## [2.1.0] - 2017-05-07

### Added

- `Window.TopHalf()`
- `Window.BottomHalf()`
- `Window.OpenInline(console, height)` : opens a new window 'inline' full width of the console, 'height' rows tall, and moves the cursor to below the window. See `Window.OpenInlineShould` test.
- `MockKeyboard` : Mock keyboard for queueing up keystrokes. Includes Autoreply. Use to simulate user input during unit tests.
- `IReadKeys` : for console input.
- `Menu` with `MenuItem`: for rendering menus and capturing user input, see `Konsole.Sample.Program` for examples.
- Threadsafe windows. Windows opened with `Window.Open` and `Window.OpenInline` are now threadsafe. Specifically you can now reliably create multiple output windows, and have seperate threads e.g. `Task.Run` threads for each window. See `Konsole.Tests`Slow\WindowConcurrencyTests`.
- new Test project `Konsole.Tests.Slow` for slow running tests. Specifically concurrency tests, that take a few seconds each. 3 to 5 seconds on my dev laptop.

## [unreleased]


## [2.0.2] - 2017-03-20

### Added 

- Can set `ProgressBar.Max` after creating a progressbar, and UX will be updated.

## [2.0.1] - 2017-03-20

### Fixed - high

- fix `ProgressBar` constructor not threadsafe.
- `ProgressBar.Refresh` with `null` text throws exception.

### Added : medium

- new readonly property `ProgressBar.Y` : Current Console Y position.
- new readonly property `ProgressBar.Line1` : Current Line1 Text.
- new readonly property `ProgressBar.Line2` : Current Line2 text.
- Update Sample  paral`(l)`el test, to use fake directories and fake file names.
-  `TestData.MakeFileNames` : generate random (unique) file names. `public static string[] MakeFileNames(int howMany = 4200, params string[] extensions)`
- `TestData.MakeNames` now supports option to generate names without special characters.
- `TestData.MakeObjectNames` : New test data method to generate believable random object names `public static string[] MakeObjectNames(int howMany = 4200, string format = "{0}{1}")`

## [2.0.0.0] - 2017-03-18

New core `Window` functionality, plus big class renames, main interface `IConsole` has changed from `1.0` and `2.0`.

### Breaking changes
- refactor all constructors, move `IConsole` parameter to 1st param.
- rename `Form.Show` to `Form.Write` (form is written at current cursor position, and position is updated)
- change `IConsole` `WindowWidth` from a `method` to a `property`.
- rename `Console` to `Window`
- rename `ConsoleWriter` to `Writer`
- rename `.LinesText` to `BufferWritten` 
- rename `HilighterBuffer` to `BufferHilighted`
- rename `.Buffer` to `BufferWrittenString` 
- rename `.TrimmedLines` to `.BufferWrittenTrimmed`
- rename 'echo' to (isMockConsole and invert) or, override behavior and remove altogether from Window.
- `Cell` class gets extra property, `Background`
- `Cell` property rename `.Color` to `.Foreground`

- `BufferedWriter` methods standardised.
  * `.Buffer` `string[]` Get the entire buffer (all the lines) regardless of whether they have been written to or not, untrimmed.
  * `.BufferString` `string` Get the entire buffer (all the lines) regardless of whether they have been written to or not, untrimmed, as a single `crln` concatenated string.
  * `.BufferWritten` `string[]` Get the entire buffer . Only the lines that have been written to, from topmost to bottommost, untrimmed.
  * `.BufferWrittenTrimmed` `string[]` Get the entire buffer . Only the lines that have been written to, from topmost to bottommost, Trimmed.
  * `.BufferWrittenString` `string` Get the entire buffer for all lines written to, as a single `crln` concatenated string.
  * `.BufferHilighted` `string[]` returns an 'approve-able' text buffer where each character is represented by 2 characters with one of of them representing the background color of the buffer.

### Added
- new `Window` class, with significant functionality, ... allowing users to print to windows sections of the screen, including either clipped or scrolling of output when out exceeds window.

```csharp

    var client = new Window(0, 0, 40, 10);
    var server = new Window(41, 0, 40, 10);
    server.WriteLine("Server started, listening on 'tcp://*:10001'.");
    ...
    client.WriteLine("enter commands, exit to quit");
    ...

```
- more demos, and demos now more useful for learning how `Konsole` works. See `Konsole.Sample` project.
- refactored demos to seperate demo classes, seperating functionality being demoed.
- Faster window printing. was printing character by character, very ...VEERY SLOW!) not good enough!
- `BackgroundColor` added to `IConsole`
- new method `BufferString` 
- new property `Cell this[int x, int y]` on `BufferedWriter` (allows for interrogating fore and background colour at X,Y position on a buffered writer.)


### Fixed
- fix bug , printing off screen was crashing.
- removed quite a bit of duplication in the unit test code.
- fix : Window sometimes not printing `ForegroundColor` correctly.
- fix clear issues. Demo program not clearing properly.
- fix bug with `PrintAt`, `Write` and `WriteLine` with text that overflows, causing crash.
- fix bug when calling clear on bufferedWriter and crashing. (was not resetting Y position.)
- fix bug, calling `refresh` on progress bar changing cursor position.
- fix bug, Write (not WriteLine) causes progress bar to print from last X position and overflows.
- fix bug with `ProgressBar` not moving cursor down two lines to give `pb` space to display.

## [1.0.0.0] - 2016-03-04


- new very simple TestData `Konsole.Testing.TestData` class to help when building and prototyping console utilities and applications.
- renamed MockConsole to TestConsole. (It's not a mock, it's a real active class with important behavior to help build testable code.)
- Split IConsole into IWriteLine and IConsole to allow for being more specific about your dependancies, .i.e. what exactly your class depend on when you state you require a IWriteLine as a dependancy, tells you a lot more than 'I Need access to the console'. (Most classes often simply need acess to write a few bits and bobs to the console, in which case I expect that IWriteLine will be the most commonly used interface.) 



## [0.0.6] - 2017-02-01

### Changed

- oops, sposed to be CamelCase, not pascalCase, i.e. captalise class names in `name.page.ts`, so that `yo gf:i2 test-apple` generates `class TestApplePage`

## [0.0.5] - 2017-02-01

### Added

- Support page names with hyphens, convert the name into camelcase so that the generated `{pagename}Page` class is valid. e.g. `my-games` becomes `class myGamesPage` instead of `class my-gamesPage`.

## [0.0.4] - 2017-02-01

### Changed

- fix incorrect documentation stating that the default command is `yo gf i2 {pagename}` that doesnt work. Correct syntax requires colon i.e. `yo gf:i2 {pagename}`

## [0.0.3] - 2017-01-31

### Added

- first useful generator, `yo gf i2 {pagename}` that generates the 1 folder + 3 files.