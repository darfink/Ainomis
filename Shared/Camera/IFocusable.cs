namespace Ainomis.Shared.Camera {
  using System;
  using System.Collections.Generic;

  using Microsoft.Xna.Framework;

  /// <summary>
  /// Represents a focusable object
  /// </summary>
  public interface IFocusable {
    ref Vector2 Position { get; }
  }
}
