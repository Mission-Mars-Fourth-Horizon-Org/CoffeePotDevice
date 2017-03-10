Param(
    [Parameter(Mandatory=$true)]
    [string]$g,
    [Parameter(Mandatory=$true)]
    [string]$n
)

Write-Host "================================================================================"
Write-Host "Showing Details"
Write-Host "================================================================================"

Write-Host "`n`n================================================================================"
Write-Host "$n IoT Hub Details"
Write-Host "--------------------------------------------------------------------------------"
az iot hub show --resource-group "$g" --name "$n" -o table

Write-Host "`n`n================================================================================"
Write-Host "$n IoT Hub consumer groups"
Write-Host "--------------------------------------------------------------------------------"
az iot hub consumer-group list --resource-group "$g" --hub-name "$n" --event-hub-name events -o tsv

Write-Host "`n`n================================================================================"
Write-Host "$n IoT Hub iothubowner connection string - Used by presenters"
Write-Host "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "$g" --name "$n" --policy-name "iothubowner" --key primary -o tsv

Write-Host "`n`n================================================================================"
Write-Host "$n IoT Hub coffeeclient connection string - Used by attendees"
Write-Host "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "$g" --name "$n" --policy-name "coffeeclient" --key primary -o tsv

Write-Host "`n`n================================================================================"
Write-Host "coffeepot device connection string"
Write-Host "--------------------------------------------------------------------------------"
az iot device show-connection-string --resource-group "$g" --hub-name "$n" --device-id "coffeepot" --key primary -o tsv

Write-Host "`n`n"
