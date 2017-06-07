namespace Ainomis.Game.States.Explore.Map {
  using System.Collections.Generic;

  using Ainomis.Game.Resources;

  using Microsoft.Xna.Framework;

  interface IMapController {
    /// <summary>Returns a tile's offset in pixels.</summary>
    Point GetTileOffset(uint index);

    /// <summary>Returns a tile's type.</summary>
    TileType GetTileType(uint index);

    /// <summary>Returns adjacent tiles in a specified direction.</summary>
    IEnumerable<uint> GetTilesInDirection(uint index, Direction direction);
  }
}
