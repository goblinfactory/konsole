var target = Argument("target", "Default");
var tag = Argument("tag", "cake");

Task("Restore")
  .Does(() =>
{
    DotNetCoreRestore(".");
});

Task("Build")
  .Does(() =>
{
    DotNetCoreBuild(".");
});

Task("Test")
  .Does(() =>
{
    var files = GetFiles("tests/**/*.csproj");
    foreach(var file in files)
    {
        DotNetCoreTest(file.ToString());
    }
});

Task("Pack")
    .Does(() => 
{
    DotNetCorePack("Konsole/Konsole.csproj");
});

Task("Default")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Pack");

 Task("RunTests")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test");


RunTarget(target);