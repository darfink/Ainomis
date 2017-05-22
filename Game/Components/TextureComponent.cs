namespace Ainomis.Game.Components {
  using Artemis.Interface;

  using Microsoft.Xna.Framework.Graphics;

  public class TextureComponent : IComponent {
    /// <summary>
    /// Initializes a new texture component.
    /// </summary>
    /// <param name="texture">Texture.</param>
    public TextureComponent(Texture2D texture = null) {
      this.Texture = texture;
    }

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public Texture2D Texture { get; set; }
  }
}
