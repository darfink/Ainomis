namespace Ainomis.Game.Components {
  using Ainomis.Extensions;

  using Artemis.Interface;

  using Microsoft.Xna.Framework.Graphics;

  public class TextureComponent : IComponent {
    private Texture2D _texture;

    /// <summary>
    /// Initializes a new texture component.
    /// </summary>
    /// <param name="texture">Texture.</param>
    public TextureComponent(Texture2D texture) => this.Texture = texture;

    /// <summary>
    /// Gets or sets the image.
    /// </summary>
    /// <value>The image.</value>
    public Texture2D Texture {
      get => _texture;
      set => _texture = value.ThrowIfNull(nameof(value));
    }
  }
}
