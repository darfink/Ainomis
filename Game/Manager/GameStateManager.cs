namespace Ainomis.Game.Manager {
  using System;

  using Ainomis.Shared.State;

  using SimpleInjector;

  internal class GameStateManager {
    private readonly Container _container;
    private readonly GameStateStack _stateStack;

    /// <summary>Constructs a new instance of the game state manager class.</summary>
    /// <param name="stateStack">State manager.</param>
    /// <param name="container">Container.</param>
    public GameStateManager(GameStateStack stateStack, Container container) {
      _stateStack = stateStack;
      _container = container;
    }

    /// <summary>Event handler for whenever the current game state is changed.</summary>
    public event EventHandler<EventArgs> Changed;

    /// <summary>Pushes a new game state on to the stack.</summary>
    /// <param name="modality">Modality.</param>
    /// <typeparam name="T">The game state type.</typeparam>
    public void Push<T>(Modality modality = Modality.Exclusive) where T : GameState {
      _stateStack.Push(_container.GetInstance<T>(), modality);
      Changed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Pops the current game state from the stack.</summary>
    /// <returns>The old game state.</returns>
    public GameState Pop() {
      var result = _stateStack.Pop();
      Changed?.Invoke(this, EventArgs.Empty);
      return result;
    }

    /// <summary>Switches the current game state with another.</summary>
    /// <returns>The replaced game state.</returns>
    /// <param name="modality">Modality.</param>
    /// <typeparam name="T">The game state type.</typeparam>
    public GameState Switch<T>(Modality modality = Modality.Exclusive) where T : GameState {
      var result = _stateStack.Switch(_container.GetInstance<T>(), modality);
      Changed?.Invoke(this, EventArgs.Empty);
      return result;
    }
  }
}
