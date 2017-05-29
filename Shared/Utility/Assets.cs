namespace Ainomis.Shared.Utility {
  using System;
  using System.IO;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  using Newtonsoft.Json;
  using Newtonsoft.Json.Serialization;

  /// <summary>
  /// System agnostic (PCL compliant) file data retriever
  /// </summary>
  internal static class Assets {
    /// <summary>
    /// Loads a file's content as string.
    /// </summary>
    /// <returns>All content.</returns>
    /// <param name="path">Path.</param>
    public static string LoadString(string path) {
      string resourcePath = GetResourcePath(path);

      string data;
      using(var reader = new StreamReader(TitleContainer.OpenStream(resourcePath))) {
        data = reader.ReadToEnd();
      }

      return data;
    }

    /// <summary>
    /// Loads a 2D texture from path.
    /// </summary>
    /// <returns>A 2D texture.</returns>
    /// <param name="device">Graphics device.</param>
    /// <param name="path">Path.</param>
    public static Texture2D LoadTexture(GraphicsDevice device, string path) {
      string resourcePath = GetResourcePath(path);

      Texture2D data;
      using (var reader = TitleContainer.OpenStream(resourcePath)) {
        data = Texture2D.FromStream(device, reader);
      }

      return data;
    }

    /// <summary>
    /// Loads JSON data from path.
    /// </summary>
    /// <returns>An object representing the JSON.</returns>
    /// <param name="path">Path.</param>
    public static T LoadJson<T>(string path) {
      var settings = new JsonSerializerSettings() {
        ContractResolver = new XnaFriendlyResolver(),
      };

      return JsonConvert.DeserializeObject<T>(LoadString(path), settings);
    }

    private static string GetResourcePath(string path) => Path.Combine(Settings.ResourcePrefix, path);

    private class XnaFriendlyResolver : DefaultContractResolver {
      protected override JsonContract CreateContract(Type objectType) {
        if (objectType == typeof(Rectangle)) {
          return CreateObjectContract(objectType);
        }

        return base.CreateContract(objectType);
      }
    }
  }
}
