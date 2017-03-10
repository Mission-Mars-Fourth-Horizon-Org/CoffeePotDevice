azure group create <YourGroupName> -l <YourLocation>
azure iothub create -g <YourGroupName> -n <YourIoTHubName> -k "S1" -u 1 -l <YourLocation>
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team01"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team02"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team03"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team04"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team05"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team06"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team07"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team08"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team09"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team10"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team11"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team12"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team13"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team14"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team15"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team16"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team17"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team18"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team19"
azure iothub ehconsumergroup create -g <YourGroupName> -n <YourIoTHubName> -e events -c "team20"
azure iothub ehconsumergroup list -g <YourGroupName> -n <YourIoTHubName> -e events
azure iothub key list -g <YourGroupName> -n <YourIoTHubName>