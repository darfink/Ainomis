namespace Ainomis.Game.Components {
  using Ainomis.Extensions;
  using Ainomis.Game.Resources;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  internal class TilesetComponent : Tileset, IComponent {
    /// <summary>Creates an empty component.</summary>
    public TilesetComponent() {
    }

    /// <summary>Creates a new component from a tileset.</summary>
    public TilesetComponent(Tileset tileset) {
      this.AssignFrom(tileset);
      TileId = FirstGid;
    }

    /// <summary>Gets the source.</summary>
    /// <value>The source.</value>
    public Rectangle Source => this.GetSourceRectangleForTile(TileId);

    /// <summary>Gets or sets the current tile.</summary>
    /// <value>The current tile.</value>
    public uint TileId { get; set; }
  }
}
