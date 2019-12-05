# Roadmap

## BUSY NOW 4.0.1

## Busy NOW (NEXT)

- #31 : fix bug when drawing window 3 lines tall.

- some references to investigate, and ideas
  - fast io stack question  : https://stackoverflow.com/questions/2754518/how-can-i-write-fast-colored-output-to-console
  - low level io, msdn docs : https://docs.microsoft.com/en-us/windows/console/low-level-console-output-functions
  - need to detect platform : https://mariusschulz.com/blog/detecting-the-operating-system-in-net-core
  - fast strings            : https://www.stevejgordon.co.uk/creating-strings-with-no-allocation-overhead-using-string-create-csharp
  - http://cgp.wikidot.com/consle-screen-buffer
- other console libraries to look at their source code?
  - console itself?
  - https://github.com/Athari/CsConsoleFormat to render an alternative Console emulator?
  

  with faster console rendering I can start to consider more avanced controls
  - http://elw00d.github.io/consoleframework/examples.html

  

## BUSY NEXT

- speed, speed, speed! direct draw to UX via whatever means after detecting platform. 
- #22 : investigate input and output redirection and using Konsole as part of a build pipeline for handling parallel build console output.
- investigate simplifying the console window creation using similar properties dto, so that I can configure borders lines with backgrounds.
- when setting a background color, as well as including Border line thickness, then the border line should share the window background colour. * I think this may only be a problem with splitleft, and splitright.
- investigate sponsorship options, see
  - github: # Replace with up to 4 GitHub Sponsors-enabled usernames e.g., [user1, user2]
  - patreon: # Replace with a single Patreon username
  - open_collective: # Replace with a single Open Collective username
  - ko_fi: # Replace with a single Ko-fi username
  - tidelift: # Replace with a single Tidelift platform-name/package-name e.g., npm/babel
  - community_bridge: # Replace with a single Community Bridge project-name e.g., cloud-foundry
  - liberapay: # Replace with a single Liberapay username
  - issuehunt: # Replace with a single IssueHunt username
  - otechie: # Replace with a single Otechie username
  - custom: # Replace with up to 4 custom sponsorship URLs e.g., ['link1', 'link2']

## BUSY NOW vers 4.0.1

- try to find some way to do resizing tests? mmm, that's quite challenging. 



## BUSY NEXT 4.1

- investigate if I can make some means of redrawing the screen onResize like winforms. 
- update : brief investigation and this looks like something that would require DLL imports for windows.
    - so possibly something like a community extension. i.e. you install Konsole, and if you want Redraw, like you have in Windows.Forms, you install Resizer, which also gives you 
      ability to lock console resize. (needs some thought.)

- mouse support, draggable windows?

## October 2019

Below items are just ideas. The true roadmap is driven by solving problems I encouter as I use Konsole to help me write my own console test, benchmarking, study and micro services projects.

- convert Konsole.Core to dotnet standard, and compile a different Konsole for each platform, that uses the correct wrapper for each platform.
- Konsole.Mac, Konsole.Windows, Konsole.Headless, Konsole.WebAssembly ?


### Unsorted backlog

- dialog with restore background.
- escape to exit, with pop up dialog
- text renderer for approvals to open a left and right comparison, what you supposed to get, what you actually got.
- mouse support?
- restore window backgrounds?
- draggable windows?

- make it all cloud based? take a look at support xterm? https://github.com/xtermjs
- support Mac via ncursors. (provide a multiplatform support in the build to dynamically link in ncursors on Mac via .NET)
- GetCursorVisible is not support on Mac (mono?) platform. Check for platform and ignore setting cursor visible. This is not critical and should not bomb an application on mac.
- move Konsole.Layouts to main namespace so that SplitLeft and SplitRight are automatically available on windows.

### Daft plan for ver [6.1]

- Touch support for mobile and tablet
- Use Konsole constructs as a way to build really (REALLY) simple cross platform applications with very few lines of code.  (TBD) Needs an example.
- e.g. what if I could take this 10 line powershell script, and turn it into a simple application I could run via my mobile?

### Daft plan for ver [6.0]

- Replacable console window.
  - Launch a console app without needing a console window. 
    - So that this can be used as a quick and dirty replacement when prototyping some functionality. For example, can use feature switching to turn a feature to be tested on, and the console window can be a temporary replacement for capturing or displaying a menu without having to make expensive WPF changes, so that the real feature under test, can be prototyped and tested quickly, and later baked into the controlling WPF application.
  - be able to "launch" a console app, from a script to achieve a task.
- Standard interface for console replacement allowing for
  - multiple platforms automatically built using build script file referencing? (.net standard or PCL bait and switch)? TBD
  - automatically support the appropriate platform
  - Mac, Nix, Windows, Mobile, Tablet, (web) Html Browsers
