write-host "Build a local evalation binary and copy it to my personal (Alan) test nuget folder"
write-host "----------------------------------------------------------------------------------"

dotnet build src/Konsole/Konsole.csproj --configuration Release
dotnet build src/Konsole.Tests/Konsole.Tests.csproj --configuration Release
dotnet test src/Konsole.Tests/Konsole.Tests.csproj

$file =  gci src\Konsole\bin\Release  | select -last 1
# change this directory to point to your offline NUGET cache, directly must exist, manually create if you need to.
$cache = 'C:\src\nuget\test-packages\Goblinfactory.Konsole'
 copy-item $file.fullname -destination $cache
 Write-host "$file copied to nuget cache $cache"