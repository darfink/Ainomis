namespace Ainomis.Shared.State {
  /// <summary>
  ///   Possible behaviors of a game state relative to states below it
  /// </summary>
  internal enum Modality {
    /// <summary>
    ///   The game state takes exclusive ownership of the screen and does not
    ///   require the state below it in the stack to be updated as long as it's
    ///   active.
    /// </summary>
    Exclusive,

    /// <summary>
    ///   The game state sits on top of the state below it in the stack, but
    ///   does not completely obscure it or requires it to continue being
    ///   updated.
    /// </summary>
    Popup
  }
}
