namespace Ainomis.Shared.Viewport {
  using Ainomis.Extensions;

  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Display;

  using Microsoft.Xna.Framework.Graphics;

  /// <summary>Represents a visible area.</summary>
  public class CameraDisplayView : IViewportProvider {
    private ICamera2D _camera;
    private IDisplayInfo _displayInfo;

    /// <summary>Creates a new viewport component.</summary>
    public CameraDisplayView(ICamera2D camera, IDisplayInfo display) {
      _displayInfo = display.ThrowIfNull(nameof(display));
      _camera = camera.ThrowIfNull(nameof(camera));
    }

    /// <inheritdoc />
    public Viewport Viewport => _camera.GetViewport(_displayInfo.Resolution);
  }
}
