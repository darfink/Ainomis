namespace Ainomis.Shared.Camera {
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  public interface ICamera2D {
    /// <summary>Gets or sets the position of the camera.</summary>
    /// <value>The position.</value>
    ref Vector2 Position { get; }

    /// <summary>
    ///   Gets or sets the move speed of the camera.
    ///   The camera will tween to its destination.
    /// </summary>
    /// <value>The move speed.</value>
    float MoveSpeed { get; set; }

    /// <summary>Gets or sets the rotation of the camera.</summary>
    /// <value>The rotation.</value>
    float Rotation { get; set; }

    /// <summary>Gets the origin of the viewport (accounts for Scale).</summary>
    /// <value>The origin.</value>
    Vector2 Origin { get; }

    /// <summary>Gets or sets the scale of the Camera.</summary>
    /// <value>The scale.</value>
    float Scale { get; set; }

    /// <summary>Gets the screen center (does not account for Scale).</summary>
    /// <value>The screen center.</value>
    Vector2 ScreenCenter { get; }

    /// <summary>
    ///   Gets the transform that can be applied to the SpriteBatch Class.
    /// </summary>
    /// <see cref="SpriteBatch"/>
    /// <value>The transform.</value>
    Matrix Transform { get; }

    /// <summary>Gets or sets the focus of the Camera.</summary>
    /// <seealso cref="IFocusable"/>
    /// <value>The focus.</value>
    IFocusable Focus { get; set; }

    /// <summary>Gets the bounds of the Camera.</summary>
    /// <seealso cref="IDisplayInfo"/>
    /// <value>The bounds.</value>
    Rectangle GetBounds(Vector2 screenSize);

    /// <summary>
    ///   Determines whether the target is in view given the specified position.
    ///   This can be used to increase performance by not drawing objects
    ///   directly in the viewport
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="bounds">The bounds.</param>
    /// <returns>
    ///   <c>true</c> if the target is in view at the specified position;
    ///   otherwise, <c>false</c>.
    /// </returns>
    bool IsInView(Vector2 position, Rectangle bounds);
  }
}
