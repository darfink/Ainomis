namespace Ainomis.Shared.Input.GamePad {
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  using Newtonsoft.Json;

  public class VirtualOverlay {
    public string Image { get; set; }

    public uint ImageHeight { get; set; }

    public uint ImageWidth { get; set; }

    public List<InputMap> Inputs { get; set; }

    public class InputMap {
      public Buttons Button { get; set; }

      public Rectangle Area { get; set; }
    }
  }
}
