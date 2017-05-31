namespace Ainomis.Game.Resources {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  internal class Character {
    public Vector2 Origin { get; set; }

    public Tileset Tileset { get; set; }

    public List<Animation> Animations { get; set; }
  }
}
