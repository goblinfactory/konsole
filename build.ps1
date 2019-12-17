write-host "Konsole"
write-host "-------"

# when running on linux we cannot build ALL the projects in the sln so have to specify them 1 by 1.
# so that can limit what frameworks we are testing on.

dotnet build src/Konsole/Konsole.csproj --configuration Release
dotnet build src/Konsole.Tests/Konsole.Tests.csproj --configuration Release
dotnet test src/Konsole.Tests/Konsole.Tests.csproj

write-host "------------------------"
write-host "Konsole.Platform.Windows"
write-host "------------------------"

# we can short circuit and build ALL the projects in the sln         
dotnet build src/Konsole.sln --configuration Release
dotnet test src/Konsole.Tests/Konsole.Tests.csproj

