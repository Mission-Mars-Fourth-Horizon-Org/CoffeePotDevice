using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeePotDevice.Models
{
  public class CloudToDeviceMessage
  {
    [JsonProperty(Required = Required.Always)]
    public string Command { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Team { get; set; }

    [JsonProperty(Required = Required.Always)]
    public string Parameters { get; set; }
  }
}
