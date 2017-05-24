namespace Ainomis.Shared.Input.Keyboard {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class KeyboardDriver : IInputDriver {
    // Private fields
    private readonly Dictionary<Keys, TimeSpan> _keyHeldTimes;
    private KeyboardState _currentState;

    public KeyboardDriver() {
      _keyHeldTimes = new Dictionary<Keys, TimeSpan>();
      _currentState = Keyboard.GetState();

      foreach(Keys key in Enum.GetValues(typeof(Keys))) {
        // Initially all keys have been held for zero seconds
        _keyHeldTimes.Add(key, TimeSpan.Zero);
      }
    }

    public void Update(GameTime gameTime) {
      _currentState = Keyboard.GetState();

      foreach(Keys key in Enum.GetValues(typeof(Keys))) {
        // Update the current hold time for each key
        if(_currentState[key] == KeyState.Down) {
          _keyHeldTimes[key] += gameTime.ElapsedGameTime;
        } else {
          _keyHeldTimes[key] = TimeSpan.Zero;
        }
      }
    }

    public bool IsInputActive(IInputBinding binding) {
      var keyboardBinding = (KeyboardBinding)binding;

      // Ensure the selected key and all its modifiers are active. If no
      // modifiers are specified, the array will be empty, and 'All' will return
      // true. See: https://msdn.microsoft.com/library/bb548541(v=vs.100).aspx
      if(_currentState.IsKeyDown(keyboardBinding.Key) && keyboardBinding.Modifiers.All(_currentState.IsKeyDown)) {
        if(_keyHeldTimes[keyboardBinding.Key] >= keyboardBinding.Duration) {
          return !keyboardBinding.Timeout.HasValue || (_keyHeldTimes[keyboardBinding.Key] < keyboardBinding.Timeout.Value);
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(KeyboardBinding);
  }
}
