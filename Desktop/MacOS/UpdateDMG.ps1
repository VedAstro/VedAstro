# Set the input and output file paths
$inputDmgFile = "C:\Users\ASUS\Desktop\Projects\VedAstro\DesktopAPI\MacOS\VedAstro.dmg"
$outputDmgFile = "C:\Users\ASUS\Desktop\Projects\VedAstro\DesktopAPI\MacOS\OUTPUT-VedAstro.dmg"

# Set the file to replace inside the dmg file
$replaceableFile = "replaceable.txt"
$newFileContent = "New content for replaceable.txt"

# Set the path to the 7z executable
$7zPath = "C:\Program Files\7-Zip\7z.exe"

# Extract the dmg file using 7z
$extractDir = "C:\Users\ASUS\Desktop\Projects\VedAstro\DesktopAPI\MacOS\ExtractedDmg"
& $7zPath x $inputDmgFile -o"$extractDir"

# Replace the file inside the extracted directory
$replaceableFilePath = Join-Path -Path $extractDir -ChildPath $replaceableFile
Set-Content -Path $replaceableFilePath -Value $newFileContent

# Create a new dmg file with the modified contents
& $7zPath a -tudf $outputDmgFile $extractDir\*

# Clean up the temporary directory
Remove-Item -Path $extractDir -Recurse -Force