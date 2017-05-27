namespace Ainomis.Game.States.Explore.Systems {
  using System;
  using System.Collections.Generic;

  using Ainomis.Game.Components;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Shared.Command;

  using Artemis;
  using Artemis.System;

  internal class StateAnimationSystem : EntityComponentProcessingSystem<TileComponent, AnimationComponent> {
    public override void Process(Entity entity, TileComponent tile, AnimationComponent animation) {
      animation.Name = GetStateAnimation(tile.State, tile.Direction);
    }

    private string GetStateAnimation(TileState state, Direction direction) {
      switch (state) {
        case TileState.Running:
        case TileState.Moving: return $"Move{direction}";
        case TileState.Idling: return $"Idle{direction}";
        default: throw new InvalidOperationException();
      }
    }
  }
}