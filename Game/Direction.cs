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
    /// <summary>Returns the direction in angles.</summary>
    public static float ToAngle(this Direction direction) {
      switch (direction) {
        case Direction.Up: return 270f;
        case Direction.Down: return 90f;
        case Direction.Left: return 180f;
        case Direction.Right: return 0f;
        default: throw new InvalidOperationException();
      }
    }

    /// <summary>Returns the command type a direction is associated with.</summary>
    public static Command ToCommand(this Direction direction) {
      switch (direction) {
        case Direction.Up: return Command.Up;
        case Direction.Down: return Command.Down;
        case Direction.Left: return Command.Left;
        case Direction.Right: return Command.Right;
        default: throw new InvalidOperationException();
      }
    }

    /// <summary>Returns the direction a command is intended for.</summary>
    public static Direction ToDirection(this Command command) {
      if (command.HasFlag(Command.Up)) {
        return Direction.Up;
      } else if (command.HasFlag(Command.Down)) {
        return Direction.Down;
      } else if (command.HasFlag(Command.Left)) {
        return Direction.Left;
      } else if (command.HasFlag(Command.Right)) {
        return Direction.Right;
      }

      throw new ArgumentOutOfRangeException(nameof(command));
    }
  }
}
