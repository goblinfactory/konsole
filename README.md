# Konsole
home of ProgressBar ( C# console progress bar with support for single or multithreaded progress updates ) and MockConsole ( The only 100%-ish* System.Console compatible console mock, supporting color printing as well as .CursorTop and .CursorLeft.

![install-package Goblinfactory.Konsole](install-package.png)

##ProgressBar usage
```csharp

    using Goblinfactory.Konsole;

           . . .

            var pb = new ProgressBar(50);
            pb.Refresh(0, "connecting to server to download 50 files sychronously.");
            Console.ReadLine();

            pb.Refresh(25, "downloading file number 25");
            Console.ReadLine();
            pb.Refresh(50, "finished.");
```

![sample output](progressbar.gif)
[sample parallel ProgressBar code that produced the output above](readme-sample-parallel.md)

##IConsole, MockConsole and ConsoleWriter usage

````csharp
        
        using Goblinfactory.Konsole;
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
        public void MockConsole_ConsoleWriter_and_IConsole_example_usage()
        {
            {
                // test the cat
                // ============
                var console = new MockConsole(80, 20);
                var cat = new Cat(console);
                cat.Greet();
                Assert.AreEqual(console.TrimmedLines, new[] {"Prrr!", "Meow!"});
            }
            {
                // create an instance of a cat that will purr to the real Console
                // ==============================================================
                var cat = new Cat(new ConsoleWriter());
                cat.Greet(); // prints Prrr! aand Meow! to the console
            }
        }
```

<sub>* By 100%-ish I mean 'some', enough to make MockConsole useful enough and accurate enough that I couldn't have written a high quality progress-bar without it ;-D If you use MockConsole to help you write a console utility and find it's lacking some important features, please contact me, I'd love to hear from you and see if I can update MockConsole to help you.</sub>

##Support, Feedback, Suggestions

[@snowcode](https://twitter.com/snowcode)

![Alan Hemmings](https://pbs.twimg.com/profile_images/624901555532095488/j5dynw0i_bigger.png)