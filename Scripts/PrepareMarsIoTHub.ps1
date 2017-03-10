Param(
    [Parameter(Mandatory=$true)]
    [string]$g,
    [Parameter(Mandatory=$true)]
    [string]$n,
    [Parameter(Mandatory=$true)]
    [string]$l
)

Write-Host "`nCreating the Azure Resource Group $g ..."
az group create --name "$g" --location "$l"

Write-Host "`nCreating the IoT Hub $n (this will take a few minutes) ..."
az iot hub create --resource-group "$g" --name "$n" --sku "S1" --unit 1 --location "$l"

Write-Host "`nCreating the Event Hub Consumer Groups team01-team20 ..."
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team01"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team02"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team03"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team04"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team05"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team06"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team07"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team08"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team09"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team10"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team11"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team12"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team13"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team14"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team15"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team16"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team17"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team18"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team19"
az iot hub consumer-group create --resource-group "$g" --hub-name "$n" --event-hub-name events --name "team20"

Write-Host "`nCreating the 'coffeeclient' SAS Policy ..."
az iot hub policy create --resource-group "$g" --hub-name "$n" --name "coffeeclient" --permissions ServiceConnect

Write-Host "`nCreating the 'coffeepot' device ..."
az iot device create --resource-group "$g" --hub-name "$n" --device-id "coffeepot"

Write-Host "`n`n"

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