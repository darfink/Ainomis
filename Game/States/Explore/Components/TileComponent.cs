namespace Ainomis.Game.States.Explore.Components {
  using Ainomis.Extensions;
  using Ainomis.Game.Resources;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  /// <summary>Describes all possible states for a tile component.</summary>
  internal enum TileState {
    Idling,
    Moving,
    Running,
    Fishing,
  }

  /// <summary>Tile component.</summary>
  internal class TileComponent : IComponent {
    /// <summary>Constructs a new tile component.</summary>
    public TileComponent(Area area, uint tile) {
      Area = area.ThrowIfNull(nameof(area));
      Index = tile;
    }

    /// <summary>Gets or sets the entity's current state.</summary>
    public TileState State { get; set; }

    /// <summary>Gets or sets the tile the entity is located at.</summary>
    public uint Index { get; set; }

    /// <summary>Gets or sets the area the entity is located in.</summary>
    public Area Area { get; set; }

    /// <summary>Gets or sets the entity's direction.</summary>
    public Direction Direction { get; set; }
  }
}
