using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using CoffeePotDevice.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.SerializationService;
using Template10.Mvvm;

namespace CoffeePotDevice.Services
{
  public class IoTHubDeviceService : BindableBase
  {

    public IoTHubDeviceService(string DeviceConnectionString)
    {
      this.DeviceConnectionString = DeviceConnectionString;
    }

    public event EventHandler<PingCommandEventArgs> PingCommandReceived;
    public event EventHandler<BrewCommandEventArgs> BrewCommandReceived;
    //public event EventHandler<TakePictureCommandEventArgs> TakePictureCommandReceived;
    //public event EventHandler<AnswerCommandEventArgs> AnswerCommandReceived;
    public event EventHandler<CommandEventArgs> UnknownCommandReceived;
    public event EventHandler<MessageEventArgs> BadMessageReceived;
    public event EventHandler<MessageEventArgs> MessageReceived;

    private string deviceConnectionString;

    public string DeviceConnectionString
    {
      get { return deviceConnectionString; }
      set
      {
        bool wasListening = IsListening;
        if (wasListening)
        {
          StopListening();
          deviceClient = null;
        }
        Set(ref deviceConnectionString, value);
        if(wasListening)
        {
          StartListening();
        }
      }
    }

    private bool isListening = false;

    public bool IsListening
    {
      get { return isListening; }
      set { Set(ref isListening, value); }
    }


    //static string iotHubUri = "marsiot.azure-devices.net";
    //static string deviceId = "coffeepot";
    //static string deviceKey = "YcsP7DdIFde4s1QKd4xJgxpo0w0ipJo7wybZVWM/p4I=";

    private DeviceClient deviceClient;

    public async Task StartListening()
    {
      IsListening = true;
      ReceiveCloudToDeviceMessagesAsync();
      await Task.CompletedTask;
    }

    public async Task StopListening()
    {
      IsListening = false;
      await Task.CompletedTask;
    }

    private void EnsureDeviceClient()
    {
      deviceClient = deviceClient ?? DeviceClient.CreateFromConnectionString(DeviceConnectionString);
    }

    public async Task SendDeviceToCloudMessagesAsync(object MessageData, string correlationId)
    {
      EnsureDeviceClient();

      var messageString = JsonConvert.SerializeObject(MessageData);
      var message = new Message(Encoding.ASCII.GetBytes(messageString));
      message.CorrelationId = correlationId;
      Debug.WriteLine(String.Format("correlation-id: {0}", correlationId));

      Debug.WriteLine(">>>>> {0} - Sending message: {1}", DateTime.Now, messageString);
      await deviceClient.SendEventAsync(message);
    }

    public async Task CompleteCloudToDeviceMessageAsync(Message Message)
    {
      EnsureDeviceClient();
      await deviceClient.CompleteAsync(Message);
    }

    private async void ReceiveCloudToDeviceMessagesAsync()
    {
      Debug.WriteLine("\nReceiving cloud to device messages from service");

      EnsureDeviceClient();

      while (IsListening)
      {
        await Task.Delay(1000);
        Message receivedMessage = await deviceClient.ReceiveAsync();
        if (receivedMessage == null) continue;

        var messageBytes = receivedMessage.GetBytes();
        var messageString = Encoding.ASCII.GetString(messageBytes);

        //Fire the general message received event
        NotifyMessageReceived(receivedMessage, messageBytes, messageString);

        //Attempt to parse it into a CommandMessage
        CloudToDeviceMessage commandMessage = null;
        try
        {
          commandMessage = JsonConvert.DeserializeObject<CloudToDeviceMessage>(messageString);
        }
        catch
        {

        }

        //if (serializationService.TryDeserialize<CommandMessage>(messageString, out commandMessage))
        if (commandMessage != null)
        {
          string commandString = commandMessage.Command.Trim().ToLower();
          string payload = commandMessage.Parameters;
          switch (commandString)
          {
            case "ping":
              PingCommandReceived?.Invoke(null, new PingCommandEventArgs(receivedMessage, messageBytes, messageString, commandMessage, payload));
              break;
            case "brew":
              BrewCommandReceived?.Invoke(null, new BrewCommandEventArgs(receivedMessage, messageBytes, messageString, commandMessage, payload));
              break;
            //case "takepicture":
            //  int camera;
            //  if(int.TryParse(commandMessage.Parameters,out camera))
            //  {
            //    TakePictureCommandReceived?.Invoke(null, new TakePictureCommandEventArgs(receivedMessage, messageBytes, messageString, commandMessage, camera));
            //  }
            //  else
            //  {
            //    NotifyBadMessageReceived(receivedMessage, messageBytes, messageString);
            //  }
            //  break;
            //case "answer":
            //  string answer = commandMessage.Parameters.Trim();
            //  if(!String.IsNullOrWhiteSpace(answer))
            //  {
            //    AnswerCommandReceived?.Invoke(null, new AnswerCommandEventArgs(receivedMessage, messageBytes, messageString, commandMessage, answer));
            //  }
            //  else
            //  {
            //    NotifyBadMessageReceived(receivedMessage, messageBytes, messageString);
            //  }
            //  break;

            default:
              // Send a generic event about the message being received
              //NotifyMessageReceived(receivedMessage, messageBytes, messageString);
              UnknownCommandReceived?.Invoke(null, new CommandEventArgs(receivedMessage, messageBytes, messageString, commandMessage, payload));
              break;
          }
        } else
        {
          NotifyBadMessageReceived(receivedMessage, messageBytes, messageString);
        }

        // Go ahead and "complete" every message, even if we don't know we've processed it
        // We'll take the "UDP" approach for now. Consider doing this only after you know a message
        // has been successfully processed in the future...
        await deviceClient.CompleteAsync(receivedMessage);
      }
    }


    void NotifyBadMessageReceived(Message Message, byte[] MessageBytes, string MessageString)
    {
      BadMessageReceived?.Invoke(null, new MessageEventArgs(Message, MessageBytes, MessageString));
    }

    void NotifyMessageReceived(Message Message, byte[] MessageBytes, string MessageString)
    {
      MessageReceived?.Invoke(null, new MessageEventArgs(Message,MessageBytes,MessageString));
    }

  }
}
