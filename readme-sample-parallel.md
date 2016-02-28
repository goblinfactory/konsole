
##ProgressBar example : showing status update for parallel tasks

This example creates 10 seperate console progress bars, each being updated on a seperate thread. (This code generates the output visible in the animated gif.)

```csharp

            // demo; take the first 10 directories that have files from c:\windows, and then pretends to process (list) them.
            // processing of each directory happens on a different thread, to simulate multiple background tasks,
            // e.g. file downloading.
            // ==============================================================================================================
            var dirs = Directory.GetDirectories(@"c:\windows").Where(d=> Directory.GetFiles(d).Count()>0).Take(10);

            var tasks = new List<Task>();
            var bars = new List<ProgressBar>();
            foreach (var d in dirs)
            {
                var dir = new DirectoryInfo(d);
                var files = dir.GetFiles().Take(100).Select(f=>f.FullName).ToArray();
                if (files.Count()==0) continue;
                var bar = new ProgressBar(files.Count());
                bars.Add(bar);
                bar.Refresh(0, d);
                tasks.Add(new Task(() => ProcessFiles(d, files, bar)));
            }
            Console.WriteLine("ready press enter.");
            Console.ReadLine();

            foreach (var t in tasks) t.Start();
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("done.");
            Console.ReadLine();

        }

        public static void ProcessFiles(string directory, string[] files, ProgressBar bar)
        {
            var cnt = files.Count();
            foreach (var file in files)
            {
                bar.Next(new FileInfo(file).Name);
                Thread.Sleep(150);
            }
        }


```

produces the following output

![](progressbar.gif)

