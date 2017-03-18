"Hello world, building first nuget package"
$dir= '.\Konsole\'
Push-Location
cd $dir
..\packages\NuGet.CommandLine.3.5.0\tools\nuget pack .\Konsole.csproj -Prop Configuration=Release
Pop-Location