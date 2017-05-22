namespace Ainomis.Shared.Display {
  using Microsoft.Xna.Framework;

  public interface IDisplayInfo {
    /// <summary>
    /// Gets the relative scale matrix.
    /// </summary>
    /// <value>The relative scale matrix.</value>
    Matrix RelativeScaleMatrix { get; }

    /// <summary>
    /// Gets the relative scale.
    /// </summary>
    /// <value>The relative scale.</value>
    Vector2 RelativeScale { get; }

    /// <summary>
    /// Gets the virtual resolution.
    /// </summary>
    /// <value>The virtual resolution.</value>
    Size VirtualResolution { get; }

    /// <summary>
    /// Gets the current game resolution.
    /// </summary>
    /// <value>The resolution.</value>
    Size Resolution { get; }
  }
}
