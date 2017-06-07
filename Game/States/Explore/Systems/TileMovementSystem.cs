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
  internal class TileMovementSystem : EntityComponentProcessingSystem<ControlComponent, NodeComponent> {
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
    public override void Process(Entity entity, ControlComponent control, NodeComponent node) {
      // These values must be stored, to determine whether the entity has been altered
      var initial = (state: node.State, direction: node.Direction);

      if (node.State.IsMoving()) {
        // TODO: The destination does not take the area's coordinates (transform) into account
        var position = entity.GetComponent<TransformComponent>().Position;
        var destination = node.Map.GetTileOffset(node.Index);
        var runCommand = Command.Run | node.Direction.ToCommand();

        if (HasReachedDestination(position, destination.ToVector2(), node.Direction)) {
          node.State = TileState.Idling;
        } else if(node.State == TileState.Walking && control.IsCommandActivated(runCommand)) {
          node.State = TileState.Running;
        }
      }

      if (node.State == TileState.Idling) {
        // Check for any new inputs from the command source
        var command = _movementCommands.Where(control.IsCommandActivated).FirstOrDefault();
        if (command != Command.None) {
          bool isRunCommandActive = command.HasFlag(Command.Run);
          var direction = command.ToDirection();

          if (isRunCommandActive || command.HasFlag(Command.Walk)) {
            // No tiles are returned in case they are out of bounds
            var tiles = node.Map.GetTilesInDirection(node.Index, direction);
            foreach(var adjacentTile in tiles.Take(1)) {
              if (node.Map.GetTileType(adjacentTile) != Ainomis.Game.Resources.TileType.Walk) {
                break;
              }

              // If the direction has changed, the entity must be aligned
              if (node.Direction != direction) {
                AlignEntityToTile(entity, node);
              }

              // Update the entity's current state and tile position
              node.State = isRunCommandActive ? TileState.Running : TileState.Walking;
              node.Index = adjacentTile;
            }
          }

          // Update the entity's current direction
          node.Direction = direction;
        }
      }

      if (initial.state != node.State || initial.direction != node.Direction) {
        UpdateEntityComponents(entity, node);
      }
    }

    /// <summary>Updates the entity's components after a state change.</summary>
    private static void UpdateEntityComponents(Entity entity, NodeComponent tile) {
      switch (tile.State) {
        case TileState.Walking:
        case TileState.Running:
          var speed = MovementSpeed * (tile.State == TileState.Running ? 1.8f : 1f);
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
    private static void AlignEntityToTile(Entity entity, NodeComponent tile) {
      var position = tile.Map.GetTileOffset(tile.Index);
      var transform = entity.GetComponent<TransformComponent>();
      transform.Position = position.ToVector2();
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
