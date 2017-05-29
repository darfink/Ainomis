namespace Ainomis.Game.Components {
  using System;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;

  internal class VelocityComponent : IComponent {
    /// <summary>Initializes a new velocity component.</summary>
    /// <param name="speed">Speed.</param>
    /// <param name="angle">Angle.</param>
    public VelocityComponent(float speed, float angle) {
      this.Speed = speed;
      this.Angle = angle;
    }

    /// <summary>Returns whether this component is moving or not.</summary>
    public bool IsMoving => Speed != 0;

    /// <summary>Gets or sets the speed.</summary>
    public float Speed { get; set; }

    /// <summary>Gets or sets the angle.</summary>
    public float Angle { get; set; }

    /// <summary>Gets the angle as radians.</summary>
    public float AngleAsRadians => MathHelper.ToRadians(this.Angle);
  }
}
