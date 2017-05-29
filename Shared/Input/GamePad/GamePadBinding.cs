namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;

  using Microsoft.Xna.Framework.Input;

  public class GamePadBinding : IInputBinding {
    /// <summary>Constructs a new game pad binding.</summary>
    public GamePadBinding(Buttons button, TimeSpan duration = new TimeSpan(), TimeSpan? timeout = null) {
      this.Buttons = button.GetFlags().Cast<Buttons>().ToList();
      this.Duration = duration;

      if(timeout != null) {
        if (timeout.Value <= duration) {
          throw new ArgumentException("timeout must be larger than duration");
        }

        this.Timeout = timeout;
      }
    }

    /// <summary>Gets the buttons.</summary>
    public List<Buttons> Buttons { get; private set; }

    /// <summary>Gets the duration.</summary>
    public TimeSpan Duration { get; private set; }

    /// <summary>Gets the timeout.</summary>
    public TimeSpan? Timeout { get; private set; }
  }
}
