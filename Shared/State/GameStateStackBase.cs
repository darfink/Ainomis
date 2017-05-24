namespace Ainomis.Shared.State {
  using Ainomis.Extensions;

  internal abstract class GameStateStackBase<T> where T : class, IGameState {
    /// <summary>Returns the currently active game state</summary>
    /// <returns>The lastmost game state on the stack</returns>
    public abstract T Peek();

    /// <summary>Appends a new game state to the stack</summary>
    /// <param name="state">Game state that will be pushed on the stack</param>
    /// <param name="modality">Whether the state hides the other below</param>
    public abstract void Push(T state, Modality modality = Modality.Exclusive);

    /// <summary>Removes the lastmost game state from the stack</summary>
    /// <returns>The state that has been removed from the stack</returns>
    public abstract T Pop();

    /// <summary>Replaces the lastmost game state on the stack</summary>
    /// <param name="state">State the current will be replaced with</param>
    /// <param name="modality">Whether the state hides the other below</param>
    /// <returns>The previously state on the stack that was replaced</returns>
    /// <remarks>
    ///   This method is mostly just syntactic sugar for a call to Pop()
    ///   followed by Push(), except that it will also work if the game state
    ///   stack is currently empty, in which case it will equal the Push()
    ///   method and return an empty pointer.
    /// </remarks>
    public virtual T Switch(T state, Modality modality = Modality.Exclusive) {
      state.ThrowIfNull(nameof(state));
      T currentState = this.Peek();

      if(currentState != null) {
        this.Pop();
      }

      this.Push(state, modality);
      return currentState;
    }
  }
}
