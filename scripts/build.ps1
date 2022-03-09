# Script place Release files to file "build.zip"
# If you have powershell launch restrictions, run this command in shell:
#			
#	Set-ExecutionPolicy RemoteSigned -Scope CurrentUser
$scriptDir = Get-Location
$baseDir = Split-Path -Path $scriptDir -Parent

#base dir with MsBuild should be specified
$env:MSBUILD_DIR = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\"

# BUILD RELEASE
cd $env:MSBUILD_DIR
.\MSBuild.exe "$baseDir\TabsHolder\TabsHolder.csproj" /t:Build /p:Configuration=Release /p:TargetFrameworkVersion=v4.5

# MAKE ZIP
if (Test-Path "$baseDir\build.zip") {
    rm "$baseDir\build.zip"
}

if (!(Test-Path "$baseDir\build")) {
    mkdir "$baseDir\build"
}

$files_to_archive = Get-Content "$scriptDir\build_files.txt"
foreach ($f in $files_to_archive)
{
    Copy-Item -Path "$baseDir\TabsHolder\bin\Release\$f" -Destination "$baseDir\build\"
}

Compress-Archive "$baseDir\build\*"  "$baseDir\TabsHolder.zip"
rmdir "$baseDir\build"