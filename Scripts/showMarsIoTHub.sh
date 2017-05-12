#!/bin/bash

echo Args: $@

while getopts "g:n:" optname
    do
        case "$optname" in
            "g")
                g=$OPTARG
                ;;
            "n")
                n=$OPTARG
                ;;
        esac
    done

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

