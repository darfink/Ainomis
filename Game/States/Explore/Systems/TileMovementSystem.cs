namespace Ainomis.Game.States.Explore.Systems {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;
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
      Command.WalkUp,
      Command.WalkDown,
      Command.WalkLeft,
      Command.WalkRight,
      Command.TapUp,
      Command.TapDown,
      Command.TapLeft,
      Command.TapRight,
    };

    /// <summary>Processes a movable entity.</summary>
    public override void Process(Entity entity, ControlComponent control, TileComponent tile) {
      var initial = (state: tile.State, direction: tile.Direction);

      if (tile.State.IsMoving()) {
        // TODO: The destination does not take the area's coordinates into account
        var position = entity.GetComponent<TransformComponent>().Position;
        var destination = tile.Area.GetTileOffset(tile.Index);
        var runCommand = Command.Run | tile.Direction.ToCommand();

        if (HasReachedDestination(position, destination, tile.Direction)) {
          tile.State = TileState.Idling;
        } else if(tile.State == TileState.Moving && control.IsCommandActivated(runCommand)) {
          tile.State = TileState.Running;
        }
      }

      if (tile.State == TileState.Idling) {
        // Check for any new inputs from the command source
        var command = _movementCommands.Where(control.IsCommandActivated).FirstOrDefault();
        if (command != Command.None) {
          bool isRunning = command.HasFlag(Command.Run);
          var direction = command.ToDirection();

          if (isRunning || command.HasFlag(Command.Walk)) {
            // No tiles are returned in case they are out of bounds
            var tiles = tile.Area.GetTilesInDirection(tile.Index, direction);
            foreach(var adjacentTile in tiles.Take(1)) {
              // If the direction has changed the position must be aligned
              if (tile.Direction != direction) {
                AlignEntityToTile(entity, tile);
              }

              // Update the entity's current state and tile position
              tile.State = isRunning ? TileState.Running : TileState.Moving;
              tile.Index = adjacentTile;
            }
          }

          // Update the entity's current direction
          tile.Direction = direction;
        }
      }

      if (initial.state != tile.State || initial.direction != tile.Direction) {
        UpdateEntityComponents(entity, tile);
      }
    }

    /// <summary>Updates the entity's components after a state change.</summary>
    private static void UpdateEntityComponents(Entity entity, TileComponent tile) {
      switch (tile.State) {
        case TileState.Moving:
        case TileState.Running:
          var speed = MovementSpeed * (tile.State == TileState.Running ? 1.5f : 1f);
          entity.AddComponent(new VelocityComponent(speed, tile.Direction.ToAngle()));
          break;
        case TileState.Idling:
          entity.RemoveComponent<VelocityComponent>();
          AlignEntityToTile(entity, tile);
          break;
        default: throw new InvalidOperationException();
      }

      entity.Refresh();
    }

    /// <summary>Aligns the entity's position to its current tile.</summary>
    private static void AlignEntityToTile(Entity entity, TileComponent tile) {
      var transform = entity.GetComponent<TransformComponent>();
      var position = tile.Area.GetTileOffset(tile.Index);
      transform.Position = position;
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
  }
}
