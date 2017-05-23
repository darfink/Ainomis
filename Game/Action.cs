namespace Ainomis.Game {
  using System;

  /// <summary>
  /// Describes an action that can be triggered by a player or AI.
  /// </summary>
  [Flags]
  public enum Action {
    // Default
    None = 0,

    // Movement
    Tap  = (1 << 0),
    Move = (1 << 1),
    Run  = (1 << 2),

    // Direction
    Up    = (1 << 3),
    Down  = (1 << 4),
    Left  = (1 << 5),
    Right = (1 << 6),

    // Meta
    Start = (1 << 7),
    Exit  = (1 << 8),

    // Tap aliases
    TapUp    = (Tap | Up),
    TapDown  = (Tap | Down),
    TapLeft  = (Tap | Left),
    TapRight = (Tap | Right),

    // Movement aliases
    MoveUp    = (Move | Up),
    MoveDown  = (Move | Down),
    MoveLeft  = (Move | Left),
    MoveRight = (Move | Right),

    // Sprint aliases
    SprintUp    = (MoveUp | Run),
    SprintDown  = (MoveDown | Run),
    SprintLeft  = (MoveLeft | Run),
    SprintRight = (MoveRight | Run),
  }
}
