# Konsole
home of ProgressBar ( C# console progress bar with support for single or multithreaded progress updates ) and MockConsole ( The only 100% System.Console compatible console mock, supporting color printing as well as .CursorTop and .CursorLeft.

### `Install-Package Goblinfactory.Konsole` 

(Apologies not yet available, busy atmo! Just moved goblinfactory/progress-bar to here today, 28.02.16, and am working on new nuget package, hope to have it up tonight.)

##ProgressBar usage
```csharp

    using Goblinfactory.ProgressBar;

           . . .

            var pb = new ProgressBar(50);
            pb.Refresh(0, "connecting to server to download 50 files sychronously.");
            Console.ReadLine();

            pb.Refresh(25, "downloading file number 25");
            Console.ReadLine();
            pb.Refresh(50, "finished.");
```

![sample output](progressbar.gif)
![sample ProgressBar code that produced the output above](readme-sample-parallel.md)
