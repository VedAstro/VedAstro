
# Function to get app settings
function Login-Azure {
    # Log in to Azure
    az login

    # Select the subscription
    $subscriptionId = Read-Host "Enter the Subscription ID"
    az account set --subscription $subscriptionId       
}

# Function to print app settings
function Print-AppSettings {

    $targetFunctionName = Read-Host "Enter the name of the target Azure Function"
    $targetResourceGroup = Read-Host "Enter the name of the target resource group"

    # Get the app settings
    $jsonOutput = az functionapp config appsettings list --name $targetFunctionName --resource-group $targetResourceGroup --output json
    try {
        $appSettings = $jsonOutput | ConvertFrom-Json
    }
    catch {
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
        Values      = $valuesHashtable
        Host        = @{
            LocalHttpPort   = 7071
            CORS            = "*"
            CORSCredentials = $false
        }
    }
    
    # Print the app settings to the console
    $finalObject | ConvertTo-Json -Depth 100 | Write-Output
}

# Function to save app settings
function Save-AppSettings {
    
    $targetFunctionName = Read-Host "Enter the name of the target Azure Function"
    $targetResourceGroup = Read-Host "Enter the name of the target resource group"

    # Get the app settings
    $jsonOutput = az functionapp config appsettings list --name $targetFunctionName --resource-group $targetResourceGroup --output json
    try {
        $appSettings = $jsonOutput | ConvertFrom-Json
    }
    catch {
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
        Values      = $valuesHashtable
        Host        = @{
            LocalHttpPort   = 7071
            CORS            = "*"
            CORSCredentials = $false
        }
    }

    # Save the app settings to a file
    $finalObject | ConvertTo-Json -Depth 100 | Out-File -FilePath local.settings.latest.json
}

# Function to clear app settings
function Clear-AppSettings {
    param($targetFunctionName, $targetResourceGroup)
    # Get the current app settings
    $currentSettings = az functionapp config appsettings list --name $targetFunctionName --resource-group $targetResourceGroup --output json | ConvertFrom-Json

    # Remove each setting
    foreach ($setting in $currentSettings) {
        az functionapp config appsettings delete --name $targetFunctionName --resource-group $targetResourceGroup --setting-names $setting.name
    }

    Write-Host "Old App settings cleared successfully."
}

# Function to clone app settings
function Clone-AppSettings {
    
    # get source of clone data
    $sourceFunctionName = Read-Host "Enter the name of the SOURCE Azure Function"
    $sourceResourceGroup = Read-Host "Enter the name of the SOURCE resource group"

    # Get the app settings
    $jsonOutput = az functionapp config appsettings list --name $sourceFunctionName --resource-group $sourceResourceGroup --output json
    try {
        $appSettings = $jsonOutput | ConvertFrom-Json
    }
    catch {
        Write-Host "Failed to parse app settings as JSON. Please check the output of the 'az functionapp config appsettings list' command."
        exit
    }
        
    # Clone the app settings to another function
    $targetFunctionName = Read-Host "Enter the name of the TARGET Azure Function"
    $targetResourceGroup = Read-Host "Enter the name of the TARGET resource group"

    # Clear existing app settings
    Clear-AppSettings -targetFunctionName $targetFunctionName -targetResourceGroup $targetResourceGroup

    foreach ($setting in $appSettings) {
        if ($setting.value -ne $null) {
            az functionapp config appsettings set --name $targetFunctionName --resource-group $targetResourceGroup --settings "$($setting.name)=$($setting.value)"
        }
    }
    Write-Host "App settings cloned successfully."
}

# Function to upload app settings
function Upload-AppSettings {
    param($appSettings)
    # Upload configuration from a local file to an online Azure function
    $filePath = Read-Host "Enter the path of the local JSON file containing the configuration settings"
    $targetFunctionName = Read-Host "Enter the name of the target Azure Function"
    $targetResourceGroup = Read-Host "Enter the name of the target resource group"

    # Clear existing app settings
    Clear-AppSettings -targetFunctionName $targetFunctionName -targetResourceGroup $targetResourceGroup

    try {
        $localConfig = Get-Content $filePath -Raw | ConvertFrom-Json

        foreach ($property in $localConfig.Values.PSObject.Properties) {
            $key = $property.Name
            $value = $property.Value

            Write-Host ("Key: {0}" -f $key)  # Print out each key name
            if ($key -ne $null -and $value -ne $null) {
                az functionapp config appsettings set --name $targetFunctionName --resource-group $targetResourceGroup --settings "$key=$value"
            }
        }

        Write-Host "Configuration uploaded successfully."
    }
    catch {
        Write-Host ("Failed to upload configuration: {0}" -f $_)
    }
}

# Main function
function Main {
    Login-Azure


    # Ask the user what they want to do with the app settings
    $userChoice = Read-Host "Do you want to print the app settings to the console, save them to a file, clone them to another function, or upload from a local configuration file? (print/save/clone/upload)"

    switch ($userChoice) {
        "print" {
            Print-AppSettings
        }
        "save" {
            Save-AppSettings
        }
        "clone" {
            Clone-AppSettings
        }
        "upload" {
            Upload-AppSettings
        }
        default {
            Write-Host "Invalid input. Please enter 'print', 'save', 'clone', or 'upload'."
        }
    }

    # Wait for user input before closing the window
    Write-Host "Done. Press Enter to exit."
    Read-Host
}

# Call the main function
Main
