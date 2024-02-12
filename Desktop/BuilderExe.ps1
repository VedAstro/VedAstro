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

# Build the .NET MAUI App
dotnet publish -c Release -f net8.0-windows10.0.19041.0 --no-self-contained

Write-Host "**************"
Write-Host "Creating Inno Setup..."
Write-Host "**************"

# Run the Inno Setup script
& 'C:\Program Files (x86)\Inno Setup 6\ISCC.exe' .\InnoSetupMakerScript.iss

# Wait for user input before closing the window
Write-Host "Done. Press Enter to exit."
Read-Host
