namespace Ainomis.Game.States.Explore {
  using Ainomis.Game.Components;
  using Ainomis.Game.Manager;
  using Ainomis.Game.Systems;
  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Input;

  using Artemis;
  using Artemis.Manager;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using Common = Shared.Common;
  using GameAction = Game.Action;

  internal class ExploreState : GameState, Common.IDrawable, Common.IUpdateable {
    public ExploreState(
      ContentManager content,
      GameStateManager gameStateManager,
      SpriteBatch spriteBatch,
      KeyActionBinder<GameAction> keyActionBinder,
      IDisplayInfo displayInfo) :
      base(content, gameStateManager, spriteBatch, keyActionBinder, displayInfo) {
    }

    public override void Enter() {
    }

    public void Update(GameTime gameTime) {
      EntityWorld.Update();
    }

    public void Draw(GameTime gameTime) {
      SpriteBatch.Begin();
      EntityWorld.Draw();
      SpriteBatch.End();
    }

    protected override void SetupSystems(SystemManager manager) {
    }
  }
}