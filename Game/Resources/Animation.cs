namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework.Graphics;

  internal class Animation {
    /// <summary>Gets or sets the animation name.</summary>
    public string Name { get; set; }

    /// <summary>Gets or sets whether the animation.</summary>
    public bool Loop { get; set; } = true;

    /// <summary>Gets or sets the animation's frames.</summary>
    public List<Frame> Frames { get; set; }

    internal class Frame {
      /// <summary>Gets or sets the frame's tile index.</summary>
      public uint TileId { get; set; }

      /// <summary>Gets or sets the frame's duration.</summary>
      public TimeSpan Duration { get; set; }

      /// <summary>Gets or sets the frame's effects.</summary>
      public SpriteEffects Effects { get; set; }
    }
  }
}
