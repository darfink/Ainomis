namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework.Graphics;

  internal class Animation {
    public string Name { get; set; }

    public List<Frame> Frames { get; set; }

    internal class Frame {
      public uint TileId { get; set; }

      public TimeSpan Duration { get; set; }

      public SpriteEffects Effects { get; set; }
    }
  }
}
