# Change Log

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## [Unreleased] will become [2.0.0.0]

New core `Window` functionality, plus big class renames, main interface `IConsole` remains unaffected between `1.0` and `2.0`.

### Breaking changes

- rename `Console` to `BufferedWriter`
- rename `ConsoleWriter` to `Writer`
- rename `.Buffer` to `BufferWritten`

### Added

- new method `Buffer` : returns the entire buffer as an array of strings, regardless of whether anything was written or not, ignoring how many lines written, and not trimming any lines, e.g. an empty buffer will return spaces as default.
- new core functionality and class `Window` (subclass of `BufferedWriter`) Usage as follows

```



```

### Fixed

- ? fix bug when calling clear on bufferedWriter and crashing. (was not resetting Y position.)

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