- pluggable graph library for viewing realtime graph updates inside the console window, without leaving "text" mode.
  - e.g. run a script to start process x,y,z and then monitor the results in realtime, requests per second, response time 99th percentile, total users etc.

### Daft plan for ver [5.0]

- Manually scrollable windows. 
  - Press tab to switch between `active' window.
  - switching `window` will highlight active window border.
  - configurable scroll and up down keys, default to arrow keys and page up and down.
- Masked input : to replace readline.
- Form input
  - array of inputs with X,Y positions
  - automatic keyboard support to navigate between active input
  - configurable submit key, default = enter

### Plan for ver [4.0]

- capture StdIn, StdOut, StdError, so that I can capture output from libraries that write to the console, e.g. `BenchMarkRunner.Run<SUT>()`
- move namespace for Layouts into root, or convert to partial classes so that these extensions are immediately accessible without having to find them with resharper or know about the namespace beforehand.
- Migrate to C# 7
- full async support
- simpler libraries, e.g. `Konsole`, `Konsole.ProgressBar`, `Konsole.Windows`, `Konsole.TestData` : so that users can only import the components that are required.  Also, so that `ProgressBar` can be released as a .net standard comoponent without the rest?
- rename package to 'Konsole'? If I can get this package name in Nuget.
- Investigate if `.Net Standard` support makes sense. If not, explain or prove why, in case this ever changes.

## [unreleased]

### Adding

### to fix

---- 

(5.3.18) some of these have been fixed and are in the latest release. I need to review the bug list and remove those that have been fixed or no longer apply.

- menu appears to be printing something to the console below the menu. run the sample app, and press and hold arrow key, and see black ' ' characters suddenly appear and start overwriting the main green demo screen area.
- window ...on a window, seems to not work! (scrolling areas dont match)
- detect screen width, so that we can make sure when printing off edge of screen this doesnt cause a problem.
  - `System.ArgumentOutOfRangeException: 'The value must be greater than or equal to zero and less than the console's buffer size in that dimension.
Parameter name: left
Actual value was 117.'`
- wire up ?? hook into the screen resize to support redrawing on resize? (see what can be done so that resize doesnt kill a window'd console application or script)
- find out why resizing a console window when printing beyond end causes such a mess?
- menu refresh does not draw the menu border
- decide if we really must reset (redraw) the screen when we create new windows, i.e. perhaps only do that when we change the color in the new window? 
- menu will crash if you give it two menuItems with the same shortcut key. (work it out, not rocket science!)
- fix : allow user to specify exit character, e.g. `q` or `x` in addition to `ConsoleKey.Escape`.
- allow menu to run code witout having to send it to a 'window', i.e. can send to the default console.

### Backlog
- add ability to clear the screen with a background color. So that I can quickly change the console green window for the sample demos. The progressbars (slim and two line) will look more profesional with a different background colour, typically black.
- investigate a custom console as a mobile application? automatically converting to Ionic-ish or something similar. i.e. take the console application and make it convertible into
  an exe, wpf mobile app, website etc.
- automatic API documentation using this script (https://gist.github.com/formix/515d3d11ee7c1c252f92) and this msbuild task. 
- be able to use a Keyboard to wait for upper or lowercase items.
  - will be great for networking tests when 'r' and 'R' could possibly do complimentary or reverse actions.
- constructor should not allow creating windows that overlap the bottom of a visible window.
- when opening a window that is clearly inline, e.g. with only height and width, move the cursor to below the window,
   - so that any normal console output appears below the window and we can safely multithread...sweet!
   - perfect for working with powershell ;)
- be able to use Konsole's ProgressBar and window from Powershell https://stackoverflow.com/questions/41549987/powershell-progress-bar-when-scrolling
  - write demo showing scrollable content in a sub window
  - might need to update Konsole to be able to take console redirection? piped content?
- remove IConsole from DoCommand. Not needed, as you always have an instance you are working on.
- detect screen resolution so that we can use the whole console by default.
- detect screen resizing, so we can have sticky boxes if needed.
- update constructor signatures so that when creating instance of class you can easily tell what property values are. (I was unable to tell and kept making mistakes myself, doh!) e.g. ` var x = new Window(1,2,10,20,...)` is `10` the width or height?
- dont allow overlapping sibling windows, and child windows must fit fully inside their parent windows.
- support border.None on Window so that I can remove access to the constructors to force all windows to be concurrent.
- detect screen resize and redraw any windows.
- update all the sample code for the main website, to use the new slimline (itemless) progressbar as well as demo for using doubleLine.
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
