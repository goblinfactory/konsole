# Change Log

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased] will become [2.0.0.0]

New core `Window` functionality, plus big class renames, main interface `IConsole` remains unaffected between `1.0` and `2.0`.

### Breaking changes

- rename `Console` to `BufferedWriter`
- rename `ConsoleWriter` to `Writer`
- rename `.LinesText` to `BufferWritten` 
- rename `HilighterBuffer` to `BufferHilighted`
- rename `.Buffer` to `BufferWrittenString` 
- rename `.TrimmedLines` to `.BufferWrittenTrimmed`

- `BufferedWriter` methods standardised.
  * `.Buffer` `string[]` Get the entire buffer (all the lines) regardless of whether they have been written to or not, untrimmed.
  * `.BufferString` `string` Get the entire buffer (all the lines) regardless of whether they have been written to or not, untrimmed, as a single `crln` concatenated string.
  * `.BufferWritten` `string[]` Get the entire buffer . Only the lines that have been written to, from topmost to bottommost, untrimmed.
  * `.BufferWrittenTrimmed` `string[]` Get the entire buffer . Only the lines that have been written to, from topmost to bottommost, Trimmed.
  * `.BufferWrittenString` `string` Get the entire buffer for all lines written to, as a single `crln` concatenated string.
  * `.BufferHilighted` `string[]` returns an 'approve-able' text buffer where each character is represented by 2 characters with one of of them representing the background color of the buffer.

### Backlog

- Optional borders for windows
- Merge two window borders to create neat single border.
- allow for printing off the screen without crashing.
- scrolling.
- faster window printing. (currently prints character by character, very ...VEERY SLOW!) not good enough!
- Investigate if possible to print using native windows and Mac, Linux API's and switch out depending on platform detection, fallback to slower rendering.
- fix clear issues. Demo program not clearing properly.
- Window not printing `ForegroundColor` correctly.

### Added

- new method `BufferString` 
- new property `Cell this[int x, int y]` on `BufferedWriter` (allows for interrogating fore and background colour at X,Y position on a buffered writer.)
- new core windowing functionality via class `Window` (subclass of `BufferedWriter`) Usage as follows

```csharp

    var client = new Window(0, 0, 40, 10);
    var server = new Window(41, 0, 40, 10);
    server.WriteLine("Server started, listening on 'tcp://*:10001'.");
    ...
    client.WriteLine("enter commands, exit to quit");
    ...

```

### Fixed

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