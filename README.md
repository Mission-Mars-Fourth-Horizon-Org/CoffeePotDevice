# CoffeePotDevice

This repository contains the source code for the "***Coffee Pot Quantum Entangled Device Twin***" used in the <a target="_blank" href="https://github.com/Mission-Mars-Fourth-Horizon-Org/Mission-Briefings/tree/master/IoTHubs">IoTHubs</a> Mission Mars mission.

The "***Coffee Pot Quantum Entangled Device Twin***" needs to be run by the PRESENTER (not the attendees) at a Mission Mars event.  

As a presenter, there are a few steps you'll have to do to prepare the CoffePotDevice.  Those steps are documented completely in this document. 

## Prerequisites

To configure the IoT Hub used by the CoffeePotDevice as well as to run the CoffeePotDevice app you will need the following

- An active Azure Subscription
- Windows 10 - The CoffeePotDevice app is a UWP app.  At this time, no cross-platform version of the app exists
- Visual Studio 2015 Community Edition or Later - Used to load the CoffeePotDevice app source code and run it.
- Device Explorer (Optional) - Follow the instructions to install it on the <a target="_blank" href="https://github.com/Azure/azure-iot-sdk-csharp/tree/master/tools/DeviceExplorer">How to use Device Explorer for IoT Hub devices</a> page

## Tasks

