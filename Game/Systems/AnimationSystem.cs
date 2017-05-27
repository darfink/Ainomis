namespace Ainomis.Game.Systems {
  using System;

  using Ainomis.Game.Components;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;

  internal class AnimationSystem : EntityComponentProcessingSystem<AnimationComponent, TilesetComponent> {
    public override void Process(Entity entity, AnimationComponent animation, TilesetComponent tileset) {
      animation.FrameTime += TimeSpan.FromTicks(this.EntityWorld.Delta);

      if (animation.FrameTime > animation.Frame.Duration) {
        animation.FrameTime = TimeSpan.Zero;
        animation.NextFrame();
      }

      var sprite = entity.GetComponent<SpriteComponent>();
      if (sprite != null) {
        sprite.Effects = animation.Frame.Effects;
      }

      tileset.TileId = animation.Frame.TileId;
    }
  }
}
