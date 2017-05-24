namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class GamePadDriver : IInputDriver {
    private readonly Dictionary<Buttons, TimeSpan> _buttonHeldTimes;
    private Func<GamePadState> _getCurrentState;
    private GamePadState _currentState;

    public GamePadDriver(PlayerIndex player) : this(() => GamePad.GetState(player)) {
    }

    public GamePadDriver(Func<GamePadState> getCurrentState) {
      _buttonHeldTimes = new Dictionary<Buttons, TimeSpan>();
      _getCurrentState = getCurrentState;
      _currentState = getCurrentState();

      foreach(var button in Enum.GetValues(typeof(Buttons))) {
        // Initially all keys have been held for zero seconds
        _buttonHeldTimes.Add((Buttons)button, TimeSpan.Zero);
      }
    }

    public static bool IsConnected(PlayerIndex player) => GamePad.GetState(player).IsConnected;

    public void Update(GameTime gameTime) {
      _currentState = _getCurrentState();

      foreach(Buttons button in Enum.GetValues(typeof(Buttons))) {
        // Update the current hold time for each key
        if(_currentState.IsButtonDown(button)) {
          _buttonHeldTimes[button] += gameTime.ElapsedGameTime;
        } else {
          _buttonHeldTimes[button] = TimeSpan.Zero;
        }
      }
    }

    public bool IsInputActive(IInputBinding binding) {
      var joystickBinding = (GamePadBinding)binding;

      if(_currentState.IsButtonDown(joystickBinding.Button)) {
        if(_buttonHeldTimes[joystickBinding.Button] >= joystickBinding.Duration) {
          return !joystickBinding.Timeout.HasValue || (_buttonHeldTimes[joystickBinding.Button] < joystickBinding.Timeout.Value);
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(GamePadBinding);
  }
}
