# Konsole
home of ProgressBar ( C# console progress bar with support for single or multithreaded progress updates ) and MockConsole ( The only 100%-ish* System.Console compatible console mock, supporting color printing as well as .CursorTop and .CursorLeft.

![install-package Goblinfactory.Konsole](install-package.png)

(Apologies not yet available, busy atmo! Just moved goblinfactory/progress-bar to here today, 28.02.16, and am working on new nuget package, hope to have it up tonight.)

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

<sub>* By 100%-ish I mean 'some', enough to make MockConsole useful enough and accurate enough that I couldn't have written a high quality progress-bar without it ;-D If you use MockConsole to help you write a console utility and find it's lacking some important features, please contact me, I'd love to hear from you and see if I can update MockConsole to help you.</sub>