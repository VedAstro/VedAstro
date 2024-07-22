
# Function to download and extract a file
function Download{
    param (
        [string]$url,
        [string]$targetZipFolder
    )

    # Extract the file name from the URL
    $fileName = [System.IO.Path]::GetFileName($url)

    #place where zip will live for future update checks
    $oriZipFilePath = Join-Path -Path $targetZipFolder -ChildPath $fileName

    # Check if the zip file already exists
    if (-not (Test-Path -Path $oriZipFilePath)) {
        Write-Host "Downloading $fileName to $targetZipFolder..."

        # Download the file
        Invoke-WebRequest -Uri $url -OutFile $oriZipFilePath
    } else {
        Write-Host "$fileName already exists...."
    }

    # return location file was downloaded with file name
    return $oriZipFilePath

}

function ExpandTarGzFile($targetZipFile, $targetExtractFolder) {

    # Check if the file exists
    if (-not (Test-Path $targetZipFile)) {
        Write-Error "File not found: $targetZipFile"
        return
    }

    # Create the destination folder if it doesn't exist
    if (-not (Test-Path $targetExtractFolder)) {
        New-Item -ItemType Directory -Path $targetExtractFolder | Out-Null
    }

    # do file extraction
    Get-ChildItem -Path $targetZipFile -Filter "*.tar.gz" | ForEach-Object {
        tar -xvzf $_.FullName -C $targetExtractFolder
    }

}
function ExpandZipFile($targetZipFile, $targetExtractFolder) {

    # Check if the zip file already exists
    if (-not (Test-Path -Path $targetExtractFolder)) {
        Write-Host "Extracting $targetZipFile to $targetExtractFolder..."

        # Extract the file to the target folder
        Expand-Archive -Path $targetZipFile -DestinationPath $targetExtractFolder -Force
    } else {
        Write-Host "$targetExtractFolder already exists...."
    }
}


# Function to build the API server
function BuildApiServer {
    param (
        [string]$outputFolder,
        [string]$runtime
    )

    # make sure back in DesktopAPI project folder
    Set-Location $PSScriptRoot

    # Change directory to the API folder (up one level)
    Set-Location "..\API"

    # Run the build command
    dotnet publish --configuration Release --output $outputFolder --runtime $runtime --verbosity Normal

    # Copy required files to the output folder
    Copy-Item -Path ".\host.json" -Destination $outputFolder
    Copy-Item -Path ".\local.settings.json" -Destination $outputFolder
}

function BuildDesktopApi {
    param (
        [string]$outputFolder,
        [string]$runtime
    )

    # make sure back in DesktopAPI project folder
    Set-Location $PSScriptRoot

    # Run the build command
    dotnet publish --configuration Release --output $outputFolder --self-contained True --runtime $runtime --verbosity Normal /property:PublishTrimmed=True /property:PublishSingleFile=True /property:IncludeNativeLibrariesForSelfExtract=True /property:DebugType=None /property:DebugSymbols=False
    #dotnet publish --configuration Release --output $outputFolder --runtime $runtime --verbosity Normal

}

# Define the base folder path during building on Windows side
$baseFolder = "publish"

# Operating system-specific download URLs and file names

$osSpecificUrls = @{
    "osx-x64" = @{
        dotNetDownloadUrl = "https://download.visualstudio.microsoft.com/download/pr/9d3fae98-a6af-4ce8-868a-db721c5825a1/e70f1e87a433ab1fbf6b94eb5d0c162d/dotnet-runtime-8.0.6-osx-x64.pkg"
        functionsCliZipUrl = "https://github.com/Azure/azure-functions-core-tools/releases/download/4.0.5907/Azure.Functions.Cli.osx-x64_net8.4.0.5907.zip"
    };
    #"osx-arm64" = @{
    #    dotNetDownloadUrl = "https://download.visualstudio.microsoft.com/download/pr/ea249dde-337d-417d-a615-1f2e0a29b1fc/ef9f8aab388fc5f9ef11a188c4da92fd/dotnet-runtime-8.0.6-osx-arm64.pkg"
    #    functionsCliZipUrl = "https://github.com/Azure/azure-functions-core-tools/releases/download/4.0.5907/Azure.Functions.Cli.osx-arm64_net8.4.0.5907.zip"
    #};
    #"win-x64" = @{
    #    dotNetDownloadUrl = "https://download.visualstudio.microsoft.com/download/pr/3c5bbae6-d848-46b0-bb65-c4f7a7ad4b2a/afba8a75f7e7f4f304362de0f1d4b3ea/dotnet-runtime-8.0.6-win-x64.zip"
    #    functionsCliZipUrl = "https://github.com/Azure/azure-functions-core-tools/releases/download/4.0.5907/Azure.Functions.Cli.win-x64_net8.4.0.5907.zip"
    #};
    #"linux-x64" = @{
    #    dotNetDownloadUrl = "https://download.visualstudio.microsoft.com/download/pr/021c3de8-14d5-493f-92dc-2c8f8be76961/6ee3407acebf74631bfc01f14301afa6/dotnet-runtime-8.0.6-linux-x64.tar.gz"
    #    functionsCliZipUrl = "https://github.com/Azure/azure-functions-core-tools/releases/download/4.0.5907/Azure.Functions.Cli.linux-x64_net8.4.0.5907.zip"
    #};
}

# Loop through the operating systems
foreach ($os in $osSpecificUrls.Keys) {
    # Create the OS-specific folder
    $osFolder = Join-Path -Path $baseFolder -ChildPath $os # add publish folder
    $osFolder = Join-Path -Path $PSScriptRoot -ChildPath $osFolder # add in absolute file location
    New-Item -Path $osFolder -ItemType Directory -Force | Out-Null

    # Set the .NET installer file name and URL based on the OS
    $dotNetFileName = $osSpecificUrls[$os].dotNetDownloadUrl.Split('/')[-1]
    $dotNetDownloadUrl = $osSpecificUrls[$os].dotNetDownloadUrl

    # Set the Azure Functions CLI folder and URL based on the OS
    $functionsCliZipUrl = $osSpecificUrls[$os].functionsCliZipUrl

    # ----------------------- CHECKING .NET installer -----------------------
    Write-Host "Downloading .NET installer for $os......"
    $oriZipFilePath = Download -url $dotNetDownloadUrl -targetZipFolder $osFolder
    
    # ----------------------- CHECKING azure-functions-core-tools installer -----------------------
    $azureCLIFilePath =(Join-Path -Path $osFolder -ChildPath "Azure.Functions.Cli")
    Write-Host "Downloading Function Core Tools for $os......"
    $oriZipFilePath = Download -url $functionsCliZipUrl -targetZipFolder $osFolder #store zip in os for future hash checks
    ExpandZipFile -targetZipFile $oriZipFilePath -targetExtractFolder $azureCLIFilePath # store extracted in sub place as API dll

    # ----------------------- BUILDING API Server -----------------------
    Write-Host "🏗️ Building API Server for $os......"
    # NOTE: place API build inside dotnet build folder
    BuildApiServer -outputFolder $osFolder -runtime $os
    
    # ----------------------- BUILDING API Server -----------------------
    Write-Host "🏗️ Building Desktop API Runner for $os......"
    # NOTE: place API build inside dotnet build folder
    BuildDesktopApi -outputFolder $osFolder -runtime $os


    Write-Host "✅ $os setup complete!"

    Read-Host "Press Enter to CONTINUE..."
}

# Wait for user input (press any key to continue)
Read-Host "Press Enter to exit..."