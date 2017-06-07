namespace Ainomis.Game.States.Explore.Map {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;
  using Ainomis.Game.Resources;
  using Ainomis.Shared;

  using Microsoft.Xna.Framework;

  using Common = Ainomis.Shared.Common;

  internal class MapController : IMapController {
    private List<TileType> _mapLayer;
    private Map _map;

    /// <summary>Constructs a new map controller.</summary>
    public MapController(Map map) {
      _map = map.ThrowIfNull(nameof(map));

      var tileset = map.Tilesets.Single(t => t.Properties.IsMeta);
      var tileTypes = tileset.Tiles
        .OrderBy(t => t.Key)
        .Select(t => t.Value.Type)
        .ToList();

      _mapLayer = map.Layers
        .Single(l => l.Properties.IsMeta).Data
        .Select(i => tileTypes[(int)(i - tileset.FirstGid)])
        .ToList();
    }

    /// <inheritdoc />
    public Point GetTileOffset(uint index) => _map.GetTileOffset(index);

    /// <inheritdoc />
    public TileType GetTileType(uint index) => _mapLayer[(int)index];

    /// <inheritdoc />
    public IEnumerable<uint> GetTilesInDirection(uint index, Direction direction) =>
      _map.GetTilesInDirection(index, direction);
  }
}
