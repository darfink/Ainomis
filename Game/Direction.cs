namespace Ainomis.Game {
  using System;

  using Ainomis.Shared.Command;

  /// <summary>Describes an objects direction.</summary>
  internal enum Direction {
    Up,
    Down,
    Left,
    Right
  }

  internal static class DirectionHelper {
    public static float ToAngle(this Direction direction) {
      switch (direction) {
        case Direction.Up: return 270f;
        case Direction.Down: return 90f;
        case Direction.Left: return 180f;
        case Direction.Right: return 0f;
        default: throw new InvalidOperationException();
      }
    }
  }
}