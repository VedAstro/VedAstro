# Log in to Azure
az login

# Get the app settings
$jsonOutput = az functionapp config appsettings list --name VedAstroAPI --resource-group VedAstro --output json
try {
    $appSettings = $jsonOutput | ConvertFrom-Json
} catch {
    Write-Host "Failed to parse app settings as JSON. Please check the output of the 'az functionapp config appsettings list' command."
    exit
}

# Create a hashtable for the 'Values' property
$valuesHashtable = @{}
foreach ($setting in $appSettings) {
    if ($setting.value -ne $null) {
        $valuesHashtable[$setting.name] = $setting.value
    }
}

# Construct the final object
$finalObject = @{
    IsEncrypted = $false
    Values = $valuesHashtable
    Host = @{
        LocalHttpPort = 7071
        CORS = "*"
        CORSCredentials = $false
    }
}

# Ask the user what they want to do with the app settings
$userChoice = Read-Host "Do you want to print the app settings to the console, save them to a file, or clone them to another function? (print/save/clone)"

switch ($userChoice) {
    "print" {
        # Print the app settings to the console
        $finalObject | ConvertTo-Json -Depth 100 | Write-Output
    }
    "save" {
        # Save the app settings to a file
        $finalObject | ConvertTo-Json -Depth 100 | Out-File -FilePath local.settings.latest.json
    }
    "clone" {
        # Clone the app settings to another function
        $targetFunctionName = Read-Host "Enter the name of the target Azure Function"
        $targetResourceGroup = Read-Host "Enter the name of the target resource group"
        foreach ($setting in $appSettings) {
            if ($setting.value -ne $null) {
                az functionapp config appsettings set --name $targetFunctionName --resource-group $targetResourceGroup --settings "$($setting.name)=$($setting.value)"
            }
        }
        Write-Host "App settings cloned successfully."
    }
    default {
        Write-Host "Invalid input. Please enter 'print', 'save', or 'clone'."
    }
}

# Wait for user input before closing the window
Write-Host "Done. Press Enter to exit."
Read-Host
