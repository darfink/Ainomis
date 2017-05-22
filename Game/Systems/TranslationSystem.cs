namespace Ainomis.Game.Systems {
  using System;

  using Ainomis.Game.Components;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;

  public class TranslationSystem : EntityComponentProcessingSystem<TransformComponent, VelocityComponent> {
    public override void Process(Entity entity, TransformComponent transform, VelocityComponent velocity) {
      if(velocity.Speed == 0) {
        return;
      }

      // Use the delta as interpolation to prevent frame inconsistencies
      float ms = TimeSpan.FromTicks(this.EntityWorld.Delta).Milliseconds;
      float radians = velocity.AngleAsRadians;

      transform.Position += new Vector2(
        (float)Math.Cos(radians) * velocity.Speed * ms,
        (float)Math.Sin(radians) * velocity.Speed * ms);
    }
  }
}
