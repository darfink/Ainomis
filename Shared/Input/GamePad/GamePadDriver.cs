namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  /// <summary>Game pad driver for controllers.</summary>
  public class GamePadDriver : GamePadDriverBase {
    private PlayerIndex _playerIndex;

    /// <summary>Constructs a new game pad driver for a specific player.</summary>
    public GamePadDriver(PlayerIndex player) : base() => _playerIndex = player;

    /// <summary>Returns whether a player's game pad is connected or not.</summary>
    public static bool IsConnected(PlayerIndex player) => GamePad.GetState(player).IsConnected;

    /// <inheritdoc />
    protected override GamePadState GetCurrentState() => GamePad.GetState(_playerIndex);
  }
}
