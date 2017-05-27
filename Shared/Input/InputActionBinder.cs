namespace Ainomis.Shared.Input {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Shared.Command;
  using Ainomis.Shared.Common;

  using Microsoft.Xna.Framework;

  // TODO: Implement multi-player support
  public class InputActionBinder : ICommandSystem, Common.IUpdateable {
    // Private fields
    private readonly Dictionary<Type, IInputDriver> _inputDrivers;
    private Dictionary<Command, List<IInputBinding>> _commandBindings;

    public InputActionBinder() {
      _commandBindings = new Dictionary<Command, List<IInputBinding>>();
      _inputDrivers = new Dictionary<Type, IInputDriver>();
    }

    /// <summary>
    /// Adds a binding system.
    /// </summary>
    /// <param name="system">System.</param>
    public void AddInputDriver(IInputDriver system) {
      Type bindingType = system.GetBindingType();

      if (!typeof(IInputBinding).IsAssignableFrom(bindingType)) {
        // Ensure that this system implements the IBinding support
        throw new ArgumentException("The system does not implement input binding support", nameof(system));
      }

      // Multiple systems cannot be registered for a single type
      if (_inputDrivers.ContainsKey(bindingType)) {
        throw new ArgumentException("A system is already registered for this type", nameof(system));
      }

      // Add the system to our system list
      _inputDrivers[bindingType] = system;
    }

    /// <summary>Adds an input binding.</summary>
    /// <param name="command">Command.</param>
    /// <param name="binding">Binding.</param>
    public void AddInputBinding(Command command, IInputBinding binding) {
      Type derivedType = binding.GetType();

      if (!_inputDrivers.ContainsKey(derivedType)) {
        throw new NotSupportedException("No system is registered for this binding type");
      }

      if (!_commandBindings.ContainsKey(command)) {
        // Create the binding list if not already created
        _commandBindings[command] = new List<IInputBinding>();
      }

      _commandBindings[command].Add(binding);
    }

    /// <inheritdoc />
    public bool IsCommandActivated(Command command) {
      List<IInputBinding> bindings;
      if (_commandBindings.TryGetValue(command, out bindings)) {
        return bindings.Any((binding) => _inputDrivers[binding.GetType()].IsInputActive(binding));
      }

      return false;
    }

    public void Update(GameTime gameTime) {
      foreach (var driver in _inputDrivers) {
        driver.Value.Update(gameTime);
      }
    }
  }
}
