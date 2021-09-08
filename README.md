# <img src="docs/konsole.png" height="30px" valign='bottom'/> Konsole 

![Goblinfactory.Konsole](https://github.com/goblinfactory/konsole/workflows/Goblinfactory.Konsole/badge.svg?branch=master)
[![NuGet Status](https://img.shields.io/nuget/v/Goblinfactory.Konsole.svg?label=Goblinfactory.Konsole)](https://www.nuget.org/packages/IConsole/)
[![nuget](https://img.shields.io/nuget/dt/Goblinfactory.Konsole.svg)](https://www.nuget.org/packages/Goblinfactory.Konsole/) 
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) 
[![Join the chat at https://gitter.im/goblinfactory-konsole/community](https://badges.gitter.im/goblinfactory-konsole/community.svg)](https://gitter.im/goblinfactory-konsole/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Low ceremony, Fluent DSL for writing console apps, utilities and spike projects. Providing thread safe progress bars, windows and forms and drawing for console applications. Build UX's as shown below in very few lines of code. Konsole provides simple threadsafe ways to write to the C# console window. [See my notes on threading](docs/threading.md). The project is growing quickly with fast responses to issues raised. 

If you have any questions on how to use Konsole, please join us on Gitter (https://gitter.im/goblinfactory-konsole/community?source=orgpage) and I'll be happy to help you. 

**Version 7 alpha release progress (see release notes for whats new)**
| date | alpha release |
| --- | --- |
| 11/2/21 | (`7.0.0.5-alpha`)[https://www.nuget.org/packages/Goblinfactory.Konsole/7.0.0.5-alpha] |
| 13/2/21 | (`7.0.0.7-alpha`)[https://www.nuget.org/packages/Goblinfactory.Konsole/7.0.0.7-alpha] |

enjoy, cheers, 

Alan

![sample demo using HighSpeedWriter](docs/img/01-intro.png)

## Contents : V6

  * [Installing and getting started](#installing-and-getting-started)
  * [using static Console.ConsoleColor](#using-static-consoleconsolecolor)      
  * [IConsole](readme-iconsole.md)
  * [ConcurrentWriter](#concurrentwriter)
  * [Progress Bars](#progressbars)
    * [ProgressBar worked parallel example](#progressbar-worked-parallel-example)
    * [DoubleLine progress bar](#doubleline-progress-bar)
  * [Threading and threadsafe writing to the Console.](#threading-and-threadsafe-writing-to-the-console)
    * [Threadsafe static constructors](#threadsafe-static-constructors)
    * [new Window is not threadsafe](#new-window-is-not-threadsafe)
    * [Make it threadsafe](#make-it-threadsafe)
    * [ConcurrentWriter](#concurrentWriter)
  * [Window](#window)
      * [Floating constructors](#floating-constructors)
      * [Inline constructors](#floating-constructors)
      * [Fullscreen constructor](#fullscreen-constructor)
    * [Static constructors](#static-constructors)
      * [OpenBox](#openbox)
      * [Open](#open)
    * [methods and extension methods](#methods-and-extension-methods)
      * [PrintAt](#printat)
      * [PrintAtColor](#printatcolor)
      * [Write](#write)
      * [WriteLine](#writeline)
      * [SplitRows](#splitrows)
      * [SplitColumns](#splitcolumns)
      * [SplitLeft](#splitleft)
      * [SplitRight](#splitright)
      * [SplitLeftRight](#splitleftright)
      * [SplitTop](#splittop)
      * [SplitBottom](#splitbottom)
      * [SplitTopBottom](#splittopbottom)
    * [Nested Windows](#nested-windows)
    * [Window properties](#window-properties)
    * [Advanced windows with SplitRows and SplitColumns](#advanced-windows-with-splitrows-and-splitcolumns)
    * [Input](#Input)
    * [Clipping and Transparency](#clipping-and-transparency)
    * [Draw](#draw)
      * [Box](#box)
      * [Line](#line)
    * [Forms](#forms)
      * [Write](#write)
      * [Rendering Null objects](#rendering-null-objects)
      * [Rendering Nullable fields](#rendering-nullable-fields)
    * [HighSpeedWriter](#highspeedwriter)
      * [Getting started with HighSpeedWriter](#getting-started-with-highspeedwriter)
      * [HighSpeedWriter end to end sample](#highspeedwriter-end-to-end-sample)
    * [Copying code from the unit tests](#copying-code-from-the-unit-tests)
    * [Other .NET console libraries](#other-net-console-libraries)
    * [Why did I write Konsole?](#why-did-i-write-konsole)
    * [Debugging problems with Konsole](#debugging-problems-with-konsole)
      * [warning NU1702](#warning-nu1702)
      * [No visible output, blank screen](#no-visible-output-blank-screen)
      * [Corrupt output](#corrupt-output)
    * [MockConsole](#mockconsole)
      * [MockConsole vs Mock<IConsole>](#mockconsole-vs-mockiconsole)
    * [Building the solution](#building-the-solution)
    * [ChangeLog](#changelog)
    * [support me, please check out Snowcode, a free developer conference I hold every year at a great ski venue](www.snowcode.com)
    
## Nuget Packages

 * https://nuget.org/packages/Goblinfactory.Konsole/
 * https://nuget.org/packages/Goblinfactory.Konsole.Windows/

## Installing and Getting started

1. start a new console application 

```
dotnet new console -n myutility
```
2. add `Konsole` package 
```
dotnet add package Goblinfactory.Konsole
```

3. add the code in the same shown below to your  `void main(string[] args)` method
4. run your program

```
dotnet run
```

Will give you the screenshot on the right. If not, please [join our gitter chat and get some help.](https://gitter.im/goblinfactory-konsole/community)

have fun!

Alan

<img src='docs/img/02-nyse-ftse100.png' width='200' align='right'/>

```csharp

using Konsole.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.ConsoleColor;

class Program
{
    static void Main(string[] args)
    {

        // quick dive in example 

        void Wait() => Console.ReadKey(true);

        // show how you can mix and match System.Console with Konsole
        Console.WriteLine("line one");

        // create an inline Box window at the current cursor position 
        // 20 characters wide, by 12 tall.
        // returns a Window that implements IConsole 
        // that you can use to write to the window 
        // and create new windows inside that window.
        
        var nyse = Window.OpenBox("NYSE", 20, 12, new BoxStyle() { 
            ThickNess = LineThickNess.Single, 
            Title = new Colors(White, Red) 
        });
        
        Console.WriteLine("line two");

        // create another inline Box window at the current cursor position
        var ftse100 = Window.OpenBox("FTSE 100", 20, 12, new BoxStyle() { 
            ThickNess = LineThickNess.Double, 
            Title = new Colors(White, Blue) 
        });
        Console.Write("line three");


        while(true) {
            Tick(nyse, "AMZ", amazon -= 0.04M, Red, '-', 4.1M);
            Tick(ftse100, "BP", bp += 0.05M, Green, '+', 7.2M);
            Wait();
        }

        decimal amazon = 84;
        decimal bp = 146;

        // simple method that takes a window and prints a stock price 
        // to that window in color
        void Tick(IConsole con, string sym, decimal newPrice, 
           ConsoleColor color, char sign, decimal perc) 
        {
            con.Write(White, $"{sym,-10}");
            con.WriteLine(color, $"{newPrice:0.00}");
            con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
            con.WriteLine("");
        }
    }
}
```

## using static Console.ConsoleColor

If you will be using a lot of different colors throughout your application I recommend making use of the new C# `static using` language feature to make the code a bit easier to read.

before
```csharp
Console.WriteLine(ConsoleColor.Red, "I am red"); 
var box = Window.OpenBox("warnings", new BoxStyle() { Title = new Colors(ConsoleColor.White, ConsoleColor.Red) })
```
becomes
```csharp
using static System.Console;
...
Console.WriteLine(Red, "I am red"); 
var box = Window.OpenBox("warnings", new BoxStyle() { Title = new Colors(White, Red) })
```

## IConsole

This is the main interface that all windows, and objects that wrap a window, or that wrap the `System.Console` writer. It implements the almost everything that `System.Console` does with some extra magic. `IConsole` is a well thought out .NET System.Console abstractions. Use to remove a direct dependancy on System.Console and replace with a dependancy on a well used and well known console interface, `IConsole`, to allow for building rich 'testable', high quality interactive console applications and utilities.

For more information about the different interfaces [please see the full documentation for the contracts, as well as details of each interface here](docs/iconsole.md)

<img src='docs/img/iconsole.png' align='center' />



# Progress bars

## ProgressBar

Create a threadsafe one or two line progress bar. 

```csharp
    var pb = new ProgressBar(PbStyle.DoubleLine, 50);
    pb.Refresh(0, "connecting to server to download 5 files asychronously.");
    Console.ReadLine();

    pb.Refresh(25, "downloading file number 25");
    Console.ReadLine();
    pb.Refresh(50, "finished.");
```

You can create a `SingleLine` or a `DoubleLine` progress bar. If none is specified, the a single line progressbar is created.

```
var pb1 = new ProgressBar(max);
```
#### ProgressBar worked parallel example

<img src='docs/img/03-progressbar.gif' align='right' width='50%'/>

```csharp       
using Konsole.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

static void Main(string[] args) {

    var dirCnt = 15;
    var filesPerDir = 30;

    var r = new Random();
    var dirs = TestData.MakeObjectNames(dirCnt);

    Console.WriteLine("Press enter to start");

    var tasks = new List<Task>();
    var bars = new ConcurrentBag<ProgressBar>(); 
    foreach (var d in dirs)
    {
        var files = TestData.MakeNames(r.Next(filesPerDir));
        var bar = new ProgressBar(files.Count());
        bars.Add(bar);
        bar.Refresh(0, d);
        tasks.Add(ProcessFakeFiles(d, files, bar));
    }
    Console.ReadLine();
    start = true;
    Task.WaitAll(tasks.ToArray());
    Console.WriteLine("finished.");
    Console.ReadLine();
}    
```

#### DoubleLine progress bar

Double line progress bar is useful if you want to roll up and display the overall progress of a parent group, while displaying the names of the items being processed seperately. For example, when processing a number of folders and files inside folders, then use a DoubleLine `ProgressBar`.

```
var pb2 = new ProgressBar(PbStyle.DoubleLine, files.Count());
```

<img src='docs/img/04-progressbar.gif' width='75%'/>

## Open a progressbar inside a window

To open a progress bar inside a new window, just pass the window (`IConsole`) as the first parameter.

<!-- snippet: ProgressBarInsideWindow -->
<a id='snippet-progressbarinsidewindow'></a>
```cs
public static void Main(string[] args)
{
    var w = Window.OpenBox("tasks", 64, 9);
    var left = w.SplitLeft("files");
    var right = w.SplitRight("users");
    
    var pb1 = new ProgressBar(left, 100);
    pb1.Refresh(50, "hotel-california.mp3");

    var pb2 = new ProgressBar(right, 100);
    pb2.Refresh(10, "Clint Eastwood");

    Console.ReadKey(true);
    var pbl = new List<ProgressBar>();
    for (int i = 1; i < 6; i++)
    {
        var pb = new ProgressBar(left, 100);
        pbl.Add(pb);
        pb.Refresh(100, $"hello {i}");
        Console.ReadKey(true);
    }
}
```
<sup><a href='/src/Konsole.Samples/Demos/ProgressBars/ProgressBarInsideWindow.cs#L9-L32' title='Snippet source file'>snippet source</a> | <a href='#snippet-progressbarinsidewindow' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Gives you

<img src='docs/img/progressbar-inside-window.png'/>


## ProgressCharBar

<!-- snippet: ProgressCharBar -->
<a id='snippet-progresscharbar'></a>
```cs
// 'OpenBox' opens to fill the entire parent window.
// this test runs inside a mock console 22 chars wide, by 3 lines tall.
// hence the 22x3 box shown in the buffer below.
// -------------------------------------------
var box = Window.OpenBox("test progress");

// default color is green, default char is #
var pb = new ProgressCharBar(box, max: 4); 
pb.Refresh(1);
_console.Buffer.Should().BeEquivalentTo(new[]
{
    "┌─── test progress ──┐",
    "│#####               │",
    "└────────────────────┘"
});
```
<sup><a href='/src/Konsole.Tests/ProgressCharBarTests/RefreshShould.cs#L38-L54' title='Snippet source file'>snippet source</a> | <a href='#snippet-progresscharbar' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

# Threading and threadsafe writing to the Console.

If you have a background thread that writes to the screen, then you have to make sure that the thread code is threadsafe, with regards to the console. `System.Console` by default is not threadsafe. Use `new ConcurrentWriter()` to create a simple threadsafe writer that will write to the current console window. New Window is not threadsafe. Call `.Concurrent()` on a new window to return a thread safe window.

All the static constructors return threadsafe windows by default; So

#### Threadsafe static constructors

- `Window.Open`
- `Window.OpenBox`
- `Window.OpenInline`
- `new ConcurrentWriter()`
- `new Window().Concurrent()`

#### new Window is not threadsafe

```csharp
var myWindow new Window(...);
```

You can make an existing window instance safe by either calling `.Concurrent()` on an instance, or by only using that window as a region that is then Split using one of the extension methods, `SplitTop`, `SplitBottom`, `SplitLeftRight` etc. Those are static extension methods, and like the static constructors, they all return threadsafe instances wrapped in a `new ConcurrentWriter()`.

#### Make it threadsafe

```csharp
// create an 80 by 20 inline window
var window = new Window(80, 20);

// split that window into boxes
var left = window.SplitLeft("left");
var right = window.SplitRight("right");

// right and left are threadsafe, window is not.

var safewin = window.Concurrent();

// safewin is threadsafe, window is still NOT threadsafe.

safewin.WriteLine(Green, "This is threadsafe");

```

## ConcurrentWriter

Provides a threadsafe way to write to the current console. You need to switch to writing to the console using a concurrent writer any time you have a background thread that is updating any portion of a console screen or a `Konsole` window. 

```csharp
var console = new ConcurrentWriter();

console.WriteLine(Green, $"finished encrypting {bytes} bytes.");
console.Write(...)
console.PrintAt(...)
```

You wrap any instance of any class that implements `IConsole` in a `ConcurrentWriter` Make any code of yours that implements `IConsole`  threadsafe when writing to the console.

```csharp
var myThreadSafeWriter = new ConcurrentWriter(myObjectThatImplementsIConsole);
```
# The Window object

## Window

```csharp
var window = new Window();
```

Create a windowed region of the console (with or without a boxed border and title) that you can write to as if it were a new Console. It will wrap text and scroll, and you can print to it using PrintAt, as well as nest child windows in windows for more advanced window layouts.

  - ( 100%-ish console compatible window, supporting all normal console writing to a windowed section of the screen) 
  - Supports scrolling and clipping of console output.
  - typical uses, for showing a scrolling output, e.g. build output in a window, while showing higher level progress in another window.
  - automatic borders
  - full color support

#### Floating constructors

When you provide a startX and startY position, as well as height and width, then the window created will be a `floating` window. The following are all floating constructors. The default foreground and background colors when none are provided are white on black.

#### Inline constructors

When you do not provide a startX and startY position, and only provide a height and width, then the window created is an `inline` window. The window is created starting at the next line using the height and width provided. The parent console `CursorTop` is advanced to the next line after the newly created window, and the cursorLeft is set to `0`.

- `public Window(int width, int height)`
- `public Window(int width, int height, ConsoleColor foreground, ConsoleColor background)`

#### fullscreen constructor

When no start position or height and width is provided, then the window is fullscreen. It fill the entire parent window, even if the cursor is halfway down the parent window at the time. The cursor position is reset to the parent window `0,0`.

* `Window()`

```csharp
var myConsoleAppMainWindow = new Window();
```

#### window example        

```csharp
var con = Window.OpenBox("client server demo", 110, 30);

con.WriteLine("starting client server demo");
var client = new Window(1, 4, 20, 20, ConsoleColor.Gray, ConsoleColor.DarkBlue, con).Concurrent();
var server = new Window(25, 4, 20, 20, con).Concurrent();
client.WriteLine("CLIENT");
client.WriteLine("------");
server.WriteLine("SERVER");
server.WriteLine("------");
client.WriteLine("<-- PUT some long text to show wrapping");
server.WriteLine(ConsoleColor.DarkYellow, "--> PUT some long text to show wrapping");
server.WriteLine(ConsoleColor.Red, "<-- 404|Not Found|some long text to show wrapping|");
client.WriteLine(ConsoleColor.Red, "--> 404|Not Found|some long text to show wrapping|");

con.WriteLine("starting names demo");
// let's open a window with a box around it by using Window.Open
var names = Window.OpenBox("names", 50, 4, 40, 10);
TestData.MakeNames(40).OrderByDescending(n => n).ToList()
        .ForEach(n => names.WriteLine(n));

con.WriteLine("starting numbers demo");
var numbers = Window.OpenBox("{numbers", 50, 15, 40, 10, new BoxStyle() { ThickNess = LineThickNess.Double, Body = new Colors(White, Blue) });
Enumerable.Range(1, 200).ToList()
        .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling

Console.ReadKey(true);
```
gives you

![window simple demo](docs/img/05-window-demo.png)

# Static constructors

## OpenBox

- `Window.OpenBox(string title)`
- `Window.OpenBox(string title, BoxStyle style)`
- `Window.OpenBox(string title, int sx, int sy, int width, int height)`
- `Window.OpenBox(string title, int width, int height, BoxStyle style = null)`
- `Window.OpenBox(string title, int sx, int sy, int width, int height, BoxStyle style)`

Open a full screen styled window with a lined box border with a title. Styling allows for setting foreground and background color of the Line, Title, and body, as well as the line thickness, single or double using default styling, white on black, single thickness line. 

```csharp

  [Test]
        public void WhenNested_draw_a_box_around_the_scrollable_window_with_a_centered_title_and_return_a_live_window_at_the_correct_screen_location()
        {
            var con = new MockConsole(20, 9);
            Window.HostConsole = con;
            var parent = Window.OpenBox("parent", 0, 0, 20, 8, new BoxStyle() { ThickNess = LineThickNess.Double });
            var child = parent.OpenBox("c1", 7, 2, 8, 4);
            parent.WindowWidth.Should().Be(18);
            parent.WindowHeight.Should().Be(6);
            //var child = parent.OpenBox("c1", 7, 2, 8, 4);

            parent.WriteLine("line1");
            parent.WriteLine("line2");

            var expected = new[]
            {
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║       ┌─ c1 ─┐   ║",
                        "║       │      │   ║",
                        "║       │      │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);

            child.WriteLine("cats");
            child.Write("dogs");
            
            expected = new[]
            {
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║       ┌─ c1 ─┐   ║",
                        "║       │cats  │   ║",
                        "║       │dogs  │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);

            // should not interfere with original window cursor position so should still be able to continue writing as 
            // if no new child window had been created.

            parent.WriteLine("line3");
            parent.WriteLine("line4");

            expected = new[]
{
                        "╔═════ parent ═════╗",
                        "║line1             ║",
                        "║line2             ║",
                        "║line3  ┌─ c1 ─┐   ║",
                        "║line4  │cats  │   ║",
                        "║       │dogs  │   ║",
                        "║       └──────┘   ║",
                        "╚══════════════════╝",
                        "                    "
            };

            con.Buffer.Should().BeEquivalentTo(expected);
        }
```

## Open

`Window.Open()`

Calling `Window.Open()` without any parameters will create a new window region consisting of the whole screen, and will clear the screen, and reset the cursor position. Returns a threadsafe `Concurrent` window.

```csharp
var win = Window.Open();
```
this is equivalent to 
```
var win = new Window().Concurrent();
```

# Methods and Extension Methods

These methods require an existing instance of a window. (`IConsole`)

## PrintAt

- `PrintAt(int x, int y, char c)`
- `PrintAt(x, y, text)`
- `PrintAt(int x, int y, string format, params object[] args)`

PrintAt an area of a window

```csharp
 var window = new Window();
 ...
 window.PrintAt(20, 20, "WARNING!");
```

## PrintAtColor

- `PrintAtColor(ConsoleColor foreground, int x, int y, string text, ConsoleColor? background = null)`

PrintAt an area of a window providing the color.

```csharp
 var window = new Window();
 ...
 window.PrintAtColor(Red, 20, 20, "WARNING!", White);
```

Print the text, optionally wrapping and causing any scrolling in the current window, at cursor position X,Y in foreground and background color without impacting the current window's cursor position or colours. This method is only threadsafe if you have created a window by using .ToConcurrent() after creating a new Window(), or the window was created using Window.Open(...) which returns a threadsafe window.

## Write

Write the text to the window in the color, withouting resetting the window's current foreground colour.  Optionally causes text to wrap, and if text moves beyond the end of the window causes the window to scroll. The cursor of the window that did the writing remains at the last printed position, and no other window's cursor positions are changed.

- `Write(string text)`
- `Write(string format, params object[] args)`
- `Write(ConsoleColor color, string format, params object[] args)`

```csharp
 var window = new Window();
 window.Write(Red, "WARNING!");
 window.Write("this text is in the default colour and is not red.");
```

## WriteLine

Same as `Write` but simulates a carriage return by moving the `CursorTop` to next line and resetting `CursorLeft` to `0`.

- `WriteLine(string format, params object[] args)`
- `WriteLine(ConsoleColor color, string format, params object[] args)`

## SplitRows

Split a console window screen into rows of screens. Returns an array of the rows. Provide either a string title for a boxed row, or a `Split`. Optionally Specify the height for each split. Use a height of `0` to indicate that row will take the remainder of the rows. Similar to `*` in CSS.

**Evenly split rows**
<!-- snippet: splitrows -->
<a id='snippet-splitrows'></a>
```cs
var rows = console.SplitRows("top","middle", "bottom");
rows[0].Write("one");
rows[1].Write("two");
rows[2].Write("three");

console.Buffer.Should().BeEquivalentTo(new[] {
    "┌── top ─┐",
    "│one     │",
    "└────────┘",
    "┌ middle ┐",
    "│two     │",
    "└────────┘",
    "┌ bottom ┐",
    "│three   │",
    "└────────┘"
});
```

<sup><a href='/src/Konsole.Tests/WindowTests/SplitWithBorderTests/splitRowsTests.cs#L51-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-splitrows' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

**Wildcards in `SplitRows`, `SplitColumns`**

SplitRows, SplitColumns now supports multiple wildcards per split layout, for example 

```csharp
    // assuming width of 60
    var columns = console.SplitColumns(
        new Split(10, "left"),      // Fixed width of 10.
        new Split("wild1"),         // Wildcard : no width specified, this column will be 15.
        new Split(10, "middle"),    // Fixed width of 10.
        new Split("wild2"),         // Wildcard, will be 15 characters wide.
        new Split(10,"right")       // Fixed width of 10.
    );
```

> Wildcard column size above is 15 because -> (balance of 60 - [10 x 3] = 30) split evenly between all the wildcards

**Shortcut `SplitColumns` notation**

Shortcut notation of supplying only the caption for each split row or column will all have equal sizes.

```csharp
    var actors = Window.OpenBox("London", width:60, height:20);
    var columns = box.SplitColumns("a-grade", "b-grade", "extras"); // Creates 3 columns 20 chars wide each.
```


## Copying code from the unit tests

Because the unit tests are run on an Azure build server that does not have access a open console fileHandle many of the tests use `MockConsole`. Whenever you see sample code, eg the test below from [src/Konsole.Tests/WindowTests/SplitColumnsShould.cs](src/Konsole.Tests/WindowTests/SplitColumnsShould.cs) 

```csharp
    var con = new MockConsole(30, 4);
    var window = new Window(con);
    var consoles = window.SplitColumns(
            new Split(8, "col1", LineThickNess.Single),
            new Split(10, "col2", LineThickNess.Single),
            new Split(12, "col3", LineThickNess.Single)
```

... then in order to use the code yourself in a project, simply leave out the MockConsole, and start with a `new Window()` as below. 
The rest of the unit test code will work the same in production as in testing.

```csharp
    var window = new Window();
    var consoles = window.SplitColumns(
            new Split(8, "col1", LineThickNess.Single),
            new Split(10, "col2", LineThickNess.Single),
            new Split(12, "col3", LineThickNess.Single)
            ... rest of code
```
 
 # Other .NET console libraries


- [tonerdo/readline](https://github.com/tonerdo/readline) : A Pure C# GNU-Readline like library for .NET/.NET Core. (works well with Konsole)
- ncurses (tbd))
- [migueldeicaza/Gui.cs](https://github.com/migueldeicaza/gui.cs) : For building Full console applications (APPS) like a windows app, but using the console and supports mouse. THis *is* fully Windows, Linux, Unix compatible. 
- [DragonFruit - as described by Scott Hanselman] (https://www.hanselman.com/blog/DragonFruitAndSystemCommandLineIsANewWayToThinkAboutNETConsoleApps.aspx) : Strongly typed `void main(int x, string something, bool yesOrNo)` <-- this is madness on a stick...so great!
- [replaysMike/AnyConsole](https://github.com/replaysMike/AnyConsole) : Great for writing utilities for full screen browsing of logs or files where you will be scrolling through large sections of text.
- [fclp/FluentCommandLineParser](https://github.com/fclp/fluent-command-line-parser) : Does what it says on the tin.

#### unsorted list of libraries (random links I saved for myself to refer to or investigate)

- fast io stack question  : https://stackoverflow.com/questions/2754518/how-can-i-write-fast-colored-output-to-console
- low level io, msdn docs : https://docs.microsoft.com/en-us/windows/console/low-level-console-output-functions
- official msdn docs      : https://docs.microsoft.com/en-us/windows/console/writeconsoleoutput
- need to detect platform : https://mariusschulz.com/blog/detecting-the-operating-system-in-net-core
- fast strings            : https://www.stevejgordon.co.uk/creating-strings-with-no-allocation-overhead-using-string-create-csharp
- simple text buffer      : http://cgp.wikidot.com/consle-screen-buffer
- other console libraries   : https://github.com/Athari/CsConsoleFormat to render an alternative Console emulator?
- another                 : http://elw00d.github.io/consoleframework/examples.html
- TBD                     : https://blog.tedd.no/2015/08/02/better-text-console-for-c/
- Windows Console Game    : http://cecilsunkure.blogspot.com/2011/11/windows-console-game-writing-to-console.html
- launching more consoles : https://neowin.net/forum/topic/904788-c-adding-a-console-window-in-a-windows-app/

# Why did I write Konsole?

I wrote Konsole to allow me to write simple test projects, or "reference architecture" projects when evaulating various libraries, (for example, `Akka.net`, `memstate` or `eventstor` ) It allows me to write simple console apps that are easy to understand and render in a visually simple way, especially when there are multiple threads, actors or servers that I need to visual represent without getting sidetracked building a WPF or windforms or web application. 

A big benefit to me is being able to visually describe in text any complex screen layout and application without requiring images. 

I'm now also using it for other serious applications besides learning material. I'm using `Konsole` in `Gunner` a `.NET` testing library similar to `Gattling` that I need to put code under stress when evaulating different enterprise messaging libraries.

# Debugging problems with Konsole

## This project may not be fully compatible with your project.

When building .NET standard or .NET core app, you may recieve the following warning. If you have `treat warnings as errors` turned on, then this will fail your build.

## Warning NU1702

> Warning	NU1702	ProjectReference 'C:\src\git-alan-public\konsole-spike\Konsole.Platform.Windows\Konsole.Platform.Windows.csproj' was resolved using '.NETFramework,Version=v4.6.1' instead of the project target framework '.NETStandard,Version=v2.0'. This project may not be fully compatible with your project.	Konsole	C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\Microsoft.Common.CurrentVersion.targets		1653

This is not a problem for Konsole and is a bug in .NET where .NET standard 2.0 apps should be able to reference up to .NET framework 4.6.1 without errors. To suppress this error, add the following to your project file.

```xml
    <MSBuildWarningsAsMessages>NU1702</MSBuildWarningsAsMessages>  
```

## No visible output, blank screen

If you are using the `HighSpeedWriter` you must call `Flush()` to render the output.

<img src='docs/flush.PNG' width='400'/>

## Corrupt output

#### colors or text output appearing in the wrong place.

possible causes and fixes

- some code somewhere is writing directly to the `System.Console`. Replace that code with calls to a `new ConcurrentWriter()`. See [Threading docs](docs/theading.md) for more information. 

# Writing Tests for your code that uses Konsole

## Tests for code using HighSpeedWriter

TBD

## All other tests

TBD

# MockConsole

## MockConsole (substitute), and IConsole (interface) usage

Use MockConsole as a real (fully functional) `System.Console` substitute, that renders with 100% fidelity to an internal buffer that can be used to assert correct console behavior of any object writing using IConsole.
MockConsole can even render out a text representation of the current state of the the console, including representations for the foreground and background color of anything written.

All the test for this library have been written using `MockConsole.` For a fully detailed examples of `MockConsole` being stretched to the limits, see `Konsole.Tests.WindowTests`.

```csharp
        
        using Konsole;
        ...
        public class Cat
        {
            private readonly IConsole _console;
            public Cat(IConsole console) { _console = console; }
            public void Greet()
            {
                _console.WriteLine("Prrr!");
                _console.WriteLine("Meow!");
            }
        }

        [Test]
        public void TestConsole_ConsoleWriter_and_IConsole_example_usage()
        {
            {
                // test the cat
                // ============
                var console = new TestConsole(80, 20);
                var cat = new Cat(console);
                cat.Greet();
                Assert.AreEqual(console.BufferWrittenTrimmed, new[] {"Prrr!", "Meow!"});
            }
            {
                // create an instance of a cat that will purr to the real Console
                // ==============================================================
                var cat = new Cat(new ConsoleWriter());
                cat.Greet(); // prints Prrr! aand Meow! to the console
            }
        }
```


## `MockConsole` vs `Mock<IConsole>`

Below is a comparison of how someone might test an Invoice class using a traditional `Mock<IConsole>` and the same test, using a `Konsole.MockConsole`. To make it a fair comparison I'm comparing to [NSubstitute](http://nsubstitute.github.io/) which is quite terse and one of my favourite mocking frameworks.

```csharp

        [Test]
        public void Test_Invoice_using_mocks()
        {
            // test the invoice
            // ============
            IConsole console = new Substitute.For<IConsole>();
            var invoice = new Invoice(console);
            invoice.AddLine(2, "Semi Skimmed Milk", "2 pints", "£",1.00);
            invoice.AddLine(3, "Warburtons Crumpets", "6 pack", "£",0.89);
            invoice.Print();
                
            // not really practical to test printed output using a mock console
            // ================================================================
            console.Received().SetCursorPosition(0,0);
            console.Received().WriteLine("ACME Wholesale Foody");
            console.Received().WriteLine("--------------------");
            console.Received().WriteLine("");
            console.Received().WriteLine("--------------------");
            console.Received().Write("qty ");
            console.Received().Write(2);
            console.Received().Write(" Semi Skimmed Milk");
            console.Received().Write(", ");
            console.Received().Write("{0} pints", 2);
            console.Received().Write("£ {0.00,-10}", 2.0m);
            .
            .
            . // and so on and so on ...for probably around another 12 or 13 lines.
            .
            .
             // having to mimick the exact formatted Write's and Writelines and SetCursor movements 
             // this is brittle, if the code is optimised to replace two Write's with a single formatted WriteLine for example
             // then this test fails even though the desired output is written to the console.
        }
        
```

using a Test Double like `Konsole.MockConsole` the test above becomes

```csharp
        [Test]
        public void testing_Invoice_class_using_MockConsole()
            {
                var expected = @"
                 ACME WHoleSale Foody 
                 -------------------- 
                 qty 2 Semi Skimmed Milk   , 2 pints     £ 2.00
                 qty 3 Warburtons Crumpets , 6 pack      £ 5.34
                 --------------
                 total   £ 7.34 
                 --------------
            
                * some random message on the footer
";
        
                var console = new MockConsole();
                var invoice = new Invoice(console);
                invoice.AddLine(2, "Semi Skimmed Milk", "2 pints", "£",1.00);
                invoice.AddLine(3, "Warburtons Crumpets", "6 pack", "£",0.89);
                invoice.Print();
                Assert.AreEqual(console.BufferString,expected);
                });
            }
                
            // Now, if someone accidentally changes your currency formatter, this test will wail
            // when the rendered output to the Console suddenly changes, bwaaam! Instant Fail.
            // Score one for MockConsole, sweetness, life is good!
        }


``` 

# Cross platform notes

TBD

# Building the solution

```shell
git clone https://github.com/goblinfactory/konsole.git
cd konsole
dotnet test `or` build.bat
```

### requirements

Any version of .net core. Update `global.json` to the version of .net core you have installed and run `build.bat`

# Dev releases

- dev branch will be available as `pre-releases` from time to time. To access the latest dev branch, use Nuget to include pre-releases. latest dev branch will be suffixed with `dev-{nn}` e.g. `6.0.1-dev-01`

# Alpha releases

- Alpha releases will be suffixed with `-alpha-{nn}`
- Alpha releases are any release that I want to make available for user feedback. Most commonly this will effectively be a private release for a user that has requested a specific feature, or reported a bug and I create a candidate release to fix that bug, or add that feature. 
- Alpha releases may contain public API api changes and would mean to include the fix it would mean that feature would be included in the next major release.  



# ChangeLog

* here is the [changelog](change-log.md). It is kept up to date.

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## Support, Feedback, Suggestions

[If you find Konsole useful please consider sponsoring me.](https://github.com/sponsors/goblinfactory)
 
keep chillin!

    O__
    _/`.\
        `=( 

Alan

[p.s. join us at snowcode free dev conference at ski resort](http://www.snowcode.com?refer=konsole) <br/>
[www.snowcode.com](http://www.snowcode.com?refer=konsole) <br/>
developers + party + snow + great learning
February (private invite only powderfest) and April (open to public) end of season snowcode conf, slushfest, best for boarders! :D will try to be at a glacier for the skiers.

[@snowcode](https://twitter.com/snowcode)

![Alan Hemmings](docs/img/12-alan.png)
