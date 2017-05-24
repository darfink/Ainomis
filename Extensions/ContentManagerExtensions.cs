namespace Ainomis.Extensions {
  using Microsoft.Xna.Framework.Content;

  internal static class ContentManagerExtensions {
    /// <summary>
    /// Creates a new content manager with identical settings.
    /// </summary>
    public static ContentManager Allocate(this ContentManager content) =>
      new ContentManager(content.ServiceProvider, content.RootDirectory);
  }
}
