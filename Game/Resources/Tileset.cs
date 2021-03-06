namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;

  internal class Tileset {
    public uint Columns { get; set; }

    public string Image { get; set; }

    public uint ImageHeight { get; set; }

    public uint ImageWidth { get; set; }

    public int Margin { get; set; }

    public string Name { get; set; }

    public int Spacing { get; set; }

    public uint TileCount { get; set; }

    public uint TileHeight { get; set; }

    public Dictionary<uint, Tile> Tiles { get; set; }

    public uint TileWidth { get; set; }

    public uint FirstGid { get; set; }

    public Props Properties { get; set; }

    /// <summary>Retrieves a tile's source rectangle.</summary>
    /// <returns>The tile's source rectangle.</returns>
    /// <param name="index">Tile index.</param>
    public Rectangle GetSourceRectangleForTile(uint index) {
      if (FirstGid > index) {
        throw new ArgumentOutOfRangeException(nameof(index));
      }

      uint adjustedIndex = index - FirstGid;

      // With some modulus magic, calculate the tile index's X and Y position
      uint horizontalIndex = adjustedIndex % Columns;
      uint verticalIndex = (adjustedIndex - horizontalIndex) / Columns;

      return new Rectangle(
        (int)(Margin + (horizontalIndex * TileWidth) + (horizontalIndex * Spacing)),
        (int)(Margin + (verticalIndex * TileHeight) + (verticalIndex * Spacing)),
        (int)TileWidth,
        (int)TileHeight);
    }

    internal class Props {
      public bool IsMeta { get; set; }
    }
  }
}
