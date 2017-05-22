namespace Ainomis.Shared.Display {
  using Microsoft.Xna.Framework;

  public class DisplayInfo : IDisplayInfo {
    public DisplayInfo(Size virtualResolution, Size realResolution) {
      this.VirtualResolution = virtualResolution;
      this.ChangeResolution(realResolution);
    }

    public Matrix RelativeScaleMatrix { get; private set; }

    public Vector2 RelativeScale { get; private set; }

    public Size VirtualResolution { get; private set; }

    public Size Resolution { get; private set; }

    public void ChangeResolution(Size newResolution) {
      this.Resolution = newResolution;
      this.RelativeScale = (Vector2)this.Resolution / (Vector2)this.VirtualResolution;
      this.RelativeScaleMatrix = Matrix.CreateScale(
        this.RelativeScale.X,
        this.RelativeScale.Y,
        1);
    }
  }
}
