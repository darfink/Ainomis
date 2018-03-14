namespace Ainomis.Game.Resources {
    internal enum TileType {
      Block,
      Walk,
      Surf,
    }

    internal class Tile {
      public TileType Type { get; set; }
    }
}
