namespace Ainomis.Shared.Input {
  internal interface IActionSystem<T> {
    /// <summary>
    /// Determines whether an action is currently active or not.
    /// </summary>
    /// <returns>true if this action is activated, otherwise false.</returns>
    /// <param name="action">Action name.</param>
    bool IsActionActivated(T action);
  }
}