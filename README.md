# CoffeePotDevice

This repository contains the source code for the "***Coffee Pot Quantum Entangled Device Twin***" used in the <a target="_blank" href="https://github.com/Mission-Mars-Fourth-Horizon-Org/Mission-Briefings/tree/master/IoTHubs">IoTHubs</a> Mission Mars mission.

The "***Coffee Pot Quantum Entangled Device Twin***" needs to be run by the PRESENTER (not the attendees) at a Mission Mars event.  

As a presenter, there are a few steps you'll have to do to prepare the CoffePotDevice.  Those steps are documented completely in this document. 

## Prerequisites

To configure the IoT Hub used by the CoffeePotDevice as well as to run the CoffeePotDevice app you will need the following

- Windows 10 - The CoffeePotDevice app is a UWP app.  At this time, no cross-platform version of the app exists
- PowerShell - The "PrepareMarsIoTHub.ps1" script helps you provision the Azure resources needed by the app.
- Azure-CLI 2.0 (<a target="_blank" href="https://docs.microsoft.com/en-us/cli/azure/install-azure-cli">Install Docs</a>) - This is used by the "PrepareMarsIoTHub.ps1" script to create the resources in Azure.
- Visual Studio 2015 Community Edition or Later - Used to load the CoffeePotDevice app source code and run it. 

## Tasks

1. [Prepare the Azure IoT Hub needed by the app](#prep)
1. [Capture the details of the IoT Hub and Device](#details)
1. [Run and configure the CoffeePotDevice UWP App](#run)

---

<a name="prep"></a>

## Prepare the Azure IoT Hub needed by the app

You need to configure an IoT Hub and device in Azure that the CoffeePotDevice app will connect to.  Once attendees have complete the tutorial walkthroughs, they will modify they code they have written to also point to this hub, via a Consumer Group that maps to their "teamxx" number and communicate with the CoffeePotDevice App.

For this to all work you need to create the following items in ***your*** azure subscription:

- An Azure Resource Group
- An Azure IoT Hub
- A "coffeeclient" SAS Policy in the hub with "ServiceConnect" permissions that the attendees will use to connect to your hub. 
- A "coffeepot" device identity that the CoffeePotDevice app will connect as
- A "teamxx" consumer group on the "events" endpoint for "team01"-"team20" that the attendess will use to listen for messages coming back from the CoffeePotDevice simulated app. 

To help you in this effort, A PowerShell script that uses the "Azure CLI 2.0" cross platform cli has been created to provision all of the required resources. Use these steps to run the script:

1. Clone this repo down to your computer (wherever and however you choose)
1. Open a ***PowerShell*** prompt and change into the `/scripts` directory in the repo.
1. Login to the azure cli and ensure that the desired subscription is current (Read <a target="_blank" href="https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli">Log in with Azure CLI 2.0</a> for more details):

    ```bash
    az login
    ```

    Then uses these commands to list, set and show your account info

    ```bash
    az account list
    az account set
    az account show
    ```

1. Decide on a name for the Resource Group and IoT Hub you will create an the Location where you want them. It's suggested that you use a name that includes "mars" to keep in theme with the event.  Perpahs appending your city name, airport code, etc. to help keep it unique. For example, if yuo were configuring the resources for an event in Seattle you might use  the `sea` airport code in your names:

    - Resource Group Name: **`marsgroupsea`**
    - IoT Hub Name: **`marsiotsea`**
    - Location: **`westus`** (Since Seattle is on the West Coast of the US)

1. Use the names and locations chosen above to run the `./Scripts/PepareMarsIoTHub.ps1` script.  The script will take a few minutes (5-ish) to complete. Creating the IoT Hub and Consumer Groups take up most of the time:

    > **Note**: When executing the `PrepareMarsIoTHub.ps1` script you must prefix it with the "**`./`**" current directory reference for PowerShell to properly locate it.

    ```bash
    ./PrepareMarsIoTHub.ps1 -g <resourece-group-name> -n <iot-hub-name> -l <location>
    ```

    For example using the names we chose above:

    ```bash
    ./PrepareMarsIoTHub.ps1 -g marsgroupsea -n marsiotsea -l westus
    ```

1. When the script is done executing, the details of the resources created are shown at end (there are a lot of them so you will need to scroll).  Keep these resources up on the screen so you can use them in the next step:

    ```bash
    ================================================================================
    Showing Details
    ================================================================================


    ================================================================================
    marsiotsea IoT Hub Details
    --------------------------------------------------------------------------------
    Location    Name        ResourceGroup    Resourcegroup    Subscriptionid
    ----------  ----------  ---------------  ---------------  ------------------------------------
    westus      marsiotsea  marsgroupsea     marsgroupsea     ed2f73c5-c021-4b86-9afb-aa7998d16085


    ================================================================================
    marsiotsea IoT Hub consumer groups
    --------------------------------------------------------------------------------
    $Default
    team01
    team02
    team03
    ...
    team20


    ================================================================================
    marsiotsea IoT Hub iothubowner connection string - Used by presenters
    --------------------------------------------------------------------------------
    HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=XFoYt5f3xI2ebGxRVpWCfua2++CH+1KIrP5RjRQp6ts=


    ================================================================================
    marsiotsea IoT Hub coffeeclient connection string - Used by attendees
    --------------------------------------------------------------------------------
    HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=1EmQKhYMBdamoriZZM7JNcWSUoEngRlypAdX4Eghc7E=


    ================================================================================
    coffeepot device connection string
    --------------------------------------------------------------------------------
    HostName=marsiotsea.azure-devices.net;DeviceId=coffeepot;SharedAccessKey=wC4gm+mXqc0UA2eOhG+vLaYijX/zXL7w85gw003TmTk=
    ```

---

<a name="details"></a>

## Capture the details of the IoT Hub and Device

You will need to capture a few details from the resources you provisioned above.

- The "iothubowner" connection string for your own use in the CoffeePotDevice app
- the "coffeecleint" connection string for attendees to use in the lab.
- The "cofeepot" device id.  It should be "coffeepot" but you need to confirm it.

1. If you accidentally cleared the output from the step above, you can retrieve the details of the resources you provisioned above using the `./Scripts/ShowMarsIoTHub.ps1` script.  Just make sure to use the same resource group name and iot hub name as above:

    ```bash
    ./ShowMarsIoTHub.ps1 -g <resource-group-name> -n <iot-hub-name>
    ```

    Again, using our "Seattle" values from above:

    ```bash
    ./ShowMarsIoTHub.ps1 -g marsgroupsea -n marsiotsea
    ```

1. First locate the `iothubowner` connection string and copy it's value out to a text file for your use.

1. Copy the 'cofeeclient' connection string, and put it in a public document that attendees can link to for use during the event. 


---

<a name="run"></a>

## Run and configure the CoffeePotDevice UWP App

1. Open the CofeePotDevice app in Visual Studio, ensure the target platform is x86 and the target device is "Local Machine"
1. Run it
1. Click on the "Gear" icon in the top left corner
1. On the "Iot Hub Settings" tab, paste the `iothubowner` connection string you copied above in.
1. Switch to the "Devices" tab
1. Click the "Get Devices" button
1. Click the "coffeepot" device in th list to select it
1. Click the "Entangle Device" button to have the app simulate the "coffeepot" device.
1. Click the "Home" icon in the top left corner to return to the main screen
1. Leave the app up and running througout the event so attendees can see their messages pop up.
1. The app has to be in the foreground to run and have the sounds associated with the messages be heard.
