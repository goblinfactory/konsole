# Roadmap

## [unreleased]

### Adding

### to fix
- fix or investigate - bug in ZeroMQ samples; scrolling? write/write/writeLine appeared to only scroll the last write on the last line, huh!
  - ` w = new Window(...); for(int i = 0;i<50) { w.Write("PART 1"); w.WriteLine("PART 2")}`
  - when code above executes, only PART2 get's scrolled in the window! DOH! 
- decide if we really must reset (redraw) the screen when we create new windows, i.e. perhaps only do that when we change the color in the new window? 
- menu will crash if you give it two menuItems with the same shortcut key. (work it out, not rocket science!)
- fix : allow user to specify exit character, e.g. `q` or `x` in addition to `ConsoleKey.Escape`.
- allow menu to run code witout having to send it to a 'window', i.e. can send to the default console.

### Backlog
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