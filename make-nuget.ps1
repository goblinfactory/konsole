"Hello, building nuget package"
$dir= '.\Konsole\'
Push-Location
try
{
	cd $dir
	nuget pack .\Konsole.csproj -Prop Configuration=Release
	# todo 
	# extract version number from nuspec or properties?
	# copy package to my offline nuget package folder so that I can test it manually 
	# check the version of the bin manually before creating the package
	# ...so that I dont forget to do a release build before making the nuget package.
	# should move this to appVeyor.
}
finally
{
	Pop-Location
}

