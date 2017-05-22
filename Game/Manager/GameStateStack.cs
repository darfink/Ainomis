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
  internal class GameStateStack : GameStateStackBase, IDrawable, IUpdateable {
    // Private members
    private List<GameStateModalityPair> mActiveStates;
    private List<IUpdateable> mExposedUpdateables;
    private List<IDrawable> mExposedDrawables;

    public GameStateStack() {
      mActiveStates = new List<GameStateModalityPair>();
      mExposedUpdateables = new List<IUpdateable>();
      mExposedDrawables = new List<IDrawable>();
    }

    ~GameStateStack() {
      while(mActiveStates.Count > 0) {
        this.Pop();
      }
    }

    public override IGameState Peek() {
      if(mActiveStates.Count == 0) {
        return null;
      } else {
        return mActiveStates.Last().State;
      }
    }

    public override void Push(
        IGameState state,
        Modality modality = Modality.Exclusive) {
      state.ThrowIfNull(nameof(state));
      mActiveStates.Add(new GameStateModalityPair(state as GameState, modality));

      if(modality == Modality.Exclusive) {
        mExposedUpdateables.Clear();
        mExposedDrawables.Clear();
      }

      this.AddToEntityList(state);
      this.NotifyObscuredStates();

      (state as GameState).Enter();
    }

    public override IGameState Pop() {
      if (mActiveStates.Count == 0) {
        throw new InvalidOperationException();
      }

      GameStateModalityPair popped = mActiveStates.Last();
      popped.State.Exit();
      mActiveStates.RemoveAt(mActiveStates.Count - 1);

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
      for(int i = 0; i < mExposedUpdateables.Count; i++) {
        mExposedUpdateables[i].Update(gameTime);
      }
    }

    public void Draw(GameTime gameTime) {
      foreach(IDrawable drawable in mExposedDrawables) {
        drawable.Draw(gameTime);
      }
    }

    private void AddToEntityList(IGameState state) {
      IDrawable drawable = state as IDrawable;

      if(drawable != null) {
        mExposedDrawables.Add(drawable);
      }

      IUpdateable updateable = state as IUpdateable;

      if(updateable != null) {
        mExposedUpdateables.Add(updateable);
      }
    }

    private void RemoveFromEntities(IGameState state) {
      var drawable = state as IDrawable;

      if(drawable != null) {
        mExposedDrawables.RemoveAt(mExposedDrawables.Count - 1);
      }

      var updateable = state as IUpdateable;

      if(updateable != null) {
        mExposedUpdateables.RemoveAt(mExposedUpdateables.Count - 1);
      }
    }

    private void RebuildEntityQueues() {
      mExposedUpdateables.Clear();
      mExposedDrawables.Clear();

      if(mActiveStates.Count == 0) {
        return;
      }

      var entityStates = mActiveStates
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
      if(mActiveStates.Count <= 1) {
        return;
      }

      var obscuredStates = mActiveStates
        .Take(mActiveStates.Count - 1)
        .Reverse()
        .TakeWhileInclusive(pair => pair.Modality != Modality.Exclusive);

      // We want to notify the states if they have been completely obscured
      bool completelyObscured = mActiveStates.Last().Modality == Modality.Exclusive;

      foreach(var pair in obscuredStates) {
        pair.State.Obscure(completelyObscured);
      }
    }

    /// <summary>
    ///   Notifies states that have been revealed because of a removed state.
    /// </summary>
    private void NotifyRevealedStates() {
      if(mActiveStates.Count == 0) {
        return;
      }

      var revealedStates = mActiveStates
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
