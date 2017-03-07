using System;
using Template10.Services.SerializationService;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common;
using Microsoft.Azure.Devices.Common.Exceptions;
using System.Collections.Generic;
using CoffeePotDevice.Models;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using System.ComponentModel;

namespace CoffeePotDevice.Services
{
  public static class IoTHubRegistryService
  {

    public static String GenerateDeviceId(string IdPrefix)
    {
      return $"{IdPrefix}{Guid.NewGuid().ToString("N")}";
    }

    public static string GenerateSymmetricKey()
    {
      return CryptoKeyGenerator.GenerateKey(32);
    }

    public static DeviceEntity CreateNewDeviceEntity(bool generateDefaults = false, string IdPrefix = "")
    {
      DeviceEntity device = new Models.DeviceEntity();
      if (generateDefaults)
      {
        device.Id = GenerateDeviceId(IdPrefix);
        device.PrimaryKey = GenerateSymmetricKey();
        device.SecondaryKey = GenerateSymmetricKey();
      }
      return device;
    }

    public static Device GetDeviceFromDeviceEntity(DeviceEntity deviceEntity)
    {
      Device device = null;

      if (deviceEntity != null)
      {
        device = new Device(deviceEntity.Id);
        device.Authentication = new AuthenticationMechanism();
        device.Authentication.SymmetricKey.PrimaryKey = deviceEntity.PrimaryKey;
        device.Authentication.SymmetricKey.SecondaryKey = deviceEntity.SecondaryKey;
        device.Authentication.X509Thumbprint.PrimaryThumbprint = deviceEntity.PrimaryThumbPrint;
        device.Authentication.X509Thumbprint.SecondaryThumbprint = deviceEntity.SecondaryThumbPrint;
      }

      return device;
    }

    public static DeviceEntity GetDeviceEntityFromDevice(String iotHubManagementConnectionString, string protocolGatewayHostName, Device device)
    {
      DeviceEntity deviceEntity = null;

      if (device != null)
      {

        deviceEntity = new DeviceEntity()
        {
          Id = device.Id,
          ConnectionState = device.ConnectionState.ToString(),
          ConnectionString = CreateDeviceConnectionString(iotHubManagementConnectionString, protocolGatewayHostName, device),
          LastActivityTime = device.LastActivityTime,
          LastConnectionStateUpdatedTime = device.ConnectionStateUpdatedTime,
          LastStateUpdatedTime = device.StatusUpdatedTime,
          MessageCount = device.CloudToDeviceMessageCount,
          State = device.Status.ToString(),
          SuspensionReason = device.StatusReason
        };

        if (device.Authentication != null)
        {

          deviceEntity.PrimaryKey = device.Authentication.SymmetricKey?.PrimaryKey;
          deviceEntity.SecondaryKey = device.Authentication.SymmetricKey?.SecondaryKey;
          deviceEntity.PrimaryThumbPrint = device.Authentication.X509Thumbprint?.PrimaryThumbprint;
          deviceEntity.SecondaryThumbPrint = device.Authentication.X509Thumbprint?.SecondaryThumbprint;
        }
      }

      return deviceEntity;
    }

    public async static Task DeleteDeviceEntityFromRegistry(String iotHubManagementConnectionString, DeviceEntity deviceEntity)
    {
      RegistryManager registry = RegistryManager.CreateFromConnectionString(iotHubManagementConnectionString);
      try
      {
        Device inputDevice = GetDeviceFromDeviceEntity(deviceEntity);
        await registry.RemoveDeviceAsync(inputDevice.Id);
      }
      catch (Exception ex)
      {
        throw new Exception($"An {ex.GetType().Name} occurred when Deleting the Device from the Registry based on the provided DeviceEntity: {ex.Message}", ex);
      }
    }

    public async static Task<DeviceEntity> SaveDeviceEntityToRegistry(bool isInsert, String iotHubManagementConnectionString, string protocolGatewayHostName, DeviceEntity deviceEntity)
    {
      RegistryManager registry;
      DeviceEntity outputDeviceEntity = null;
      try
      {
        Device inputDevice = GetDeviceFromDeviceEntity(deviceEntity);
        Device outputDevice;
        registry = RegistryManager.CreateFromConnectionString(iotHubManagementConnectionString);
        if (isInsert)
        {
          outputDevice = await registry.AddDeviceAsync(inputDevice);
        }
        else
        {
          outputDevice = await registry.UpdateDeviceAsync(inputDevice, true);
        }
        outputDeviceEntity = GetDeviceEntityFromDevice(iotHubManagementConnectionString, protocolGatewayHostName, outputDevice);
      }
      catch (Exception ex)
      {
        throw new Exception($"An {ex.GetType().Name} occurred when Adding the Device to the Registry based on the provided DeviceEntity: {ex.Message}", ex);
      }

      return outputDeviceEntity;
    }

    public static async Task<List<DeviceEntity>> GetDevices(string iotHubManagementConnectionString, string protocolGatewayHostName)
    {
      List<DeviceEntity> listOfDevices = null;

      try
      {
        RegistryManager registry = RegistryManager.CreateFromConnectionString(iotHubManagementConnectionString);

        var devices = await registry.GetDevicesAsync(100);

        if (devices != null)
        {

          listOfDevices = new List<Models.DeviceEntity>();

          foreach (var device in devices)
          {
            listOfDevices.Add(GetDeviceEntityFromDevice(iotHubManagementConnectionString, protocolGatewayHostName, device));
          }

        }

      }
      catch (Exception ex)
      {
        throw ex;
      }

      return listOfDevices;
    }

    private static string CreateDeviceConnectionString(String iotHubManagementConnectionString, string protocolGatewayHostName, Device device)
    {
      StringBuilder deviceConnectionString = new StringBuilder();

      var hostName = String.Empty;
      var tokenArray = iotHubManagementConnectionString.Split(';');
      for (int i = 0; i < tokenArray.Length; i++)
      {
        var keyValueArray = tokenArray[i].Split('=');
        if (keyValueArray[0] == "HostName")
        {
          hostName = tokenArray[i] + ';';
          break;
        }
      }

      if (!String.IsNullOrWhiteSpace(hostName))
      {
        deviceConnectionString.Append(hostName);
        deviceConnectionString.AppendFormat("DeviceId={0}", device.Id);

        if (device.Authentication != null)
        {
          if ((device.Authentication.SymmetricKey != null) && (device.Authentication.SymmetricKey.PrimaryKey != null))
          {
            deviceConnectionString.AppendFormat(";SharedAccessKey={0}", device.Authentication.SymmetricKey.PrimaryKey);
          }
          else
          {
            deviceConnectionString.AppendFormat(";x509=true");
          }
        }

        if (!String.IsNullOrWhiteSpace(protocolGatewayHostName))
        {
          deviceConnectionString.AppendFormat(";GatewayHostName=ssl://{0}:8883", protocolGatewayHostName);
        }
      }

      return deviceConnectionString.ToString();
    }


  }
}
