namespace Ainomis.Game.Manager {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;
  using Ainomis.Shared.State;

  using Microsoft.Xna.Framework;

  using IDrawable = Shared.Common.IDrawable;
  using IUpdateable = Shared.Common.IUpdateable;

  /// <summary>
  ///   Stacked game state manager that forwards Draw() and Update() calls.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     This game state manager will call the Updateable.Update() and
  ///     Drawable.Draw() methods when any of those interfaces are implemented
  ///     by the game states pushed onto its stack.
  ///   </para>
  ///   <para>
  ///     To figure out whether a game state implements these interfaces, a
  ///     dynamic_cast will be performed during the Push() call. All active
  ///     updateables and drawables are then kept in separate list, allowing
  ///     very cheap frame-by-frame processing.
  ///   </para>
  /// </remarks>
  internal class GameStateStack : GameStateStackBase<GameState>, IDrawable, IUpdateable {
    private Stack<GameStateModalityPair> _stateStack = new Stack<GameStateModalityPair>();

    /// <summary>Calls <see cref="Clear">Clear</see>.</summary>
    ~GameStateStack() => Clear();

    /// <summary>Returns all states that are completely, or partially exposed.</summary>
    private IEnumerable<GameState> ExposedStates => _stateStack
      .TakeWhileInclusive(pair => pair.Modality != Modality.Exclusive)
      .Select(pair => pair.State);

    /// <summary>Removes all game states from the stack.</summary>
    public void Clear() {
      while(_stateStack.Count > 0) {
        this.Pop();
      }
    }

    /// <inheritdoc />
    public override GameState Peek() => _stateStack.LastOrDefault()?.State;

    /// <inheritdoc />
    public override void Push(GameState state, Modality modality = Modality.Exclusive) {
      state.ThrowIfNull(nameof(state));

      foreach (var exposedState in ExposedStates) {
        exposedState.Obscure(modality == Modality.Exclusive);
      }

      _stateStack.Push(new GameStateModalityPair(state, modality));
      state.Enter();
    }

    /// <inheritdoc />
    public override GameState Pop() {
      if (_stateStack.Count == 0) {
        throw new InvalidOperationException();
      }

      var popped = _stateStack.Pop();
      popped.State.Exit();

      bool completelyRevealed = true;
      foreach (var exposedState in ExposedStates) {
        exposedState.Reveal(completelyRevealed);
        completelyRevealed = false;
      }

      return popped.State;
    }

    /// <summary>Updates all exposed game states.</summary>
    public void Update(GameTime gameTime) {
      var exposedUpdateables = ExposedStates.OfType<IUpdateable>();

      // Since game states may very well try to change/switch, push or pop
      // states while they are updating. To prevent an exception from being
      // thrown, force evalution of the LINQ-expression by using Count().
      var exposedCount = exposedUpdateables.Count();

      foreach(var updateable in exposedUpdateables) {
        updateable.Update(gameTime);
      }
    }

    /// <summary>Renders all exposed game states.</summary>
    public void Draw(GameTime gameTime) {
      foreach(var drawable in ExposedStates.OfType<IDrawable>()) {
        drawable.Draw(gameTime);
      }
    }

    /// <summary>Stores a game state and the modality it was activated with.</summary>
    internal class GameStateModalityPair : Tuple<GameState, Modality> {
      /// <summary>Constructs a new pair with a state and modality.</summary>
      public GameStateModalityPair(GameState state, Modality modality) : base(state, modality) {
      }

      /// <summary>Returns the game state.</summary>
      public GameState State => Item1;

      /// <summary>Returns the game state's modality.</summary>
      public Modality Modality => Item2;
    }
  }
}
