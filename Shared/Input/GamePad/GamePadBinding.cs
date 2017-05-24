namespace Ainomis.Shared.Input.GamePad {
  using System;

  using Microsoft.Xna.Framework.Input;

  public class GamePadBinding : IInputBinding {
    /// <summary>
    /// Initializes a new instance of the GamePadBinding class.
    /// </summary>
    /// <param name="button">Button.</param>
    /// <param name="duration">Duration.</param>
    public GamePadBinding(Buttons button, TimeSpan duration = new TimeSpan(), TimeSpan? timeout = null) {
      this.Duration = duration;
      this.Button = button;

      if(timeout != null) {
        if (timeout.Value <= duration) {
          throw new ArgumentException("timeout must be larger than duration");
        }

        this.Timeout = timeout;
      }
    }

    /// <summary>
    /// Gets the button.
    /// </summary>
    /// <value>The button.</value>
    public Buttons Button { get; private set; }

    /// <summary>
    /// Gets the duration.
    /// </summary>
    /// <value>The duration.</value>
    public TimeSpan Duration { get; private set; }

    /// <summary>
    /// Gets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    public TimeSpan? Timeout { get; private set; }
  }
}
