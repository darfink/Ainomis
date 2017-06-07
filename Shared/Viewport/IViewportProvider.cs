namespace Ainomis.Shared.Viewport {
  using Microsoft.Xna.Framework.Graphics;

  // TODO: Rename to IViewportState?
  internal interface IViewportProvider {
    /// <summary>Returns the current viewport.</summary>
    Viewport Viewport { get; }
  }
}
