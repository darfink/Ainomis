namespace Ainomis.Shared.Common {
  using Microsoft.Xna.Framework;

  /// <summary>
  /// Represents a updateable object
  /// </summary>
  public interface IUpdateable {
    void Update(GameTime gameTime);
  }
}
