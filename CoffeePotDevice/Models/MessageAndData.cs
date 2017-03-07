using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeePotDevice.Models
{
  class MessageAndData
  {
    public Message Message { get; set; }
    public byte[] MessageBytes { get; set; }
    public string MessageString { get; set; }
  }
}
