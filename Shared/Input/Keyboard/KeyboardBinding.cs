namespace Ainomis.Shared.Input.Keyboard {
  using System;

  using Microsoft.Xna.Framework.Input;

  public class KeyboardBinding : IKeyBinding {
    /// <summary>
    /// Initializes a new instance of the <see cref="Ainomis.Input.Keyboard.KeyboardBinding"/> class.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="duration">Duration.</param>
    /// <param name="modifiers">Modifiers.</param>
    public KeyboardBinding(Keys key, TimeSpan duration = new TimeSpan(), TimeSpan? timeout = null, params Keys[] modifiers) {
      this.Modifiers = modifiers;
      this.Duration = duration;
      this.Key = key;

      if(timeout != null) {
        if (timeout.Value <= duration) {
          throw new ArgumentException("timeout must be larger than duration");
        }

        this.Timeout = timeout;
      }
    }

    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <value>The key.</value>
    public Keys Key { get; private set; }

    /// <summary>
    /// Gets the modifiers.
    /// </summary>
    /// <value>The modifiers.</value>
    public Keys[] Modifiers { get; private set; }

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
