
export resourceGroup=<resource-group-name>
export storageAccountName=<storage-account-name>     # All lower case latters or numbers, no dashes (-) or underscores (_)
export location=<location>                           # az account list-locations --output table

# Create a Storage account
az storage account create \
    --name $storageAccountName \
    --resource-group $resourceGroup \
    --location $location

# Create some initial blob containers in the storage account (optional)
az storage container create \
    --account-name $storageAccountName \
    --name photos \
    --auth-mode login

az storage container create \
    --account-name $storageAccountName \
    --name ebooks \
    --auth-mode login

# Upload some sample files to Blob storage (optional)
az storage blob upload \
    --account-name $storageAccountName \
    --container-name photos \
    --file dotnet-bot_chilling.png \
    --name dotnet-bot_chilling.png
    
az storage blob upload \
    --account-name $storageAccountName \
    --container-name photos \
    --file dotnet-bot_grilling.png \
    --name dotnet-bot_grilling.png


# Get the connection string for use with the storage account
echo 'This is the connection string your application will use to connect to the storage account'
echo 'Safeguard this value like you would any other secret'
az storage account show-connection-string \
    -g $resourceGroup \
    -n $storageAccountName \
    --output tsv
