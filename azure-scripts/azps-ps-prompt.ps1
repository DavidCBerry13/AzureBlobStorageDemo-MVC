# PowerShell script to provision Azure resources for Storage Blobs demo
#
# If not already signed in to Azure, use Connect-AzAccount to sign in
# #################################################################################################

$resourceGroup = "<resource-group-name>"
$storageAccountName = "<storage-account-name>"     # All lower case latters or numbers, no dashes (-) or underscores (_)
$location = "<location>"                           # List locations using 'Get-AzLocation | select location'


# Create a Resource Group
New-AzResourceGroup `
    -Name $resourceGroup `
    -Location $location

# Create a Storage account
$storageAccount = New-AzStorageAccount `
    -AccountName $storageAccountName `
    -ResourceGroupName $resourceGroup `
    -Location $location `
    -Sku Standard_RAGRS

# Create some initial blob containers in the storage account (optional)
New-AzStorageContainer `
    -Context $storageAccount.Context `
    -Name photos

New-AzStorageContainer `
    -Context $storageAccount.Context `
    -Name ebooks

# Upload some initial files
Set-AzStorageBlobContent `
    -Context $storageAccount.Context `
    -Container "photos" `
    -File "dotnet-bot_chilling.png" `
    -Blob "dotnet-bot_chilling.png"

Set-AzStorageBlobContent `
    -Context $storageAccount.Context `
    -Container "photos" `
    -File "dotnet-bot_grilling.png" `
    -Blob "dotnet-bot_grilling.png"

# Get Connection String for Storage Account
$storageKey=(Get-AzStorageAccountKey -ResourceGroupName $ResourceGroup -Name $StorageAccountName).Value[0]
$storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=$storageAccountName;AccountKey=$storageKey;EndpointSuffix=core.windows.net"

Write-Host 'This is the connection string your application will use to connect to the storage account'
Write-Host 'Safeguard this value like you would any other secret'
Write-Host $storageConnectionString


# To remove the resource group, the storage account and all containers and blobs contained within,
# run the following command

# Remove-AzResourceGroup -Name $resourceGroup