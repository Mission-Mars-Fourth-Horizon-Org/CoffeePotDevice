Param(
    [Parameter(Mandatory=$true)]
    [string]$g,
    [Parameter(Mandatory=$true)]
    [string]$n,
    [Parameter(Mandatory=$true)]
    [string]$l
)
azure group create -n "$g" -l "$l"
azure iothub create -g "$g" -n "$n" -k "S1" -u 1 -l "$l"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team01"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team02"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team03"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team04"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team05"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team06"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team07"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team08"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team09"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team10"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team11"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team12"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team13"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team14"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team15"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team16"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team17"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team18"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team19"
azure iothub ehconsumergroup create -g "$g" -n "$n" -e events -c "team20"
azure iothub ehconsumergroup list -g "$g" -n "$n" -e events
azure iothub key list -g "$g" -n "$n"