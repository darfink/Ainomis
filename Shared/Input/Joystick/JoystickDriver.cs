namespace Ainomis.Shared.Input.Joystick {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class JoystickDriver : IInputDriver {
    // Private fields
    private readonly Dictionary<Buttons, TimeSpan> _buttonHeldTimes;
    private GamePadState _currentState;
    private PlayerIndex _playerIndex;

    public JoystickDriver(PlayerIndex player) {
      _buttonHeldTimes = new Dictionary<Buttons, TimeSpan>();
      _currentState = GamePad.GetState(player);
      _playerIndex = player;

      foreach(var button in Enum.GetValues(typeof(Buttons))) {
        // Initially all keys have been held for zero seconds
        _buttonHeldTimes.Add((Buttons)button, TimeSpan.Zero);
      }
    }

    public static bool IsConnected(PlayerIndex player) => GamePad.GetState(player).IsConnected;

    public void Update(GameTime gameTime) {
      _currentState = GamePad.GetState(_playerIndex);

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
      var joystickBinding = (JoystickBinding)binding;

      // Ensure the selected key and all its modifiers are active. If no
      // modifiers are specified, the array will be empty, and 'All' will return
      // true. See: https://msdn.microsoft.com/library/bb548541(v=vs.100).aspx
      if(_currentState.IsButtonDown(joystickBinding.Button)) {
        if(_buttonHeldTimes[joystickBinding.Button] >= joystickBinding.Duration) {
          return !joystickBinding.Timeout.HasValue || (_buttonHeldTimes[joystickBinding.Button] < joystickBinding.Timeout.Value);
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(JoystickBinding);
  }
}
