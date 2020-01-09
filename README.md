# Konsole

[![nuget](https://img.shields.io/nuget/dt/Goblinfactory.Konsole.svg)](https://www.nuget.org/packages/Goblinfactory.Konsole/) [![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0) [![Join the chat at https://gitter.im/goblinfactory-konsole/community](https://badges.gitter.im/goblinfactory-konsole/community.svg)](https://gitter.im/goblinfactory-konsole/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

# Konsole library

Low ceremony, simply to use C# (.NET standard) windowing console library, providing progress bars, windows and forms and drawing for console applications. Build UX's as shown below in very few lines of code.

**Konsole is a simple threadsafe way to write to the C# console window.** Write your own threadsafe wrapper at your peril. [See my notes on threading](docs/threading.md) :D

If you have any questions on how to use Konsole, please join us on Gitter (https://gitter.im/goblinfactory-konsole) and I'll be happy to help you. 

cheers, 

Alan

***cross-platform*** Konsole is 90% cross platform. `ProgressBar` and `Forms` and small inline `windows` that do not use text that causes scrolling all work in Nix, OSX and .NET standard code). **Konsole is a .NET Standard 2.2 project.**

![sample demo using HighSpeedWriter](docs/crazy-fast-screen.PNG)


### ProgressBar , Window  , Form , Menu , Draw & MockConsole

---



## Installing

![install-package Goblinfactory.Konsole](docs/install-package.png)

# ProgressBars

#### `ProgressBar`
```csharp
    using Konsole;
           
            var pb = new ProgressBar(PbStyle.DoubleLine, 50);
            pb.Refresh(0, "connecting to server to download 5 files asychronously.");
            Console.ReadLine();

            pb.Refresh(25, "downloading file number 25");
            Console.ReadLine();
            pb.Refresh(50, "finished.");
```
<p align='center'>
<img src='docs/progressbar.gif' align='center'/>
</p>

## ProgressBar worked parallel example
```csharp
    using Konsole;
           
    Console.WriteLine("ready press enter.");
    Console.ReadLine();

    var dirCnt = 15;
    var filesPerDir = 100;
    var r = new Random();
    var q = new ConcurrentQueue<string>();
    foreach (var name in TestData.MakeNames(2000)) q.Enqueue(name);
    var dirs = TestData.MakeObjectNames(dirCnt).Select(dir => new
    {
        name = dir,
        cnt = r.Next(filesPerDir)
    });

    var tasks = new List<Task>();
    var bars = new ConcurrentBag<ProgressBar>();
    foreach (var d in dirs)
    {
        var files = q.Dequeue(d.cnt).ToArray();
        if (files.Length == 0) continue;
        tasks.Add(new Task(() =>
        {
            var bar = new ProgressBar(files.Count());
            bars.Add(bar);
            bar.Refresh(0, d.name);
            ProcessFakeFiles(d.name, files, bar);
        }));
    }

    foreach (var t in tasks) t.Start();
    Task.WaitAll(tasks.ToArray());
    Console.WriteLine("done.");
```
![sample output](docs/progressbar2.gif)

# Threading (and `threadsafe` writing to the Console at last!) `.Concurrent()`

If you are writing a small command line utility that will be called from a build script, where you script does something, and uses threads to update the console the Konsole will make that a lot simpler.

## `ConcurrentWriter` and `Threading` with `.Concurrent()`

Use `new ConcurrentWriter()` to create a simple threadsafe writer that will write to the current console window. New Window is not threadsafe. Call `.Concurrent()` on a new window to return a thread safe window.

e.g. `new Window(...).Concurrent()`

All the static constructors return threadsafe windows by default. 

**THREADSAFE**

- `Window.Open`
- `Window.OpenBox`
- `Window.OpenInline`
- `new ConcurrentWriter()`
- `new Window().Concurrent()`

**NOT THREADSAFE** (make safe with `.Concurrent()`)

- `new Window(...)`

** [Full documentation here, with worked example for threading and `ConcurrentWriter`](docs/threading.md)



## Windows,  `Window.Open`, `new Window`, `Window.OpenBox`

  - ( 100%-ish console compatible window, supporting all normal console writing to a windowed section of the screen) 
  - Supports scrolling and clipping of console output.
  - typical uses, for showing a scrolling output, e.g. build output in a window, while showing higher level progress in another window.
  - automatic borders
  - full color support

```csharp
            var con = new Window(200,50);
            con.WriteLine("starting client server demo");
            var client = new Window(1, 4, 20, 20, ConsoleColor.Gray, ConsoleColor.DarkBlue, con);
            var server = new Window(25, 4, 20, 20, con);
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
            var names = Window.Open(50, 4, 40, 10, "names");
            TestData.MakeNames(40).OrderByDescending(n => n).ToList()
                 .ForEach(n => names.WriteLine(n));

            con.WriteLine("starting numbers demo");
            var numbers = Window.Open(50, 15, 40, 10, "numbers", 
                  LineThickNess.Double,ConsoleColor.White,ConsoleColor.Blue);
            Enumerable.Range(1,200).ToList()
                 .ForEach(i => numbers.WriteLine(i.ToString())); // shows scrolling
```
**gives you**

![window simple demo](docs/window-demo.png)

#### [Window.OpenBox](#window-openbox)

- `Window.OpenBox(string title, int sx, int sy, int width, int height, BoxStyle style)`
- `Window.OpenBox(string title, int sx, int sy, int width, int height)`
- `Window.OpenBox(string title, int width, int height, BoxStyle style = null)`
- `Window.OpenBox(string title, BoxStyle style)`
- `Window.OpenBox(string title)`

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

#### Simple Example - Mix and match `System.Console` with `Konsole`

<img src='docs/openbox-example.png' width='200' align='right'/>

```csharp
    void Wait() => Console.ReadKey(true);

    // show how you can mix and match Console with Konsole
    Console.WriteLine("line one");

    // create an inline Box window at the current cursor position
    // (returned Window implements IConsole) 
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

    // simple method that takes a window and prints a stock price to that window in color
    void Tick(IConsole con, string sym, decimal newPrice, ConsoleColor color, char sign, decimal perc) 
    {
        con.Write(White, $"{sym,-10}");
        con.WriteLine(color, $"{newPrice:0.00}");
        con.WriteLine(color, $"  ({sign}{newPrice}, {perc}%)");
        con.WriteLine("");
    }

```

#### Window.PrintAt()

- `window.PrintAt(x, y, text)`
- `window.PrintAtColor(foregroundColor, x, y, text, backgroundColor ?)`

PrintAt an area of a window, optionally providing the color.

```csharp
 var window = new Window();
 ...
 window.PrintAtColor(Red, 20, 20, "WARNING!");
```

Print the text, optionally wrapping and causing any scrolling in the current window, at cursor position X,Y in foreground and background color without impacting the current window's cursor position or colours. This method is only threadsafe if you have created a window by using .ToConcurrent() after creating a new Window(), or the window was created using Window.Open(...) which returns a threadsafe window.

**Maintaining seperate colors and cursor positions for windows so that other threads do not change the color or printing while another thread is writing to the console is a really big deal and is what makes Konsole a solid library to use when evaluating multi-threaded libraries** and need a simple way to monitor the results of various asynchronous operations without having to write multiple console apps, or create a Javascript UX library. 

## `window.Write(string format, params object[] args)`
## `window.Write(stringConsoleColor color, format, params object[] args)`

Write the text to the window in the {color} color, withouting resetting the window's current foreground colour.  Optionally causes text to wrap, and if text moves beyond the end of the window causes the window to scroll. The current cursor remains at the last printed position.

## `window.WriteLine(string format, params object[] args)`
## `window.WriteLine(ConsoleColor color, string format, params object[] args)`

Write the text to the window in the {color} color, withouting resetting the window's current foreground colour.  Optionally causes text to wrap, and if text moves beyond the end of the window causes the window to scroll. Moves the cursor to the next line after printing and `CursorLeft` is reset to 0.

## `Window.Open()`

*Open a window taking up the whole screen.*

```csharp
    var numbers = Window.Open();
```
this is equivalent to 
```
    var numbers = new Window();
```

## `Window.Open(sx, sy, ex, ey, title)`

```csharp
    var numbers = Window.Open(50, 15, 40, 10, "numbers");
```

## `Window.Open(sx, sy, ex, ey, title, LineThickNess, ForegroundColor, BackgroundColor)`

```csharp
    var numbers = Window.Open(50, 15, 40, 10, "numbers", 
        LineThickNess.Double,
        ConsoleColor.White,
        ConsoleColor.Blue
    );
```
gives you the blue window in the same code further up. (there is a known bug atmo, this constructor does not currently print the title. Will be fixed in upcoming version 6.x) As a workaround for now, If you need a title, use any of the Splits, `SplitTop`, and `SplitBottom`, or `SplitLeft`, and `SplitRight`. Or use `PrintAt` to print the title on top of the border. 

## Opening Inline Windows `new Window(height, width)` or `new Window(height)`

```
var fruit = new Window(3, 20);
or
var fruit = new Window(3);
```

Create a new window inline starting on the next line, at current `CursorTop + 1, using the specified width or the whole screen width if none is provided. Default color of White on Black. If you need to override the defaults then use the static constructor. `Window.OpenInline`

## `Window.OpenInline(echoConsole, padLeft, width, height, foregroundColor, backgroundColor)`

Create a new window inline starting on the next line, at current `CursorTop + 1`, using the specified `width` with `foreground` and `background` color. 

```csharp
   var fruit = Window.OpenInline(2, 50, 3, White, Blue);
```


# `SplitLeft()`, `SplitRight()`

Split an `IConsole` window into two equal halves, returning either the left or right half.

```csharp
    var w = new Window();

    // split left
    var left = w.SplitLeft("left");

    // split right
    var right = w.SplitRight("right");

    left.WriteLine("one");
    left.WriteLine("two");
    left.Write("three");

    right.WriteLine("four");
    right.WriteLine("five");
    right.Write("six");
```

gives you

```
    ┌ left ─┐┌─ right ┐
    │one    ││four    │
    │two    ││five    │
    │three  ││six     │
    └───────┘└────────┘
```

# `SplitLeftRight()`

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

# SplitTop, SplitBottom

```csharp
    var w = new Window();
    var top = w.SplitTop("top");
    var bottom = w.SplitBottom("bot");

    top.WriteLine("one");
    top.WriteLine("two");
    top.Write("three");

    bottom.WriteLine("four");
    bottom.WriteLine("five");
    bottom.Write("six");

```

gives you

```
    ┌── top ─┐
    │one     │
    │two     │
    │three   │
    └────────┘
    ┌── bot ─┐
    │four    │
    │five    │
    │six     │
    └────────┘
```

# Nested Windows - combining `SplitTop, SplitBottom` with `SplitLeft, SplitRight`

```csharp
    var win = new Window(30,10);

    var left = win.SplitLeft("left");
    var right = win.SplitRight("right");
    
    var top = left.SplitTop("top");
    var bottom = left.SplitBottom("bot");
    
    top.WriteLine("one");
    top.WriteLine("two");
    top.Write("three");

    bottom.WriteLine("four");
    bottom.WriteLine("five");
    bottom.Write("six");
```

Gives you the window shown below.

Note that `top` and `bottom` windows are only 2 lines high and therefore printing three lines has cause the windows to scroll the top item off the window.

```
┌─── left ────┐┌─── right ───┐
│┌─── top ───┐││             │
││two        │││             │
││three      │││             │
│└───────────┘││             │
│┌─── bot ───┐││             │
││five       │││             │
││six        │││             │
│└───────────┘││             │
└─────────────┘└─────────────┘
```

# Advanced windows with `SplitRows` and `SplitColumns`

You can create advanced window layouts using `SplitRows` and `SplitColumns` passing in a collection of Splits. Pass in a size of `0` to indicate that `row` or `column` window must contain the remainder of the window space. 



```csharp
            var c = new Window();
            var consoles = c.SplitRows(
                    new Split(4, "heading", LineThickNess.Single),
                    new Split(0),
                    new Split(4, "status", LineThickNess.Single)
            ); ; ;

            var headline = consoles[0];
            var status = consoles[2];

            var contents = consoles[1].SplitColumns(
                    new Split(20),
                    new Split(0, "content") { Foreground = ConsoleColor.White, Background = ConsoleColor.Cyan },
                    new Split(20)
            );
            var menu = contents[0];
            var content = contents[1];
            var sidebar = contents[2];

            headline.Write("my headline");
            content.WriteLine("content goes here");

            menu.WriteLine("Options A");
            menu.WriteLine("Options B");

            sidebar.WriteLine("20% off all items between 11am and midnight tomorrow!");

            status.Write("System offline!");
            Console.ReadLine();
```

Produces the following window. Each of the console(s) that you have a reference to can be written to like any normal console, and will scroll and clip correctly. You can create progress bar instances inside these windows like any console.

<img src='./docs/window-example.PNG' width='600' />

Configure the properties of each section of a window with the `Split` class.

```csharp
new Split(size) 
{
    title,
    lineThickNess, 
    foregroundColor,
    backgroundColor
};
```

# Handling Input

To capture input, create an Inline Window, e.g. `var myWindow = Window.Open(width, height, title)` and the cursor will be placed immediately UNDERNEATH the newly created window, and you can use and normal `Console.ReadLine()` reads, `Console.ReadLine()` will run at the current cursor.

Here's a worked example showing you how to read input using `Konsole`

```csharp

        static void Main(string[] args)
        {
            static void Compress(IConsole status, string file)
            {
                status.WriteLine($"compressing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, $"{file} (OK)");
            }

            static void Index(IConsole status, string file)
            {
                status.WriteLine($"indexing {file}");
                Thread.Sleep(new Random().Next(10000));
                status.WriteLine(Green, " finished.");
            }

            var console = new ConcurrentWriter();  // < -- NOTE THE ConcurrentWriter to replace Console

            // open two new windows inline at the current cursor position
            // cursor will move to below the new windows for easy ReadLine input

            var compressWindow = Window.OpenBox("compress", 50, 10);
            
            console.WriteLine("I am below compress");

            var encryptWindow = Window.OpenBox("encrypt", 50, 10);

            var tasks = new List<Task>();

            while (true)
            {
                console.Write("Enter name of file to process (quit) to exit:");
                var file = Console.ReadLine();
                if (file == "quit") break;
                tasks.Add(Task.Run(() => Compress(compressWindow, file)));
                tasks.Add(Task.Run(() => Index(encryptWindow, file)));
                console.WriteLine($"processing {file}");
            }

            console.WriteLine("waiting for background tasks");
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
        }
```

Running the code above gives you

<img src='./docs/image-input.png' width='600' />

# Draw

Draw lines and boxes single or double line width on the console window and intelligently merge lines that are drawn.

Start by creating a `Draw` instance, that needs an `IConsole`. for example

```
var window = new Window();
var draw = new Draw(window);
```

## `.Box(startX, startY, endX, endYT, title)`

Once you have a `Draw` instance you can draw lines or boxes.
```csharp
draw.Box(2, 2, 42, 8, "my test box", LineThickNess.Single);
```
gives you
```
 ┌───────────── my test box ─────────────┐  
 │                                       │ 
 │                                       │ 
 │                                       │ 
 │                                       │ 
 │                                       │ 
 └───────────────────────────────────────┘ 
```
adding `.Line(2,5, )

## boxes can overlap

Boxes can overlap and line chars are intelligently merged. This allows for creating very sophisticated designs.

The sample below uses `MockConsole` which is also part of the `Konsole` library.

```csharp
            var console = new MockConsole(12, 10);
            var line = new Draw(console, LineThickNess.Single, Merge);
            line.Box(0, 0, 8, 6, LineThickNess.Single);
            line.Box(3, 3, 11, 9, LineThickNess.Double);

            var expected = new[]
            {
               "┌───────┐   ",
               "│       │   ",
               "│       │   ",
               "│  ╔════╪══╗",
               "│  ║    │  ║",
               "│  ║    │  ║",
               "└──╫────┘  ║",
               "   ║       ║",
               "   ║       ║",
               "   ╚═══════╝"
            };
            console.Buffer.Should().BeEquivalentTo(expected);
```


## .Line(startX, StartY, endX, endY, LineThickNess)

```csharp

var window = new Window();


int height = 18;
int sy = 2;
int sx = 2;
int width = 60;
int ex = sx + width;
int ey = sy + height;
int col1 = 20;

  var draw = new Draw(console, LineThickNess.Double);
            draw
                .Box(sx, sy, ex, ey, "my test box")
                .Line(sx, sy + 2, ex, sy + 2)
                .Line(sx + col1, sy, sx + col1, sy + 2, LineThickNess.Single)
                .Line(sx + 35, ey - 4, ex - 5, ey - 4, LineThickNess.Double)
                .Line(sx + 35, ey - 2, ex - 5, ey - 2, LineThickNess.Double)
                .Line(sx + 35, ey - 4, sx + 35, ey - 2, LineThickNess.Single) 
                .Line(ex - 5, ey - 4, ex - 5, ey - 2, LineThickNess.Single); 

window.PrintAt(sx + 2, sy + 1, "DEMO INVOICE");                
```

gives you. (there is a small bug when switching from single to double line at a corner, as seen in the sample below, this will be fixed in upcoming version 6.)

```
╔═══════════════════╤═══ my test box ═══════════════════════╗
║ DEMO INVOICE      │                                       ║
╠═══════════════════╧═══════════════════════════════════════╣
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                                           ║
║                                  ╤═══════════════════╤    ║
║                                  │                   │    ║
║                                  ╧═══════════════════╧    ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝

```

# Forms

  - quickly and neatly render an object and it's properties in a window or to the console.
  - support multiple border styles.
  - Support C# objects or dynamic objects

Readonly forms are currently rendered. Below are examples showing auto rendering of simple objects.
(Currently only text fields, readonly, simple objects.)
On the backlog; add additional field types, complex objects, and editing. 

```csharp
        using Konsole.Form;
        ...
            var form1 = new Form(80,new ThickBoxStyle());
            var person = new Person()
            {
                FirstName = "Fred",
                LastName = "Astair",
                FieldWithLongerName = "22 apples",
                FavouriteMovie = "Night of the Day of the Dawn of the Son 
                of the Bride of the Return of the Revenge of the Terror 
                of the Attack of the Evil, Mutant, Hellbound, Flesh-Eating 
                Subhumanoid Zombified Living Dead, Part 2: In Shocking 2-D"
            };
            form1.Write(person);
```

![sample output](docs/Form-Person.png)


```csharp        

           // works with anonymous types
            new Form().Write(new {Height = "40px", Width = "200px"}, "Demo Box");
```
![sample output](docs/Form-DemoBox.png)

```csharp        

            // change the box style, and width
            new Form(40, new ThickBoxStyle()).Show(new { AddUser= "true", CloseAccount = "false", OpenAccount = "true"}, "Permissions");
```
![sample output](docs/Form-Permissions.png)

# `Goblinfactory.Konsole.Windows`

## HighSpeedWriter 

If you want to write a console game, or serious console application that you've tested and it's too slow using normal Konsole writing, and you are OK with the app only running on windows, then you can use `HighSpeedWriter`, which is available as an additional nuget package `Goblinfactory.Konsole.Windows` to write to the console window via windows `Kernal32` for really high speed. This package has a dependancy on `Goblinfactory.Konsole` so you can simply install this package to get all the features. (nuget will automatically install the dependant package as well for you.)

If you are only going to be updating small portions of the screen, then it is less CPU intensive to simply use `PrintAt` without a highspeed writer. 

The other trade off is you need to keep calling `.Flush()` on your writer to refresh the screen. For a game you could dedicate a timer thread to refresh on `tick` and allow you to control the refresh rate, and cpu usage. Higher refresh rates will use more cpu.

```dos
Install package `goblinfactory.konsole.windows`
```

`HighSpeedWriter` can write to a 120 x 60 console window at over 30 frames per second with minimal CPU overhead.

TBD - INSERT GIF SHOWING DEMO OF HIGH SPEED OUTPUT

## Getting started with `HighSpeedWriter`

You use `Konsole` in the same way as described in the docs further above, except that all output is buffered, and you need to call `Flush()` on the writer when you want to update the screen. 

If you have multiple threads writing to the Console, then instead of calling flush all the time, another option is to create a background thread that will `tick` over and refresh the screen x times a second. (depending on what framerate you require).

# End to end sample - `HighSpeedWriter`

below is code that should give you a clue as to how I'm using HighSpeedWriter for myself. This sample code produces the following screen and output.

![sample demo using HighSpeedWriter](docs/crazy-fast-demo.gif)

**Below is the source code that produced these screenshots** It is also available in the code in the single file demo project [src/TestPackage/Program.cs](src/TestPackage/Program.cs)

```csharp

using System;
using System.Threading;
using System.Threading.Tasks;
using Konsole;
using Konsole.Internal;
using static System.ConsoleColor;

namespace TestPackage
{
    class Program
    {
        static bool finished = false;
        static bool crazyFast = false;
        static Func<bool> rand = () => new Random().Next(100) > 49;
        static void Main(string[] args)
        {


            using var writer = new HighSpeedWriter();
            var window = new Window(writer);

            window.CursorVisible = false;
            
            var left = window.SplitLeft();
            var leftConsoles = left.SplitRows(
                new Split(0),
                new Split(10, "status"),
                new Split(10)
                );

            var status = leftConsoles[1];           
            status.BackgroundColor = Yellow;
            status.ForegroundColor = Red;
            status.Clear();

            var stocksCon = leftConsoles[0];            
            var menuCon = leftConsoles[2];
            var namesCon = window.SplitRight("numbers B");

            var r = new Random();
            int speed = 200;
            int i = 0;

            // print random names in random colors 
            // and demonstrate scrolling and wrapping at high speed
            var t1 = Task.Run(() => {
                var names = TestData.MakeNames(500);
                while (!finished)
                {
                    if (crazyFast)
                    {
                        while (crazyFast && !finished)
                        {
                            // fill a screen full before flushing
                            // this is super quick because writer 
                            // simply writes to a buffer and no actual
                            // slow IO has happened yet
                            for(int x = 0;x < 100; x++)
                            {
                                var color = (ConsoleColor)(r.Next(100) % 16);
                                namesCon.Write(color, $" {names[i++ % names.Length]} ");
                            }
                            // now lets flush this massive block of updates
                            writer.Flush();
                        }
                    }
                    else
                    {
                        var color = (ConsoleColor)(r.Next(100) % 16);
                        namesCon.Write(color, $" {names[i % names.Length]} ");
                        if (finished) break;
                        Thread.Sleep(r.Next(speed));
                        writer.Flush();
                    }
                }
            });


            // a window with more slower printing numbers
            var t2 = Task.Run(() => {
                while(!finished)
                {
                    namesCon.Write(Green, $"({i++}) ");
                    Thread.Sleep(speed * 10);
                    writer.Flush();
                }
            });

            var t3 = Task.Run(() =>
            {
                // stock ticker simulation
                var stocksNYSE = new[] { "BRK.A", "MSFT", "AMZN", "BTG.L", "AAPL", "LWRF.L", "GBLN1", "GBLN2" };
                var stocksFTSE = new[] { "SDR", "WPP", "ABF", "BP", "AVV", "AAL", "KGF", "MNDI", "NG","RM", "NXT","PSON" };
                var FTSE100Con = stocksCon.SplitLeft("FTSE 100");
                var NYSECon = stocksCon.SplitRight("NYSE");

                while (!finished)
                {
                    decimal move = (decimal)r.Next(100) / 10;
                    decimal newPrice = (50 + r.Next(100) + move);
                    decimal perc = ((decimal)r.Next(2000) / 100);
                    var increase = rand() ? true : false;
                    var sign = increase ? '+' : '-';
                    var changeColor = perc < 10 ? Cyan : increase ? Green : Red;
                    IConsole con;
                    string stock;
                    if (rand())
                    {
                        con = FTSE100Con;
                        stock = stocksFTSE[r.Next(stocksFTSE.Length)];
                    }
                    else
                    {
                        con = NYSECon;
                        stock = stocksNYSE[r.Next(stocksNYSE.Length)];
                    }
                    con.Write(White, $"{stock,-10}");
                    con.WriteLine(changeColor, $"{newPrice:0.00}");
                    con.WriteLine(changeColor, $"  ({sign}{newPrice}, {perc}%)");
                    con.WriteLine("");
                    Thread.Sleep(r.Next(5000));
                }
            });


            // create a menu inside the menu console window
            // the menu will write updates to the status console window

            var menu = new Menu(menuCon, "Progress Bars", ConsoleKey.Escape, 30,
                new MenuItem('s', "slow", () =>
                {
                    speed = 200;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:ss - ")}");
                    status.WriteLine(Green, $" SLOW ");
                    crazyFast = false;
                }),
                new MenuItem('f', "fast", () =>
                {
                    speed = 10;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:ss - ")}");
                    status.WriteLine(White, $" FAST ");
                    crazyFast = false;
                }),
                new MenuItem('c', "crazy fast", () =>
                {
                    speed = 1;
                    crazyFast = true;
                    status.Write(White, $" : {DateTime.Now.ToString("HH:mm:SS - ")}");
                    status.WriteLine(Red, $" CRAZY FAST ");
                })

            );

            status.WriteLine("press up and down to select a menu item, and enter or highlighted letter to select. Press escape to quit.");

            // menu writes to the console automatically,
            // but because we're using a buffered screen writer
            // we need to flush the UI after any menu action.
            menu.OnAfterMenuItem = _ => writer.Flush();

            menu.Run();
            // menu will block until user presses the exit key.

            finished = true;
            Task.WaitAll(t1, t2, t3);

            window.Clear();
            window.WriteLine("thank you for flying Konsole air.");
            writer.Flush();
        }
    }
}


```

# Using the tests as Documentation

Because the unit tests are run on an Azure build server that does not have access a open console fileHandle most of the tests use `MockConsole`. 
Whenever you see sample code, eg the test below from [src/Konsole.Tests/WindowTests/SplitColumnsShould.cs](src/Konsole.Tests/WindowTests/SplitColumnsShould.cs) 

```csharp
    var con = new MockConsole(30, 4);
    var window = new Window(con);
    var consoles = window.SplitColumns(
            new Split(8, "col1", LineThickNess.Single),
            new Split(10, "col2", LineThickNess.Single),
            new Split(12, "col3", LineThickNess.Single)
```

In order to use the code yourself in a project, simply leave out the MockConsole, and start with a `new Window()` as below. 
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

placeholder list for now, will expand on this shortly. (this is a placeholder starter list only, there are a lot of good console libraries. )

| nuget | |
| --- | --- |
| [tonerdo/readline](https://github.com/tonerdo/readline) |  A Pure C# GNU-Readline like library for .NET/.NET Core. (works well with Konsole)
| ncurses | Various links, tbd.
| [migueldeicaza/Gui.cs](https://github.com/migueldeicaza/gui.cs) | For building Full console applications (APPS) like a windows app, but using the console and supports mouse. THis *is* fully Windows, Linux, Unix compatible. 
| [DragonFruit - as described by Scott Hanselman](https://www.hanselman.com/blog/DragonFruitAndSystemCommandLineIsANewWayToThinkAboutNETConsoleApps.aspx) | Strongly typed `void main(int x, string something, bool yesOrNo)` <-- this is madness on a stick...so great!
| [replaysMike/AnyConsole](https://github.com/replaysMike/AnyConsole) | Great for writing utilities for full screen browsing of logs or files where you will be scrolling through large sections of text.
| [fclp/FluentCommandLineParser](https://github.com/fclp/fluent-command-line-parser) | Does what it says on the tin.
| --- | --- |

I still need to add a few links to various .NET console templates that allow you to take advantage of full asp.net .NET core stack, e.g. dependancy injection etc. Plus command line parsing! (don't re-invent the wheel) 

# Why did I write Konsole?

I wrote Konsole to allow me to write simple test projects, or "reference architecture" projects when evaulating various libraries, (for example, `Akka.net`, `memstate` or `eventstor` ) It allows me to write simple console apps that are easy to understand and render in a visually simple way, especially when there are multiple threads, actors or servers that I need to visual represent without getting sidetracked building a WPF or windforms or web application. 

A big benefit to me is being able to visually describe in text any complex screen layout and application without requiring images. 

I'm now also using it for other serious applications besides learning material. I'm using `Konsole` in `Gunner` a `.NET` testing library similar to `Gattling` that I need to put code under stress when evaulating different enterprise messaging libraries.

# Debugging problems with Konsole, random items

## This project may not be fully compatible with your project.

When building .NET standard or .NET core app, you may recieve the following warning. If you have `treat warnings as errors` turned on, then this will fail your build.

> Warning	NU1702	ProjectReference 'C:\src\git-alan-public\konsole-spike\Konsole.Platform.Windows\Konsole.Platform.Windows.csproj' was resolved using '.NETFramework,Version=v4.6.1' instead of the project target framework '.NETStandard,Version=v2.0'. This project may not be fully compatible with your project.	Konsole	C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\Microsoft.Common.CurrentVersion.targets		1653

This is not a problem for Konsole and is a bug in .NET where .NET standard 2.0 apps should be able to reference up to .NET framework 4.6.1 without errors. To suppress this error, add the following to your project file.

```xml
    <MSBuildWarningsAsMessages>NU1702</MSBuildWarningsAsMessages>  
```

## No visible output, blank screen

If you are using the `HighSpeedWriter` you must call `Flush()` to render the output.

<img src='docs/flush.PNG' width='400'/>

## Corrupt output - colours or text output appearing in the wrong place.

possible causes and fixes

- some code somewhere is writing directly to the `System.Console`. Replace that code with calls to a `new ConcurrentWriter()`. See [Threading docs](docs/theading.md) for more information. 

# Writing Tests for your code that uses Konsole

## Tests for code using HighSpeedWriter

TBD

## All other tests

TBD

# MockConsole

## MockConsole (substitute), and IConsole (interface) usage

Use MockConsole as a real (fully functional) System.Console substitute, that renders with 100% fidelity to an internal buffer that can be used to assert correct console behavior of any object writing using IConsole.
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

## Cross platform notes
ProgressBar has been manually tested with Mono on Mac in version 1.0. I don't currently have any automated testing in place for OSX (mono) and Linux. That's all on the backlog.
It's possible I might split out the ProgressBar into a seperate nuget package, since that appeared to work remarkably well cross platform, while `Window` makes calls to some `System.Console` methods that are not supported in Mono.

The scrolling support currently uses `Console.MoveBufferArea` which is not implemented on Mono. I will be working on a suitable alrternative to this on Linux and OSX. (on the backlog) Biggest challenge will be doing crossplatform testing, ...mmm, I predict I will be eating [Cake](http://cakebuild.net/docs/tutorials/getting-started) and containers in the very near future!

# Source code

## Building the solution


### using visual studio

 1. `git clone https://github.com/goblinfactory/konsole.git`
 2. run the following commands from the root folder;

### requirements

Any version of .net core. Update `global.json` to the version of .net core you have installed and run the command below in order.

 > dotnet restore

 > dotnet build
 
 > dotnet test
 


## ChangeLog

* [changelog](change-log.md)

The format is based on [Keep a Changelog](http://keepachangelog.com/) 
and this project adheres to [Semantic Versioning](http://semver.org/).

## Support, Feedback, Suggestions

Please drop me a tweet if you find Konsole useful. Latest updates to Konsole was written at Snowcode 2017.
keep chillin!

    O__
    _/`.\
        `=( 

Alan

[p.s. join us at snowcode 2020! ](http://www.snowcode.com?refer=konsole) <br/>
[www.snowcode.com](http://www.snowcode.com?refer=konsole) <br/>
(free dev conf at great ski resort)<br/>
developers + party + snow + great learning
April 2020 end of season snowcode conf, slushfest, best for boarders! :D will try to be at a glacier for the skiers.

[@snowcode](https://twitter.com/snowcode)

![Alan Hemmings](https://pbs.twimg.com/profile_images/624901555532095488/j5dynw0i_bigger.png)
