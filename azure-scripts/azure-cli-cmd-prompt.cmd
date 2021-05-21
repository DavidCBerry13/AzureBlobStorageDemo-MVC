

REM Create a Storage account
az storage account create ^
    --name <name> ^
    --resource-group <resource-group-name> ^
    --location <location>

REM Get the connection string for use with the storage account
az storage account show-connection-string ^
    -g <resource-group-name> ^
    -n <storage-account-name> ^
    --output tsv

REM Create some initial blob containers in the storage account (optional)
az storage container create ^
    --name photos ^
    --connection-string <storage-account-name>

az storage container create ^
    --name ebooks ^
    --connection-string <storage-account-name>



az storage account show-connection-string ^
    -g rg-storage-blob-demo ^
    -n storageblobdemodcb ^
    --output tsv



    az storage account show-connection-string `
    -g rg-storage-blob-demo `
    -n storageblobdemodcb `
    --output tsv