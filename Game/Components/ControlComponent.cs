namespace Ainomis.Game.Components {
  using Ainomis.Extensions;
  using Ainomis.Shared.Command;

  using Artemis.Interface;

  /// <summary>Control component.</summary>
  internal class ControlComponent : ICommandSource, IComponent {
    private ICommandSource _commandSystem;

    /// <summary>Constructs a new control component.</summary>
    public ControlComponent(ICommandSource system) =>
      _commandSystem = system.ThrowIfNull(nameof(system));

    /// <inheritdoc />
    public bool IsCommandActivated(Command command) =>
      _commandSystem.IsCommandActivated(command);
  }
}
