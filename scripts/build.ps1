# Script place Release files to file "build.zip"
# If you have powershell launch restrictions, run this command in shell:
#			
#	Set-ExecutionPolicy RemoteSigned -Scope CurrentUser

$curDir = Get-Location
rm "..\build.zip"
mkdir "..\build"
$files_to_archive = Get-Content "$curDir\build_files.txt"
foreach ($f in $files_to_archive)
{
    Copy-Item -Path "..\TabsHolder\bin\Release\$f" -Destination "..\build"
}

Compress-Archive "..\build\*"  "..\build.zip"
rmdir "..\build"