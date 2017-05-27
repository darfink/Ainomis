namespace Ainomis.Shared {
  using Ainomis.Extensions;

  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Display;

  using Microsoft.Xna.Framework;

  /// <summary>Represents a renderable area.</summary>
  public class RenderArea {
    private ICamera2D _camera;
    private IDisplayInfo _displayInfo;

    /// <summary>Creates a new render area component.</summary>
    public RenderArea(IDisplayInfo display, ICamera2D camera) {
      _displayInfo = display.ThrowIfNull(nameof(display));
      _camera = camera.ThrowIfNull(nameof(camera));
    }

    /// <summary>Gets the bounds of the render area in pixels.</summary>
    /// <value>The bounds.</value>
    public Rectangle GetBounds() => _camera.GetBounds(_displayInfo.Resolution);
  }
}