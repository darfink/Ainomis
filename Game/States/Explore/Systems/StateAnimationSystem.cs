namespace Ainomis.Game.States.Explore.Systems {
  using System;
  using System.Collections.Generic;

  using Ainomis.Game.Components;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Shared.Command;

  using Artemis;
  using Artemis.System;

  internal class StateAnimationSystem : EntityComponentProcessingSystem<NodeComponent, AnimationComponent> {
    public override void Process(Entity entity, NodeComponent node, AnimationComponent animation) {
      animation.Name = GetAnimationForState(node.State, node.Direction);
    }

    private string GetAnimationForState(TileState state, Direction direction) {
      switch (state) {
        case TileState.Running: return $"Run{direction}";
        case TileState.Walking: return $"Walk{direction}";
        case TileState.Idling: return $"Idle{direction}";
        default: throw new InvalidOperationException();
      }
    }
  }
}
