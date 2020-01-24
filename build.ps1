$ErrorActionPreference = "Stop"

function Invoke {
    param (
        [scriptblock]$ScriptBlock
    )
    & @ScriptBlock
    if ($lastexitcode -ne 0) {
        Write-Host "------------------------" -foregroundcolor Red -backgroundcolor white
        Write-Host "BUILD FAILED WITH ERROR!" -foregroundcolor Red -backgroundcolor white
        Write-Host "------------------------" -foregroundcolor Red -backgroundcolor white
        exit $lastexitcode
    }
}


write-host "Konsole"
write-host "-------"

# dotnet build src/Konsole/Konsole.csproj --configuration Release
# dotnet build src/Konsole.Tests/Konsole.Tests.csproj --configuration Release
# dotnet test src/Konsole.Tests/Konsole.Tests.csproj

# when running on windows we can short circuit and build ALL the projects in the sln


invoke { dotnet build src/Konsole.sln --configuration Release } 
invoke { dotnet test src/Konsole.Tests/Konsole.Tests.csproj }
invoke { dotnet test src/Konsole.Tests.Slow/Konsole.Tests.Slow.csproj }

Write-Host "---------------"
Write-Host "BUild completed"
Write-Host "---------------"