Param(
    [Parameter(Mandatory=$true)]
    [string]$g,
    [Parameter(Mandatory=$true)]
    [string]$n
)

Write-Host "================================================================================"
Write-Host "Showing Details"
Write-Host "================================================================================"

Write-Host "`n`n--------------------------------------------------------------------------------"
Write-Host "$n IoT Hub Details ..."
Write-Host "--------------------------------------------------------------------------------"
az iot hub show --resource-group "$g" --name "$n" 

Write-Host "`n`n--------------------------------------------------------------------------------"
Write-Host "$n IoT Hub consumer groups..."
Write-Host "--------------------------------------------------------------------------------"
az iot hub consumer-group list --resource-group "$g" --hub-name "$n" --event-hub-name events

Write-Host "`n`n--------------------------------------------------------------------------------"
Write-Host "$n IoT Hub iothubowner connection string - Used by presenters in the CoffeePotDeviceApp..."
Write-Host "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "$g" --name "$n" --policy-name "iothubowner" --key primary

Write-Host "`n`n--------------------------------------------------------------------------------"
Write-Host "$n IoT Hub coffeeclient connection string - Used by attendees..."
Write-Host "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "$g" --name "$n" --policy-name "coffeeclient" --key primary

Write-Host "`n`n--------------------------------------------------------------------------------"
Write-Host "coffeepot device details and connection string..."
Write-Host "--------------------------------------------------------------------------------"
az iot device show --resource-group "$g" --hub-name "$n" --device-id "coffeepot"
az iot device show-connection-string --resource-group "$g" --hub-name "$n" --device-id "coffeepot" --key primary
