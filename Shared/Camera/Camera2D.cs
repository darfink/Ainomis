namespace Ainomis.Shared.Camera {
  using System;

  using Microsoft.Xna.Framework;

  public class Camera2D : ICamera2D, Common.IUpdateable {
    // Private members
    private Vector2 _position;

    public Camera2D(Vector2 relativeScale) {
      this.RelativeScale = relativeScale;
      this.ScreenCenter = Vector2.Zero;
      this.MoveSpeed = 1.25f;
      this.Scale = 1f;
    }

    public Vector2 Position {
      get { return _position; }
      set { _position = value; }
    }

    public Vector2 RelativeScale { get; set; }

    public Vector2 Origin { get; set; }

    public Vector2 ScreenCenter { get; set; }

    public Matrix Transform { get; set; }

    public float MoveSpeed { get; set; }

    public float Rotation { get; set; }

    public float Scale { get; set; }

    public IFocusable Focus { get; set; }

    /// <summary>
    /// Gets the bounds of the camera in pixels.
    /// </summary>
    public Rectangle GetBounds(Vector2 screenSize) {
      Matrix inverseViewMatrix = Matrix.Invert(this.Transform);

      Vector2 tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
      Vector2 tr = Vector2.Transform(new Vector2(screenSize.X, 0), inverseViewMatrix);
      Vector2 bl = Vector2.Transform(new Vector2(0, screenSize.Y), inverseViewMatrix);
      Vector2 br = Vector2.Transform(screenSize, inverseViewMatrix);

      var min = new Vector2(
        MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
        MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
      var max = new Vector2(
        MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
        MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));

      return new Rectangle((int)min.X, (int)min.Y, (int)Math.Ceiling(max.X - min.X), (int)Math.Ceiling(max.Y - min.Y));
    }

    /// <summary>
    /// Update the camera's matrix.
    /// </summary>
    /// <param name="gameTime">Game time.</param>
    public virtual void Update(GameTime gameTime) {
      this.Transform =
        Matrix.Identity *
        Matrix.CreateTranslation(-this.Position.X, -this.Position.Y, 0) *
        Matrix.CreateRotationZ(this.Rotation) *
        Matrix.CreateTranslation(this.Origin.X, this.Origin.Y, 0) *
        Matrix.CreateScale(new Vector3(
          this.Scale * this.RelativeScale.X,
          this.Scale * this.RelativeScale.Y,
          1));

      this.Origin = this.ScreenCenter / this.Scale;

      if(this.Focus != null) {
        // Move the camera using fluid animations
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        _position.X += (this.Focus.Position.X - this.Position.X) * this.MoveSpeed * delta;
        _position.Y += (this.Focus.Position.Y - this.Position.Y) * this.MoveSpeed * delta;
      }
    }

    public bool IsInView(Vector2 position, Rectangle bounds) {
      throw new NotImplementedException();
    }
  }
}
