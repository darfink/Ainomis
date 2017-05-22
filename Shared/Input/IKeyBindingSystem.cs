namespace Ainomis.Shared.Input {
  using System;

  public interface IKeyBindingSystem : Common.IUpdateable {
    /// <summary>
    /// Determines whether a binding is currently active or not.
    /// </summary>
    /// <returns>true if the binding is active, otherwise false.</returns>
    /// <param name="binding">Binding.</param>
    bool IsBindingActive(IKeyBinding binding);

    /// <summary>
    /// Returns the associated binding type.
    /// </summary>
    /// <returns>the associated binding type.</returns>
    Type GetBindingType();
  }
}
