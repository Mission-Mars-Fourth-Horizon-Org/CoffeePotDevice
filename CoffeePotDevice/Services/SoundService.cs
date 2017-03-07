using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace CoffeePotDevice.Services
{
  public static class SoundService
  {
    public static async Task PlayAudioFileAsync(String Path)
    {
      string soundFilePath = String.Format("ms-appx:///Assets/Sounds/{0}", Path);
      var soundFile = await StorageFile.GetFileFromApplicationUriAsync(
           new Uri(soundFilePath));
      await PlayAudioAsync(soundFile);
    }

    public static async Task PlayAudioAsync(IStorageFile mediaFile, bool looping = false)
    {
      var stream = await mediaFile.OpenAsync(FileAccessMode.Read).AsTask();
      var mediaControl = new MediaElement() { IsLooping = looping };
      mediaControl.SetSource(stream, mediaFile.ContentType);
      mediaControl.Volume = .5;
      mediaControl.Play();
    }
  }
}
