# PowerShell Script to Build .NET MAUI App

# let user know what is going on
Write-Host "**************"
Write-Host "This will make .net MAUI files for distribution"
Write-Host "version number needs to be set in InnoSetupMakerScript.iss"
Write-Host "**************"

# ask user to press enter to start
Write-Host "Press Enter to start..."
$null = Read-Host

# Navigate to the root directory of the script
$rootDirectory = Split-Path -Parent $MyInvocation.MyCommand.Definition
cd $rootDirectory

# Build the .NET MAUI App for a specific RID to make output path predictable
Write-Host "Building .NET MAUI App for Windows (win-x64)..."
dotnet publish -c Release -f net8.0-windows10.0.19041.0 -r win-x64 --no-self-contained

# Define paths
$mauiPublishDir = Join-Path $rootDirectory "bin\Release\net8.0-windows10.0.19041.0\win-x64\publish"
$innoSetupSourceDir = Join-Path $rootDirectory "publish"

# Prepare the target directory for Inno Setup
Write-Host "Preparing files for Inno Setup installer..."
if (Test-Path $innoSetupSourceDir) {
    Write-Host "Removing existing Inno Setup source directory: $innoSetupSourceDir"
    Remove-Item -Recurse -Force $innoSetupSourceDir
}
Write-Host "Creating Inno Setup source directory: $innoSetupSourceDir"
New-Item -ItemType Directory -Force -Path $innoSetupSourceDir

# Copy published files to the directory Inno Setup expects
Write-Host "Copying MAUI app files from $mauiPublishDir to $innoSetupSourceDir"
Copy-Item -Path (Join-Path $mauiPublishDir "*") -Destination $innoSetupSourceDir -Recurse -Force

Write-Host "**************"
Write-Host "Creating Inno Setup..."
Write-Host "**************"

# Run the Inno Setup script
& 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe' .\InnoSetupMakerScript.iss

# Wait for user input before closing the window
Write-Host "Done. Press Enter to exit."
Read-Host
