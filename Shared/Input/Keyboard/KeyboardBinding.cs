namespace Ainomis.Shared.Input.Keyboard {
  using System;
  using System.Linq;

  using Ainomis.Extensions;

  using Microsoft.Xna.Framework.Input;

  public class KeyboardBinding : IInputBinding {
    /// <summary>Constructs a new keyboard binding.</summary>
    public KeyboardBinding(
        Keys key,
        TimeSpan duration = new TimeSpan(),
        TimeSpan? timeout = null,
        params Keys[] modifiers) {
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

    /// <summary>Gets the key.</summary>
    public Keys Key { get; private set; }

    /// <summary>Gets the modifiers.</summary>
    public Keys[] Modifiers { get; private set; }

    /// <summary>Gets the duration.</summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>Gets the timeout.</summary>
    public TimeSpan? Timeout { get; private set; }
  }
}
