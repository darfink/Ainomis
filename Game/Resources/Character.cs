namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  internal class Character {
    public int OffsetX { get; set; }

    public int OffsetY { get; set; }

    public Tileset Tileset { get; set; }

    public List<Animation> Animations { get; set; }
  }
}