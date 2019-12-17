write-host "Konsole Tests"
write-host "-------------"

dotnet build src/Konsole/Konsole.csproj --configuration Release
dotnet build src/Konsole.Tests/Konsole.Tests.csproj --configuration Release
dotnet test src/Konsole.Tests/Konsole.Tests.csproj
