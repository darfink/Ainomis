namespace Ainomis.Game.Manager {
  using Ainomis.Shared.State;

  using SimpleInjector;

  internal class GameStateManager {
    private readonly Container _container;
    private readonly GameStateStack _stateStack;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Ainomis.Game.Manager.GameStateManager"/> class.
    /// </summary>
    /// <param name="stateStack">State manager.</param>
    /// <param name="container">Container.</param>
    public GameStateManager(GameStateStack stateStack, Container container) {
      _stateStack = stateStack;
      _container = container;
    }

    /// <summary>
    /// Pushes a new game state on to the stack.
    /// </summary>
    /// <param name="modality">Modality.</param>
    /// <typeparam name="T">The game state type.</typeparam>
    public void Push<T>(Modality modality = Modality.Exclusive) where T : GameState =>
      _stateStack.Push(_container.GetInstance<T>(), modality);

    /// <summary>
    /// Pops the current game state from the stack.
    /// </summary>
    /// <returns>The old game state.</returns>
    public GameState Pop() => _stateStack.Pop() as GameState;

    /// <summary>
    /// Switches the current game state with another.
    /// </summary>
    /// <returns>The replaced game state.</returns>
    /// <param name="modality">Modality.</param>
    /// <typeparam name="T">The game state type.</typeparam>
    public GameState Switch<T>(Modality modality = Modality.Exclusive) where T : GameState =>
      _stateStack.Switch(_container.GetInstance<T>(), modality) as GameState;
  }
}