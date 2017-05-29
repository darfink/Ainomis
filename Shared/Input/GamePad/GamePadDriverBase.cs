namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public abstract class GamePadDriverBase : IInputDriver, Common.IUpdateable {
    private readonly Dictionary<Buttons, TimeSpan> _buttonHeldTimes;
    private GamePadState _currentState;

    public GamePadDriverBase() {
      _buttonHeldTimes = new Dictionary<Buttons, TimeSpan>();

      foreach(var button in Enum.GetValues(typeof(Buttons))) {
        // Initially all keys have been held for zero seconds
        _buttonHeldTimes.Add((Buttons)button, TimeSpan.Zero);
      }
    }

    public virtual void Update(GameTime gameTime) {
      _currentState = GetCurrentState();

      foreach(Buttons button in Enum.GetValues(typeof(Buttons))) {
        // Update the current hold time for each key
        if(_currentState.IsButtonDown(button)) {
          _buttonHeldTimes[button] += gameTime.ElapsedGameTime;
        } else {
          _buttonHeldTimes[button] = TimeSpan.Zero;
        }
      }
    }

    public bool IsInputActive(IInputBinding interfaceBinding) {
      var binding = (GamePadBinding)interfaceBinding;

      if(binding.Buttons.All(_currentState.IsButtonDown)) {
        if(binding.Buttons.All(button => _buttonHeldTimes[button] >= binding.Duration)) {
          return !binding.Timeout.HasValue || binding.Buttons
            .All(button => _buttonHeldTimes[button] < binding.Timeout.Value);
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(GamePadBinding);

    protected abstract GamePadState GetCurrentState();
  }
}
