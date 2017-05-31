namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;

  using Microsoft.Xna.Framework.Input;

  public class GamePadBinding : IInputBinding {
    /// <summary>Constructs a new game pad binding.</summary>
    public GamePadBinding(
        Buttons button,
        TimeSpan duration = new TimeSpan(),
        TimeSpan? timeout = null,
        params Buttons[] modifiers) {
      if (button.GetFlags().Count() != 1) {
        throw new ArgumentOutOfRangeException(nameof(button), "One button only must be specified");
      }

      this.Modifiers = modifiers;
      this.Duration = duration;
      this.Button = button;

      if(timeout != null) {
        if (timeout.Value <= duration) {
          throw new ArgumentException("timeout must be larger than duration");
        }

        this.Timeout = timeout;
      }
    }

    /// <summary>Gets the buttons.</summary>
    public Buttons Button { get; private set; }

    /// <summary>Gets the modifiers.</summary>
    public Buttons[] Modifiers { get; private set; }

    /// <summary>Gets the duration.</summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>Gets the timeout.</summary>
    public TimeSpan? Timeout { get; private set; }
  }
}
