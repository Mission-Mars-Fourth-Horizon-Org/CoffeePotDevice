using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace CoffeePotDevice.Models
{
  public class DeviceEntity : BindableBase, IComparable<DeviceEntity>
  {
    private string id;
    public string Id
    {
      get { return id; }
      set { Set(ref id, value); }
    }

    private string primaryKey;
    public string PrimaryKey
    {
      get { return primaryKey; }
      set { Set(ref primaryKey, value); }
    }

    private string secondaryKey;
    public string SecondaryKey
    {
      get { return secondaryKey; }
      set { Set(ref secondaryKey, value); }
    }

    private string primaryThumbPrint;
    public string PrimaryThumbPrint
    {
      get { return primaryThumbPrint; }
      set { Set(ref primaryThumbPrint, value); }
    }

    private string secondaryThumbPrint;
    public string SecondaryThumbPrint
    {
      get { return secondaryThumbPrint; }
      set { Set(ref secondaryThumbPrint, value); }
    }

    private string connectionString;
    public string ConnectionString
    {
      get { return connectionString; }
      set { Set(ref connectionString,value); }
    }

    private string connectionState;
    public string ConnectionState
    {
      get { return connectionState; }
      set { Set(ref connectionState, value); }
    }

    private DateTime lastActivityTime;
    public DateTime LastActivityTime
    {
      get { return lastActivityTime; }
      set { Set(ref lastActivityTime, value); }
    }

    private DateTime lastConnectionStateUpdatedTime;
    public DateTime LastConnectionStateUpdatedTime
    {
      get { return lastConnectionStateUpdatedTime; }
      set { Set(ref lastConnectionStateUpdatedTime, value); }
    }

    private DateTime lastStateUpdatedTime;
    public DateTime LastStateUpdatedTime
    {
      get { return lastStateUpdatedTime; }
      set { Set(ref lastStateUpdatedTime, value); }
    }


    private int messageCount;

    public int MessageCount
    {
      get { return messageCount; }
      set { Set(ref messageCount, value); }
    }

    private string state;
    public string State
    {
      get { return state; }
      set { Set(ref state, value); }
    }

    private string suspensionReason;
    public string SuspensionReason
    {
      get { return suspensionReason; }
      set { Set(ref suspensionReason, value); }
    }

    public int CompareTo(DeviceEntity other)
    {
      return string.Compare(this.Id, other.Id, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
      return $"Device ID = {this.Id}, Primary Key = {this.PrimaryKey}, Secondary Key = {this.SecondaryKey}, Primary Thumbprint = {this.PrimaryThumbPrint}, Secondary Thumbprint = {this.SecondaryThumbPrint}, ConnectionString = {this.ConnectionString}, ConnState = {this.ConnectionState}, ActivityTime = {this.LastActivityTime}, LastConnState = {this.LastConnectionStateUpdatedTime}, LastStateUpdatedTime = {this.LastStateUpdatedTime}, MessageCount = {this.MessageCount}, State = {this.State}, SuspensionReason = {this.SuspensionReason}\r\n";
    }

    public DeviceEntity Copy()
    {
      DeviceEntity copy = new Models.DeviceEntity();
      copy.Id = this.Id;
      copy.ConnectionState = this.ConnectionState;
      copy.ConnectionString = this.ConnectionString;
      copy.LastActivityTime = this.LastActivityTime;
      copy.LastConnectionStateUpdatedTime = this.LastConnectionStateUpdatedTime;
      copy.LastStateUpdatedTime = this.LastStateUpdatedTime;
      copy.MessageCount = this.MessageCount;
      copy.PrimaryKey = this.PrimaryKey;
      copy.PrimaryThumbPrint = this.PrimaryThumbPrint;
      copy.SecondaryKey = this.SecondaryKey;
      copy.SecondaryThumbPrint = this.SecondaryThumbPrint;
      copy.State = this.State;
      copy.SuspensionReason = this.SuspensionReason;

      return copy;
    }
  }
}
