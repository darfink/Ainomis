namespace Ainomis.Shared.Input {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;

  using Microsoft.Xna.Framework;

  // TODO: Implement multi-player support
  public class KeyActionBinder<T> : Common.IUpdateable {
    // Private fields
    private readonly Dictionary<Type, IKeyBindingSystem> bindingSystems;
    private Dictionary<T, List<IKeyBinding>> actionBindings;

    public KeyActionBinder() {
      actionBindings = new Dictionary<T, List<IKeyBinding>>();
      bindingSystems = new Dictionary<Type, IKeyBindingSystem>();
    }

    /// <summary>
    /// Adds a binding system.
    /// </summary>
    /// <param name="system">System.</param>
    public void AddBindingSystem(IKeyBindingSystem system) {
      Type bindingType = system.GetBindingType();

      /*if (!(bindingType is IKeyBinding))
      {
        // Ensure that this system implements the IBinding support
        throw new Exception("The system does not implement binding interface support");
      }*/

      // Multiple systems cannot be registered for a single type
      if(bindingSystems.ContainsKey(bindingType)) {
        throw new Exception("A system is already registered for this type");
      }

      // Add the system to our system list
      bindingSystems[bindingType] = system;
    }

    /// <summary>
    /// Adds the action binding.
    /// </summary>
    /// <param name="action">Action.</param>
    /// <param name="binding">Binding.</param>
    public void AddActionBinding(T action, IKeyBinding binding) {
      Type derivedType = binding.GetType();

      if(!bindingSystems.ContainsKey(derivedType)) {
        throw new Exception("No system is registered for this binding type");
      }

      if(!actionBindings.ContainsKey(action)) {
        // Create the binding list if not already created
        actionBindings[action] = new List<IKeyBinding>();
      }

      actionBindings[action].Add(binding);
    }

    /// <summary>
    /// Determines whether an action is currently active or not.
    /// </summary>
    /// <returns>true if this action is activated, otherwise false.</returns>
    /// <param name="action">Action name.</param>
    public bool IsActionActivated(T action) {
      List<IKeyBinding> bindings;
      if(actionBindings.TryGetValue(action, out bindings)) {
        return bindings.Any((binding) => bindingSystems[binding.GetType()].IsBindingActive(binding));
      }

      return false;
    }

    public void Update(GameTime gameTime) {
      foreach(var item in bindingSystems) {
        item.Value.Update(gameTime);
      }
    }
  }
}
