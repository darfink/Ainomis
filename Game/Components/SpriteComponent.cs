namespace Ainomis.Game.Components {
  using Artemis.Interface;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Sprite component.
  /// </summary>
  public class SpriteComponent : IComponent {
    /// <summary>
    /// Gets or sets the source.
    /// </summary>
    /// <value>The source.</value>
    public Rectangle? Source { get; set; }

    /// <summary>
    /// Gets or sets the mirroring.
    /// </summary>
    /// <value>The mirroring.</value>
    public SpriteEffects Mirroring { get; set; }

    /// <summary>
    /// Gets or sets the origin.
    /// </summary>
    /// <value>The origin.</value>
    public Vector2 Origin { get; set; }

    /// <summary>
    /// Gets or sets the scale.
    /// </summary>
    /// <value>The scale.</value>
    public Vector2 Scale { get; set; } = Vector2.One;

    /// <summary>
    /// Gets or sets the offset.
    /// </summary>
    /// <value>The offset.</value>
    public Vector2 Offset { get; set; }

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    /// <value>The color.</value>
    public Color Color { get; set; } = Color.White;

    /// <summary>
    /// Gets or sets the layer.
    /// </summary>
    /// <value>The layer.</value>
    public float Layer { get; set; }
  }
}
