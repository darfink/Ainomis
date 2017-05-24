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
    // Private members
    private List<GameStateModalityPair> _activeStates;
    private List<IUpdateable> _exposedUpdateables;
    private List<IDrawable> _exposedDrawables;

    public GameStateStack() {
      _activeStates = new List<GameStateModalityPair>();
      _exposedUpdateables = new List<IUpdateable>();
      _exposedDrawables = new List<IDrawable>();
    }

    ~GameStateStack() => Clear();

    public void Clear() {
      while(_activeStates.Count > 0) {
        this.Pop();
      }
    }

    /// <inheritdoc />
    public override GameState Peek() => _activeStates.LastOrDefault()?.State;

    /// <inheritdoc />
    public override void Push(
        GameState state,
        Modality modality = Modality.Exclusive) {
      state.ThrowIfNull(nameof(state));
      _activeStates.Add(new GameStateModalityPair(state, modality));

      if(modality == Modality.Exclusive) {
        _exposedUpdateables.Clear();
        _exposedDrawables.Clear();
      }

      this.AddToEntityList(state);
      this.NotifyObscuredStates();

      state.Enter();
    }

    /// <inheritdoc />
    public override GameState Pop() {
      if (_activeStates.Count == 0) {
        throw new InvalidOperationException();
      }

      GameStateModalityPair popped = _activeStates.Last();
      popped.State.Exit();
      _activeStates.RemoveAt(_activeStates.Count - 1);

      if(popped.Modality == Modality.Exclusive) {
        this.RebuildEntityQueues();
      } else {
        this.RemoveFromEntities(popped.State);
      }

      this.NotifyRevealedStates();
      return popped.State;
    }

    public void Update(GameTime gameTime) {
      // We need to use a for-loop instead of foreach, because game states may
      // very well try to change/switch, push or pop states while they are
      // updating. To prevent an exception from being thrown, we use a for-loop.
      for(int i = 0; i < _exposedUpdateables.Count; i++) {
        _exposedUpdateables[i].Update(gameTime);
      }
    }

    public void Draw(GameTime gameTime) {
      foreach(IDrawable drawable in _exposedDrawables) {
        drawable.Draw(gameTime);
      }
    }

    private void AddToEntityList(GameState state) {
      IDrawable drawable = state as IDrawable;

      if(drawable != null) {
        _exposedDrawables.Add(drawable);
      }

      IUpdateable updateable = state as IUpdateable;

      if(updateable != null) {
        _exposedUpdateables.Add(updateable);
      }
    }

    private void RemoveFromEntities(GameState state) {
      var drawable = state as IDrawable;

      if(drawable != null) {
        _exposedDrawables.RemoveAt(_exposedDrawables.Count - 1);
      }

      var updateable = state as IUpdateable;

      if(updateable != null) {
        _exposedUpdateables.RemoveAt(_exposedUpdateables.Count - 1);
      }
    }

    private void RebuildEntityQueues() {
      _exposedUpdateables.Clear();
      _exposedDrawables.Clear();

      if(_activeStates.Count == 0) {
        return;
      }

      var entityStates = _activeStates
        .AsEnumerable()
        .Reverse()
        .TakeWhileInclusive(pair => pair.Modality != Modality.Exclusive);

      foreach(var pair in entityStates) {
        this.AddToEntityList(pair.State);
      }
    }

    /// <summary>
    ///   Notifies states that have been obscured by a new state.
    /// </summary>
    /// <remarks>
    ///   This method notifies states that have been obscured by a new state.
    ///   It does not notify all states, but only those who are "above" an
    ///   exclusive state. The algorithm is inclusive, so the state with
    ///   exclusive modality is also notified by the obscured call.
    /// </remarks>
    private void NotifyObscuredStates() {
      if(_activeStates.Count <= 1) {
        return;
      }

      var obscuredStates = _activeStates
        .Take(_activeStates.Count - 1)
        .Reverse()
        .TakeWhileInclusive(pair => pair.Modality != Modality.Exclusive);

      // We want to notify the states if they have been completely obscured
      bool completelyObscured = _activeStates.Last().Modality == Modality.Exclusive;

      foreach(var pair in obscuredStates) {
        pair.State.Obscure(completelyObscured);
      }
    }

    /// <summary>
    ///   Notifies states that have been revealed because of a removed state.
    /// </summary>
    private void NotifyRevealedStates() {
      if(_activeStates.Count == 0) {
        return;
      }

      var revealedStates = _activeStates
        .AsEnumerable()
        .Reverse()
        .TakeWhileInclusive(pair => pair.Modality != Modality.Exclusive);

      foreach(var pair in revealedStates) {
        pair.State.Reveal();
      }
    }

    /// <summary>
    ///   Stores a game state and the modality it was activated with.
    /// </summary>
    internal class GameStateModalityPair : Tuple<GameState, Modality> {
      public GameStateModalityPair(GameState state, Modality modality) : base(state, modality) {
      }

      public GameState State => Item1;

      public Modality Modality => Item2;
    }
  }
}
