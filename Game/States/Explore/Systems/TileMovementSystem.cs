namespace Ainomis.Game.States.Explore.Systems {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Game.Components;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Shared.Command;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;

  /// <summary>Tile movement system.</summary>
  internal class TileMovementSystem : EntityComponentProcessingSystem<ControlComponent, TileComponent> {
    /// <summary>Default movement speed for entities.</summary>
    private const float MovementSpeed = 0.05f;

    /// <summary>Available movement commands.</summary>
    private readonly Command[] _movementCommands = {
      Command.RunUp,
      Command.RunDown,
      Command.RunLeft,
      Command.RunRight,
      Command.MoveUp,
      Command.MoveDown,
      Command.MoveLeft,
      Command.MoveRight,
      Command.TapUp,
      Command.TapDown,
      Command.TapLeft,
      Command.TapRight,
    };

    /// <summary>Processes a movable entity on the tile map.</summary>
    public override void Process(Entity entity, ControlComponent control, TileComponent tile) {
      Action alignPosition = null;
      bool hasReachedDestination = false;

      // If the entity is moving, determine whether it has reached its destination or not
      if (tile.State == TileState.Moving || tile.State == TileState.Running) {
        var transform = entity.GetComponent<TransformComponent>();
        var destination = tile.Area.GetOffsetForTile(tile.Index);

        if (!HasReachedDestination(transform.Position, destination, tile.Direction)) {
          return;
        }

        // Normalizer for the position so it's aligned along tiles
        alignPosition = () => transform.Position = destination;
        hasReachedDestination = true;
      }

      // Check for any new inputs from the command source
      foreach (var command in _movementCommands.Where(control.IsCommandActivated).Take(1)) {
        var direction = GetCommandDirection(command);
        var previousDirection = tile.Direction;
        tile.Direction = direction;

        if (!command.HasFlag(Command.Tap)) {
          // No tiles are returned in case it is out of bounds
          var tiles = tile.Area.GetTilesForDirection(tile.Index, direction);

          foreach(var nextTile in tiles.Take(1)) {
            bool isRunning = command.HasFlag(Command.Run);

            // Update the entity's current state and tile position
            tile.State = isRunning ? TileState.Running : TileState.Moving;
            tile.Index = nextTile;

            // Determine the speed of the entity
            float speed = MovementSpeed * (isRunning ? 2 : 1);
            entity.AddComponent(new VelocityComponent(speed, direction.ToAngle()));
            entity.Refresh();

            // If the direction has changed the position must be aligned
            if (alignPosition != null && previousDirection != direction) {
              alignPosition();
            }

            // Prevent the velocity component from being removed
            hasReachedDestination = false;
          }
        }
      }

      if (hasReachedDestination) {
        tile.State = TileState.Idling;
        alignPosition();

        entity.RemoveComponent<VelocityComponent>();
        entity.Refresh();
      }
    }

    /// <summary>Determines whether the entity has reached the designated destination or not.</summary>
    private static bool HasReachedDestination(Vector2 position, Vector2 destination, Direction direction) {
      switch (direction) {
        case Direction.Up: return position.Y <= destination.Y;
        case Direction.Down: return position.Y >= destination.Y;
        case Direction.Left: return position.X <= destination.X;
        case Direction.Right: return position.X >= destination.X;
        default: throw new InvalidOperationException();
      }
    }

    /// <summary>Returns the direction a command is intended for.</summary>
    private static Direction GetCommandDirection(Command command) {
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