1. [Prepare the Azure IoT Hub needed by the app](#prep)
1. [Make the coffeeclient Connection String available to attendees](#coffeeclient)
1. [Run and configure the CoffeePotDevice UWP App](#run)
1. [Testing the CoffeePotDevice app with Device Explorer](#testing)
1. [Testing with the supplied .net or node.js CoffeeClient code](#testingcode)
1. [Prepare the Team Cards](#teamcards)
1. [Clean up after the event](#cleanup)

---

<a name="prep"></a>

## Prepare the Azure IoT Hub needed by the app

You need to configure an IoT Hub and device in Azure that the CoffeePotDevice app will connect to.  Once attendees have complete the tutorial walkthroughs, they will modify they code they have written to  point to the hub you create below rather then the one they created in the lab, via a Consumer Group that maps to their "teamxx" number and communicate with the "coffeepot" device simulated by the "CoffeePotDevice" app.

For this to all work you need to create the following items in ***your*** azure subscription:

- An Azure Resource Group
- An Azure IoT Hub
- A "coffeeclient" SAS Policy in the hub with "ServiceConnect" permissions that the attendees will use to connect to your hub.
- A "coffeepot" device identity that the CoffeePotDevice app will connect as
- A "teamxx" consumer group on the "events" endpoint for "team01"-"team20" that the attendees will use to listen for messages coming back from the CoffeePotDevice simulated app.

To help you in this effort, a Bash shell script that uses the "Azure CLI 2.0" cross platform cli has been created to provision all of the required resources. Use these steps to run the script using the Azure Cloud Shell:

1. Login to the [Azure Portal](https://portal.azure.com) with the credentials for the subscription you wish to use, then click on the "Cloud Shell" icon along the top to open the Cloud Shell, and ensure that the "**Bash**" shell is selected.  You can also optionally click the button to expand the Cloud Shell window to make it easier to work with commands.

    | **Note**: The "**Azure Cloud Shell**" is a virtual shell environment integrated into the Azure Portal.  It makes it very easy to open a shell environment with the current Azure CLI tools already installed and authenticated against your subscription.  You can learn more about the Azure Cloud Shell here: [Overview of Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/overview)

    ![Open Cloud Bash Shell](images/OpenCloudShell.png)

    Also, if this is the first time you've opened the Cloud Shell for your subscription you may be prompted to create the storage account that backs your "**clouddrive**" share. If so, click the "**Create storage**" button to continue:

    ![Create Storage](images/CreateCloudStorageIfNeeded.png)


1. Next, download the scripts to help you provision your IoT Hub, consumer groups, device and coffeeclient SAS policy to your clouddrive share in the Cloud shell by running the following commands in the Cloud Shell environment:

    | **Note**: If you copy the code below to your clipboard, you can use "**SHIFT-INSERT**" to paste the code into your Cloud Shell terminal.  

    ```bash
    cd ~/clouddrive
    mkdir missionmars
    cd missionmars
    wget https://raw.githubusercontent.com/Mission-Mars-Fourth-Horizon-Org/CoffeePotDevice/master/Scripts/prepareMarsIoTHub.sh
    wget https://raw.githubusercontent.com/Mission-Mars-Fourth-Horizon-Org/CoffeePotDevice/master/Scripts/showMarsIoTHub.sh

    ```

    ![Download Scripts](images/DownloadScripts.png)

1. Decide on a name for the Resource Group and IoT Hub you will create an the Location where you want them. It's suggested that you use a name that includes "mars" to keep in theme with the event.  Perhaps appending your city name, airport code, etc. to help keep it unique. For example, if you were configuring the resources for an event in Seattle you might use  the `sea` airport code in your names:

    - Resource Group Name: **`marsgroupsea`**
    - IoT Hub Name: **`marsiotsea`**
    - Location: **`westus`** (Since Seattle is on the West Coast of the US)

1. Use the names and locations chosen above to run the `~/clouddrive/missionmars/pepareMarsIoTHub.sh` script.  The script will take a few minutes (5-ish) to complete. Creating the IoT Hub and Consumer Groups take up most of the time:

    > **Note**: Use the full path to the script as given below, or if you are sure the "**Azure Cloud Shell**" terminal is currently in the `~/clouddrive/missionmars/` directory and when you can run the `./prepareMarsIoTHub.sh` path to run the script, just make sure to include the `./` prefix so that the bash Cloud Shell can locate it properly.

    ```bash
    ~/clouddrive/missionmars/prepareMarsIoTHub.sh -g <resource-group-name> -n <iot-hub-name> -l <location>
    ```

    For example using the names we chose above:

    ```bash
    ~/clouddrive/missionmars/prepareMarsIoTHub.sh -g marsgroupsea -n marsiotsea -l westus
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

<a name="coffeeclient"></a>

## Make the coffeeclient Connection String available to attendees

You will need to capture a few details from the resources you provisioned above.

- The "iothubowner" connection string for your own use in the CoffeePotDevice app
- the "coffeeclient" connection string for attendees to use in the lab.
- The "cofeepot" device id.  It should be "coffeepot" but you need to confirm it.

1. If you accidentally cleared the output from the step above, you can retrieve the details of the resources you provisioned above using the `~/clouddrive/missionmars/showMarsIoTHub.sh` script in the "**Azure Cloud Shell**".  Just make sure to use the same resource group name and iot hub name as above:

    ```bash
    ~/clouddrive/missionmars/showMarsIoTHub.sh -g <resource-group-name> -n <iot-hub-name>
    ```

    Again, using our "Seattle" values from above:

    ```bash
    ~/clouddrive/missionmars/showMarsIoTHub.sh -g marsgroupsea -n marsiotsea
    ```

1. Locate the `coffeeclient` connection strings from the output of the script, and copy the "`HostName=marsiotsea...;SharedAccessKeyName=coffeeclient;SharedAccessKey=1E...7E=`" connection

    For example, from the output above, locate the "**coffeeclient**" connection string in the details and copy it to a text file.  ***Make sure the entire connection string is on a single line.  If it was wrapped in the output of the script above, fix it in the text file so it is all one line.***

    Also, when copying the output from the **Azure Cloud Shell**, use the **CTRL-INSERT** key combination instead of **CTRL-C** to copy the text.

    ```bash
    ================================================================================
    marsiotsea IoT Hub coffeeclient connection string - Used by attendees
    --------------------------------------------------------------------------------
    HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=1EmQKhYMBdamoriZZM7JNcWSUoEngRlypAdX4Eghc7E=
    ```

1. Save the text file to a folder in OneDrive:

    ![Save CoffeeClient Text File to OneDrive](images/SaveCoffeeClientTextFileToOneDrive.png)

1. Right click on the new file in the OneDrive folder, and select "**Share a OneDrivelink**":

    ![Get OneDrive Link](images/GetCoffeeClientTextFileOneDriveLink.png)

1. Use the URL Shortner of your choice and create an easy to type short URL for the text file:

    > **Note**: Use any URL shortner you like.

    ![URL Shortner](images/CoffeeClientTextFileShortUrl.png)


1. Edit the deck for your event, and paste your short URL in on the instruction slide for the IoT Hub mission.  Attendees will need easy access to the text file the URL points to so they can copy the cofeeclient connection string in the lab.

    ![Short URL In Deck](images/PutShortUrlInSlideDeck.png)

1. Keep the short URL handy during the event so you can easily share it with attendees when they need it.  Attendees will get to the portion of the lab where the connection string is needed at widely different times depending how quickly they progress through the lab.

---

<a name="run"></a>

## Run and configure the CoffeePotDevice UWP App

1. Clone this repo to a path of your choosing on your local Windows 10 computer:

    ```bash
    git clone https://github.com/Mission-Mars-Fourth-Horizon-Org/CoffeePotDevice.git
    ```

1. From folder where the repo was cloned, open the "CoffeePotDevice/CoffeePotDevice.sln" solution in Visual Studio 2015 Community or later.

1. From the Debug toolbar, make sure the target platform is "x86" and the target device is "Local Machine", and start the app:

    ![CoffeePot Targets](images/CoffeePotTargets.png)

1. Click on the "Gear" icon in the top left corner to go to the app's settings

    ![Settings Icon](images/SettingsIcon.png)


1. On the "Iot Hub Settings" tab, paste the `iothubowner` connection string returned from the `prepareMarsIoTHub.sh` script you ran earlier. ***Make sure the entire connection string is on a single line.  If it was wrapped in the output of the script above, edit it so it is all one line.***:

    > **Note**: Recall that you can use the `~/clouddrive/missionmars/showMarsIoTHub.sh` script in the "**Azure Cloud Shell**" as mentioned above to retrieve your values if you no longer have them.

    ```bash
    ================================================================================
    marsiotsea IoT Hub iothubowner connection string - Used by presenters
    --------------------------------------------------------------------------------
    HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=XFoYt5f3xI2ebGxRVpWCfua2++CH+1KIrP5RjRQp6ts=
     ```

    ![IoT Hub Settings](images/IoTHubConnectionString.png)

1. Switch to the "**Manage Devices**" tab, and click the "**Get Devices**" button:

    ![Get Devices](images/GetDevices.png)

1. You should see the "**coffeepot**" device that was created by the `prepareMarsIoTHub.sh` earlier.  Select the "**coffeepot**" device from the list, and click the "**Entangle Device**" button:

    > **Note**: The "**Entangle Device**" button is just saving the device ID and key that the CoffeePotDevice app will use when simulating the coffee pot.  Make sure that whatever device you select here, the attendees are targeting that same device in their code.

    ![Entangle coffeepot](images/EntangleCoffeePotDevice.png)


1. Click the "Home" icon in the top left corner to return to the main screen

    ![Home Icon](images/HomeIcon.png)

1. You can toggle the app between Full Screen and Windowed mode using the full screen toggle button in the top right corner:

    ![Full Screen Toggle](images/FullScreenToggle.png)

1. Leave the app up and running throughout the event so attendees can see their messages pop up. The app has to be in the foreground to run and have the sounds associated with the messages be heard.

---

<a name="testing"></a>

## Testing the CoffeePotDevice app with Device Explorer

You can test the CoffeePotDevice app a number of ways.  

- You could of course just complete the same lab steps that attendees are instructed to do in the <a target="_blank" href="https://github.com/Mission-Mars-Fourth-Horizon-Org/Mission-Briefings/tree/master/IoTHubs">IoT Hubs Mission Briefing</a>.  You should do this at LEAST once for both .NET and Node.js as you prepare for the event.  Completed versions of the .NET and Node.js code are provided in the [CoffeePotDevice](./CoffeePotDevice) folder in this project, and there are steps to walk you through testing with them below.

- You can use the "Device Explorer" tool from the SDK on Windows.

- You can use the iothub-explorer Node.js command line tool. (Not documented here because I was fighting with how to pass the JSON command along from the command line)

### Testing with Device Explorer

The "**Device Explorer**" app from the Azure-IoT SDK provides an convenient set of tools to work with your IoT Hub, devices, and messaging.  It only works on Windows platforms however.

1. Install the Device Explorer using the instructions on the <a target="_blank" href="https://github.com/Azure/azure-iot-sdk-csharp/tree/master/tools/DeviceExplorer">How to use Device Explorer for IoT Hub devices</a> page.

1. Run "**Device Explorer**" on your Windows Computer and on the "**Configuration**" tab, paste the `iothubowner` connection string from above into the "**IoT Hub Connection String**" box.  Then click the "**Update**" button, and then "**Ok**" to confirm the settings where updated.

    ![Device Explorer Configuration](images/DeviceExplorerConnectionInfo.png)

1. Switch to the "**Messages To Device**" tab, select the "**coffeepot**" device, and in the "**Message**" box, paste a valid command string as documented in the lab.  Finally, click the "**Send**" button:

    > **Note**: ***DO NOT CHECK THE "Add Time Stamp" CHECKBOX***.  This will prepend a date/time stamp to the string that is sent to the device, and will invalidate the message.

    ```json
    {"Command":"Ping","Team":"team01","Parameters":"Hello, Mars!"}
    ```

    ![Ping Coffee Pot](images/DeviceExplorerPingCoffeePot.png)

1. Back on the CoffeePotDevice app, you should see your ping message on the screen, as well as it's response.  In addition, you should have heard a distinctive "Ping" sound when the app received the message:

    > **Note**:  ***IF YOU DID NOT HEAR THE SOUND*** ensure that the CoffeePotDevice app was visible on the screen when you sent the message from the "Device Explorer"

    ![Ping Display](images/PingInApp.png)

1. Back in "**Device Explorer**" edit the command to send "Brew" instead, listen for the resulting sound and view the result in the app:

    > **Note**: The most recent messages are displayed at the ***top*** of the list in the CoffeePotDevice app.

    ```json
    {"Command":"Brew","Team":"team01","Parameters":"Hello, Mars!"}
    ```

    ![Brew Command](images/BrewCommandInDeviceExplorer.png)

    ![Brew in App](images/BrewInApp.png)

1. Next, try a command that has the right format, but isn't "Ping" or "Brew" (the only valid commands) and the listen for, and view the response:

    ```json
    {"Command":"Oops","Team":"team01","Parameters":"Hello, Mars!"}
    ```

    ![Oops Command](images/OopsMessageInDeviceExplorer.png)

    ![Oops in App](images/OopsMessageInApp.png)

1. How about a command that doesn't meet the JSON format expected by the CoffeePotDevice app:

    ```json
    Oops
    ```

    ![Malformed in Device Explorer](images/MalformedOopsInDeviceExplorer.png)

    ![Malformed in App](images/MalformedOopsInApp.png)

1. Finally, you can monitor the responses from the CoffeePotDevice App in the "**Device Explorer**" as shown below:

    ![Monitor Responses](images/MonitorResponsesInDeviceExplorer.png)

---

<a name="testingcode"></a>

## Testing with the supplied .net or node.js CoffeeClient code

This repo contains working versions of the .NET and Node.js code attendees create to receive device to cloud messages and send cloud to device messages.  The code to create a device as well as for the simulated device is not supplied here because it isn't used for the coffee pot portion of the lab.

### Test the .NET version of the code

1. From the directory where this repo was cloned, open the ./CoffeeClient/net/CoffeePotClient.sln solution in Visual Studio 2015 Community edition or later.

1. In the "**Solution Explorer**" window, expand the "**ReadDeviceToCloudMessages**" project, and then select the "**ReadDeviceToCloudMessages.cs**" to open it in the editor.  

1. At the top of the "**ReadDeviceToCloudMessages.cs**" code file, locate the following line of code at the top of the `ReadDeviceToCloudMessages` class:

    ```c#
    static string connectionString = "paste-your-coffeeclient-connection-string-here";
    ```

1. Replace the `paste-your-coffeeclient-connection-string-here` place holder with the value for the coffeeclient connection string that was created during the `prepareMarsIoTHub.sh` script above.  For example, using the `coffeeclient` connection string for the `marsiotsea` iot hub above, we would change it to:

    ```c#
    static string connectionString = "HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=1EmQKhYMBdamoriZZM7JNcWSUoEngRlypAdX4Eghc7E=";
    ```

1. Repeat the process for "**SendCloudToDevice**" project's "**SendCloudToDevice.cs**" code file, by updating the line at the top of the `SendCloudToDevice` class that reads:

    ```c#
    static string connectionString = "paste-your-coffeeclient-connection-string-here";
    ```

1. Again, replace the `paste-your-coffeeclient-connection-string-here` place holder with your `coffeeclient` connection string created above.  Once again with our `marsiotsea` iot hub's `coffeeclient` connection string, it would change to:

    ```c#
        static string connectionString = "HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=1EmQKhYMBdamoriZZM7JNcWSUoEngRlypAdX4Eghc7E=";
        ```

1. Next, modify the solution's Startup Projects to start both the **ReadDeviceToCloudMessages** and "**SendCloudToDevice**" projects by right clicking on the "**CoffeePotClient**" solution in the "**Solution Explorer**" window, and selecting "**Set StartUp Projects...**" from the pop-up menu:

    ![Set StartUp Projects](images/SetStartUpProjects.png)

1. The in the "**Solution CoffeePotClient Property Pages**" window, choose:

    - Multiple startup projects
    - ReadeDeviceToCloudMessages - Start
    - SendCloudToDevice - Start

    ![Muultiple Startup Projects](/images/MultipleStartUpProjects.png)

1. Finally, In the "**Solution Explorer**", right-click on the "**CoffeePotClient**" solution and select "**Rebuild Solution**" from the pop-up menu to rebuild the solution.  Then click the "**Start**" button on the debug toolbar to launch the programs.

    ![Rebuild and Debug Solution](images/RebuildAndDebugCoffeeClient.png)

1. In the "**SendCloudToDevice.exe**" window press a key to send a message to the Coffee Pot, and view the response from the CoffeePot in the "**ReadDeviceToCloudMessages.exe**" and "**CoffeePotDevice**"

    ![Test .NET CoffeePotClient](images/RunNetCoffeePotClient.png)

### Test the Node.js version of the code

1. Open a command prompt or terminal window on your system and change to the "**/CoffeePotClient/node/readdevicetocloudmessages**" folder under the folder where you cloned this repo.

1. Open the "**ReadDeviceToCloudMessages.cs" code file in the editor of your choice (Like Visual Studio Code), and locate the following line of code near the top of the file.

    ```js
    var connectionString = 'paste-your-coffeeclient-connection-string-here';
    ```
1. Replace the `paste-your-coffeeclient-connection-string-here` place holder with the "**coffeeclient**" connection string that was created during the `prepareMarsIoTHub.sh` script execution previously.  For example, using the `coffeeclient` connection string for the `marsiotsea` iot hub above, we would change it to:

    ```js
     var connectionString = 'HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=SB4pL2sYG95iXFOlYOtYO23sIQieKtk/7jiU1X5T4ng=';
    ```

1. Back in your command prompt or terminal window, ensure that you are in the "**./CoffeeClient/node/readdevicetocloudmessages**" folder and install the required Node.js modules by running:

    ```bash
    npm install
    ```

1. Finally, run the "**ReadDeviceToCloudMessages.js**" program by running:

    | **Note**: The program won't do anything until we send a message later.  Just keep it running in this terminal window for now.

    ```bash
    node .\ReadDeviceToCloudMessages.js
    ```

    | **Note**: There seems to currently be a bug in the underlying amqp libraries that may  cause the program to crash immediately with the error shown below.  If this occurs, simply re-run it until it works.

    ```bash
    readdevicetocloudmessages/node_modules/amqp10/lib/frames.js:56
        stream.write(buffer, callback);
    ^

    TypeError: Cannot read property 'write' of null
    ...
    ```



1. Keeping the command prompt or terminal window with the "**ReadDeviceToCloudMessages.js**" program running in it, open a second terminal window or command prompt window and navigate to the "**./CoffeeClient/node/sendcloudtodevicemessage**" folder.

1. Open the "**./CoffeeClient/node/sendcloudtodevicemessage/SendCloudToDeviceMessage.js**" file in your editor, and locate the following line of code:

    ```js
    var connectionString = 'paste-your-coffeeclient-connection-string-here';
    ```

1. As before, replace the `paste-your-coffeeclient-connection-string-here` place holder with your `coffeeclient` connection string.  Again using our `marsiotsea` `coffeeclient` connection string as an example:

    ```js
    var connectionString = 'HostName=marsiotsea.azure-devices.net;SharedAccessKeyName=coffeeclient;SharedAccessKey=SB4pL2sYG95iXFOlYOtYO23sIQieKtk/7jiU1X5T4ng=';
    ```
1. Back in your command prompt or terminal window, change into the "**./CoffeeClient/node/sendcloudtodevicemessage**" folder, and install the Node.js dependencies with:

    ```bash
    npm install
    ```
1. Make sure the CoffeePotDevice UWP app is running and "**entangled**" with your `coffeepot` device as documented above.

1. In the Command prompt or terminal window pointing at the "**./CoffeeClient/node/sendcloudtodevicemessage**" folder, run the "**SendCloudToDeviceMessage.js**" program by running the following:

    | **Note**:  The program only sends a single message.  To send another you need to exit the program with a **Ctrl-C** and run it again.

    ```bash
    node .\SendCloudToDeviceMessage.js
    ```
1. You can monitor the sent message the the **CoffeePotDevice** app as well as in the terminal window with the "**ReadDeviceToCloudMessages.js**" program running.

    ![Run Node Coffee Client](images/RunNodeCoffeeClient.png)

---

<a name="teamcards"></a>

## Prepare the Team Cards

The attendees need to know the team name or number they are so they can use the correct event hub consumer group when reading devices from the iot hub. The "teamxx" consumer groups were created on the iot hub during of the `prepareMarsIoTHub.sh` script execution above.

1. Download the two PDF files from the repo:

    - [TeamCards1to10.pdf](TeamCards/TeamCards1to10.pdf)
    - [TeamCards11to20.pdf](TeamCards/TeamCards11to20.pdf)

    ![Team Cards Pages](images/TeamCardsPages.png)

1. Print the pages out, and cut out the individual cards to distribute to the teams during the event.  

    ![Team01 Card Sample](TeamCards/cardsample.png)

1. Make sure that attendees form into ***NO MORE*** than twenty teams, and that they modify the code in the "ReadDeviceToCloudMessages" program to use the "teamxx" consumer group as documented in the Mission Briefing.

---

<a name="cleanup"></a>

## Clean up after the event

Once the event is over you do not need to retain the Azure IoT Hub. The SKU used (S1) by the script currently costs $50/month to run.  No problem for a day or two, but could start to impact your available credits on a long term basis.

To clean up your Azure IoT Hub it's surprisingly simple.

1. From a command prompt where the "Azure CLI 2.0" is in the path run the following command, and enter `y` to confirm the operation:

    > **Note**: You could of course also just log in to the portal and delete the resource group from there.  Also, sometimes there is an error after the `az group delete --name <resource-group-name>` command.  If you get one, first verify that you have the right name, and then run the command again.  It usually works the second time if it errored the first time.

    ```bash
    az group delete --name <resource-group-name>
    ```

    For example, with our `marsgroupsea` name from above:

    ```bash
    az group delete --name marsgroupsea
    ```



