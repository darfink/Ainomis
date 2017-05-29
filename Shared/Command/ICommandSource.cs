namespace Ainomis.Shared.Command {
  internal interface ICommandSource {
    /// <summary>
    /// Determines whether an action is currently active or not.
    /// </summary>
    /// <returns>true if this command is activated, otherwise false.</returns>
    /// <param name="command">Command.</param>
    bool IsCommandActivated(Command command);
  }
}
