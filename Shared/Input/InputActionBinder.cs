namespace Ainomis.Shared.Input {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using Microsoft.Xna.Framework;

  // TODO: Implement multi-player support
  public class InputActionBinder<T> : IActionSystem<T>, Common.IUpdateable {
    // Private fields
    private readonly Dictionary<Type, IInputDriver> _inputDrivers;
    private Dictionary<T, List<IInputBinding>> _actionBindings;

    public InputActionBinder() {
      _actionBindings = new Dictionary<T, List<IInputBinding>>();
      _inputDrivers = new Dictionary<Type, IInputDriver>();
    }

    /// <summary>
    /// Adds a binding system.
    /// </summary>
    /// <param name="system">System.</param>
    public void AddInputDriver(IInputDriver system) {
      Type bindingType = system.GetBindingType();

      /*if (!(bindingType is IInputBinding))
      {
        // Ensure that this system implements the IBinding support
        throw new Exception("The system does not implement binding interface support");
      }*/

      // Multiple systems cannot be registered for a single type
      if(_inputDrivers.ContainsKey(bindingType)) {
        throw new Exception("A system is already registered for this type");
      }

      // Add the system to our system list
      _inputDrivers[bindingType] = system;
    }

    /// <summary>
    /// Adds an action binding.
    /// </summary>
    /// <param name="action">Action.</param>
    /// <param name="binding">Binding.</param>
    public void AddInputBinding(T action, IInputBinding binding) {
      Type derivedType = binding.GetType();

      if(!_inputDrivers.ContainsKey(derivedType)) {
        throw new Exception("No system is registered for this binding type");
      }

      if(!_actionBindings.ContainsKey(action)) {
        // Create the binding list if not already created
        _actionBindings[action] = new List<IInputBinding>();
      }

      _actionBindings[action].Add(binding);
    }

    /// <inheritdoc />
    public bool IsActionActivated(T action) {
      List<IInputBinding> bindings;
      if(_actionBindings.TryGetValue(action, out bindings)) {
        return bindings.Any((binding) => _inputDrivers[binding.GetType()].IsInputActive(binding));
      }

      return false;
    }

    public void Update(GameTime gameTime) {
      foreach(var driver in _inputDrivers) {
        driver.Value.Update(gameTime);
      }
    }
  }
}
