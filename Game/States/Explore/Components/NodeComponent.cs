namespace Ainomis.Game.States.Explore.Components {
  using Ainomis.Extensions;
  using Ainomis.Game.Resources;
  using Ainomis.Game.States.Explore.Map;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  /// <summary>Describes all possible states for a node component.</summary>
  internal enum TileState {
    Idling,
    Walking,
    Running,
    Fishing,
  }

  internal static class TileStateHelper {
    public static bool IsMoving(this TileState state) =>
      state == TileState.Walking || state == TileState.Running;
  }

  /// <summary>Node component.</summary>
  internal class NodeComponent : IComponent {
    /// <summary>Constructs a new node component.</summary>
    public NodeComponent(IMapController map, uint tile) {
      Map = map.ThrowIfNull(nameof(map));
      Index = tile;
    }

    /// <summary>Gets or sets the entity's current state.</summary>
    public TileState State { get; set; }

    /// <summary>Gets or sets the tile the entity is located at.</summary>
    public uint Index { get; set; }

    /// <summary>Gets or sets the map the entity is located in.</summary>
    public IMapController Map { get; set; }

    /// <summary>Gets or sets the entity's direction.</summary>
    public Direction Direction { get; set; }
  }
}
