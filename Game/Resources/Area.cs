namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;

  internal class Area {
    public uint Width { get; set; }

    public uint Height { get; set; }

    public uint Size => Width * Height;

    public uint TileWidth { get; set; }

    public uint TileHeight { get; set; }

    public List<Layer> Layers { get; set; }

    public List<Tileset> Tilesets { get; set; }

    public Props Properties { get; set; }

    /// <summary>Retrieves a tile's offset.</summary>
    public Vector2 GetOffsetForTile(uint index) {
      uint horizontalIndex = index % Width;
      uint verticalIndex = (index - (index % Width)) / Width;

      return new Vector2(horizontalIndex * TileWidth, verticalIndex * TileHeight);
    }

    /// <summary>Retrieves a series of adjacent tiles in a direction.</summary>
    public IEnumerable<uint> GetTilesForDirection(uint index, Direction direction) {
      long delta = index;
      while ((delta = GetNextTileDelta((uint)delta, direction)) >= 0 && delta < Size) {
        yield return (uint)delta;
      }
    }

    /// <summary>Returns the delta for an adjacent tile.</summary>
    private long GetNextTileDelta(uint index, Direction direction) {
      switch (direction) {
        case Direction.Up: return index - Width;
        case Direction.Down: return index + Width;
        case Direction.Left:
          // Check if it is on the left edge of the area
          return (index % Width == 0) ? -1 : ((long)index - 1);
        case Direction.Right:
          // Check if it is on the right edge of the area
          return (index % Width == Width - 1) ? -1 : ((long)index + 1);
        default: throw new InvalidOperationException();
      }
    }

    internal class Layer {
      public List<uint> Data { get; set; }

      public string Name { get; set; }

      public float Opacity { get; set; }
    }

    internal class Props {
      public string MusicTheme { get; set; }
    }
  }
}
