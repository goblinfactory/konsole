"Hello world, building first nuget package"
$dir= '.\Konsole\'
Push-Location
cd $dir
nuget pack .\Konsole.csproj -Prop Configuration=Release
Pop-Location