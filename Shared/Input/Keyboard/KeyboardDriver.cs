namespace Ainomis.Shared.Input.Keyboard {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  public class KeyboardDriver : IInputDriver {
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

    public bool IsInputActive(IInputBinding interfaceBinding) {
      var binding = (KeyboardBinding)interfaceBinding;

      if(_currentState.IsKeyDown(binding.Key) && binding.Modifiers.All(_currentState.IsKeyDown)) {
        var heldTime = _keyHeldTimes[binding.Key];
        if(heldTime >= binding.Duration) {
          return !binding.Timeout.HasValue || (heldTime < binding.Timeout.Value);
        }
      }

      return false;
    }

    public Type GetBindingType() => typeof(KeyboardBinding);
  }
}
