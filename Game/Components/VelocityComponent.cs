namespace Ainomis.Game.Components {
  using System;

  using Artemis.Interface;

  internal class VelocityComponent : IComponent {
    /// <summary>To radians.</summary>
    private const float ToRadians = (float)(Math.PI / 180.0);

    /// <summary>
    /// Initializes a new velocity component.
    /// </summary>
    /// <param name="speed">Speed.</param>
    /// <param name="angle">Angle.</param>
    public VelocityComponent(float speed = 0, float angle = 0) {
      this.Speed = speed;
      this.Angle = angle;
    }

    /// <summary>
    /// Gets or sets the speed.
    /// </summary>
    /// <value>The speed.</value>
    public float Speed { get; set; }

    /// <summary>
    /// Gets or sets the angle.
    /// </summary>
    /// <value>The angle.</value>
    public float Angle { get; set; }

    /// <summary>
    /// Gets the angle as radians.
    /// </summary>
    /// <value>The angle as radians.</value>
    public float AngleAsRadians {
      get { return ToRadians * this.Angle; }
    }
  }
}
