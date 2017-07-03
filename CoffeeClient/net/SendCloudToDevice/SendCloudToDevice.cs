using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace SendCloudToDevice
{
  class SendCloudToDevice
  {

    static ServiceClient serviceClient;
    static string connectionString = "paste-your-coffeeclient-connection-string-here";
    static string deviceId = "coffeepot";

    private async static Task SendCloudToDeviceMessageAsync()
    {
      //string message = "Cloud to device message.";
      //string message = "Cloud to device message.";
      string message = "{\"Command\":\"Brew\",\"Team\":\"team01\",\"Parameters\":\"\"}";
      var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
      await serviceClient.SendAsync(deviceId, commandMessage);
    }

    static void Main(string[] args)
    {
      Console.WriteLine("Send Cloud-to-Device message\n");
      serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

      while (true)
      {
        Console.Clear();
        Console.WriteLine("Press any key to send a C2D message.");
        Console.WriteLine("Press Q to quit");
        ConsoleKeyInfo key = Console.ReadKey();
        if (key.KeyChar.ToString().ToLower().Trim()=="q")
        {
          break;
        }
        else
        {
          SendCloudToDeviceMessageAsync().Wait();
        }
      }
    }
  }
}
