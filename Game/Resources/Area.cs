namespace Ainomis.Game.Resources {
  using System.Collections.Generic;

  internal class Area {
    public uint Width { get; set; }

    public uint Height { get; set; }

    public List<Layer> Layers { get; set; }

    public List<Tileset> Tilesets { get; set; }

    public Props Properties { get; set; }

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