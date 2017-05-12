#!/bin/bash

echo Args: $@

while getopts "g:n:l:" optname
    do
        case "$optname" in
            "g")
                g=$OPTARG
                ;;
            "n")
                n=$OPTARG
                ;;
            "l")
                l=$OPTARG
                ;;
        esac
    done

echo
echo "Creating the Azure Resource Group ${g} ..."
az group create --name "${g}" --location "${l}"

echo
echo "Creating the IoT Hub ${n} (this will take a few minutes) ..."
az iot hub create --resource-group "${g}" --name "${n}" --sku "S1" --unit 1 --location "${l}"

echo
echo "Creating the Event Hub Consumer Groups team01-team20 ..."
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team01"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team02"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team03"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team04"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team05"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team06"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team07"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team08"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team09"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team10"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team11"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team12"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team13"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team14"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team15"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team16"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team17"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team18"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team19"
az iot hub consumer-group create --resource-group "${g}" --hub-name "${n}" --event-hub-name events --name "team20"

echo
echo "Creating the 'coffeeclient' SAS Policy ..."
az iot hub policy create --resource-group "${g}" --hub-name "${n}" --name "coffeeclient" --permissions ServiceConnect

echo
echo "Creating the 'coffeepot' device ..."
az iot device create --resource-group "${g}" --hub-name "${n}" --device-id "coffeepot"

echo
echo
echo "================================================================================"
echo "Showing Details"
echo "================================================================================"

echo
echo
echo "================================================================================"
echo "${n} IoT Hub Details"
echo "--------------------------------------------------------------------------------"
az iot hub show --resource-group "${g}" --name "${n}" -o table

echo
echo
echo "================================================================================"
echo "${n} IoT Hub consumer groups"
echo "--------------------------------------------------------------------------------"
az iot hub consumer-group list --resource-group "${g}" --hub-name "${n}" --event-hub-name events -o tsv

echo
echo
echo "================================================================================"
echo "${n} IoT Hub iothubowner connection string - Used by presenters"
echo "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "${g}" --name "${n}" --policy-name "iothubowner" --key primary -o tsv

echo
echo
echo "================================================================================"
echo "${n} IoT Hub coffeeclient connection string - Used by attendees"
echo "--------------------------------------------------------------------------------"
az iot hub show-connection-string --resource-group "${g}" --name "${n}" --policy-name "coffeeclient" --key primary -o tsv

echo
echo
echo "================================================================================"
echo "coffeepot device connection string"
echo "--------------------------------------------------------------------------------"
az iot device show-connection-string --resource-group "${g}" --hub-name "${n}" --device-id "coffeepot" --key primary -o tsv

echo
echo