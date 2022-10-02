
function Get-MimeType()
{
  param($extension = $null);
  $mimeType = $null;
  if ( $null -ne $extension )
  {
    $drive = Get-PSDrive HKCR -ErrorAction SilentlyContinue;
    if ( $null -eq $drive )
    {
      $drive = New-PSDrive -Name HKCR -PSProvider Registry -Root HKEY_CLASSES_ROOT
    }
    $mimeType = (Get-ItemProperty HKCR:$extension)."Content Type";
  }
  $mimeType;
}

function GetExtension(){
    param ($filePath)
    #Write-Output $filePath
   
    #return file extention
    Add-Type -AssemblyName "System.Web"
    $extension = [System.Web.MimeMapping]::GetMimeMapping($filePath)
    Write-Output $extension
    $extension
}

#build project release
Write-Host "Building"

#dotnet publish -c Release

#upload to server
Write-Host "Login"

#PAY AS YOU GO
Update-AzConfig -DefaultSubscriptionForLogin f453f954-107f-4b85-b1d3-744b013dfd5d

Connect-AzAccount

$uploadstorage=Get-AzStorageAccount -ResourceGroupName VedAstro -Name vedastrowebsitestorage

$storcontext=$uploadstorage.Context

#upload to server
Write-Host "Uploading"

#---------------change folder with files to upload
cd "C:\Users\vigne\OneDrive\Desktop\Genso.Astrology\Website\bin\Release\net6.0\publish\wwwroot"

#---------------upload all files in the current folder


#Get-ChildItem -File -Recurse | ForEach-Object {}
$contentType = "test"

#Get-ChildItem -File -Recurse | ForEach-Object -Begin {$contentType = [System.Web.MimeMapping]::GetMimeMapping($_.FullName)} -Process {%{$contentType}}
Get-ChildItem -File -Recurse | ForEach-Object -Begin {$contentType = GetExtension($_.FullName)} -Process {Set-AzStorageBlobContent -File $_.FullName -Container '$web' -Context $storcontext -Force -Properties @{"ContentType" = $contentType}}

Write-Host $contentType
#Set-AzStorageBlobContent -Container $web -File "<Location of file in local disk>" -Context $storcontext
#Get-ChildItem -File -Recurse | Set-AzStorageBlobContent -Container "<storage container name>" -Context $storcontext

#$env:AZCOPY_CRED_TYPE = "Anonymous";
#./azcopy.exe copy "C:\Users\vigne\OneDrive\Desktop\Genso.Astrology\Website\bin\Release\net6.0\publish\wwwroot\*" "https://vedastrowebsitestorage.blob.core.windows.net/$web?sv=2021-04-10&st=2022-09-30T12%3A09%3A03Z&se=2025-10-01T12%3A09%3A00Z&sr=c&sp=racwdxltf&sig=tO6eVgPjK5NSbotvMxWg7eyTeALUusrhLouJFU%2F1bBE%3D" --overwrite=true --from-to=LocalBlob --blob-type Detect --follow-symlinks --put-md5 --follow-symlinks --disable-auto-decoding=false  --recursive --log-level=INFO;
#$env:AZCOPY_CRED_TYPE = "";

#$filepath = "c:\temp\index.html"
#Get-MimeType(get-item $filepath).Extension

#---------------change folder back
cd "C:\Users\vigne\OneDrive\Desktop\Genso.Astrology\Website"

Write-Host "Done!"
Read-Host -Prompt "Press Enter to exit"
$env:AZCOPY_CRED_TYPE = "Anonymous";
./azcopy.exe remove 'https://vedastrowebsitestorage.blob.core.windows.net/$web/?sv=2021-04-10&se=2022-10-31T11%3A19%3A33Z&sr=c&sp=rdl&sig=uLo0l%2BuYH4E2DS2d6BYvjdH3n6cACT912gNjTQY6Fc0%3D' --from-to=BlobTrash --list-of-files "C:\Users\vigne\AppData\Local\Temp\stg-exp-azcopy-40038261-ac5b-4ad1-85a8-71e247e244c4.txt" --recursive --log-level=INFO;
$env:AZCOPY_CRED_TYPE = "";

#clear folder
#$env:AZCOPY_CRED_TYPE = "Anonymous";
#./azcopy.exe remove "https://vedastrowebsitestorage.blob.core.windows.net/$web/?sv=2021-04-10&se=2022-10-31T11%3A19%3A33Z&sr=c&sp=rdl&sig=uLo0l%2BuYH4E2DS2d6BYvjdH3n6cACT912gNjTQY6Fc0%3D" --from-to=BlobTrash --list-of-files "C:\Users\vigne\AppData\Local\Temp\stg-exp-azcopy-40038261-ac5b-4ad1-85a8-71e247e244c4.txt" --recursive --log-level=INFO;
#$env:AZCOPY_CRED_TYPE = "";
