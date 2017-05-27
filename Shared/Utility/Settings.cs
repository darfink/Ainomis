namespace Ainomis.Shared.Utility {
  using System.Collections.Generic;
  using System.IO;

  using Plugin.Settings;
  using Plugin.Settings.Abstractions;

  internal static class Settings {
    private static readonly ISettings AppSettings = new SettingsImplementation();

    // Avoid mixing app files from game resources by using a prefix
    private static readonly KeyValuePair<string, string> ResourcePrefixKV =
      new KeyValuePair<string, string>("ResourcePrefix",  "Content");

    // The virtual resolution width and height is the same resolution as the
    // iPhone 7 (retina) device screen when positioned in landscape mode.
    private static readonly KeyValuePair<string, int> VirtualResolutionWidthKV = new KeyValuePair<string, int>("VirtualResolutionWidth",  1334);
    private static readonly KeyValuePair<string, int> VirtualResolutionHeightKV = new KeyValuePair<string, int>("VirtualResolutionHeight",  750);

    private static readonly KeyValuePair<string, float> MusicVolumeKV =
      new KeyValuePair<string, float>("MusicVolume", 0f);

    private static readonly KeyValuePair<string, bool> FullscreenKV =
      new KeyValuePair<string, bool>("Fullscreen",  false);

    /// <summary>Gets or sets the resource file path.</summary>
    public static string ResourcePrefix {
      get => Path.Combine(
        PlatformResourcePrefix,
        AppSettings.GetValueOrDefault<string>(ResourcePrefixKV.Key, ResourcePrefixKV.Value));

      set => AppSettings.AddOrUpdateValue<string>(ResourcePrefixKV.Key, value);
    }

    /// <summary>Gets or sets the music volume.</summary>
    public static float MusicVolume {
      get => AppSettings.GetValueOrDefault<float>(MusicVolumeKV.Key, MusicVolumeKV.Value);
      set => AppSettings.AddOrUpdateValue<float>(MusicVolumeKV.Key, value);
    }

    /// <summary>Gets or sets whether fullscreen is enabled or not.</summary>
    public static bool Fullscreen {
      get => AppSettings.GetValueOrDefault<bool>(FullscreenKV.Key, FullscreenKV.Value);
      set => AppSettings.AddOrUpdateValue<bool>(FullscreenKV.Key, value);
    }

    /// <summary>Gets or sets the virtual resolution width.</summary>
    public static uint VirtualResolutionWidth {
      get => (uint)AppSettings.GetValueOrDefault<int>(VirtualResolutionWidthKV.Key, VirtualResolutionWidthKV.Value);
      set => AppSettings.AddOrUpdateValue<int>(VirtualResolutionWidthKV.Key, (int)value);
    }

    /// <summary>Gets or sets the virtual resolution height.</summary>
    public static uint VirtualResolutionHeight {
      get => (uint)AppSettings.GetValueOrDefault<int>(VirtualResolutionHeightKV.Key, VirtualResolutionHeightKV.Value);
      set => AppSettings.AddOrUpdateValue<int>(VirtualResolutionHeightKV.Key, (int)value);
    }

    /// <summary>Gets or sets an optional platform prefix for resources.</summary>
    public static string PlatformResourcePrefix { get; set; } = string.Empty;
  }
}
