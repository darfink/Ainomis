namespace Ainomis.Game.Components {
  using Ainomis.Game.Resource;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  class TilesetComponent : Tileset, IComponent {
    /// <summary>
    /// Gets the source.
    /// </summary>
    /// <value>The source.</value>
    public Rectangle Source => this.GetSourceRectangleForTile(TileIndex);

    /// <summary>
    /// Gets or sets the current tile.
    /// </summary>
    /// <value>The current tile.</value>
    public uint TileIndex { get; set; }
  }
}
