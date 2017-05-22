namespace Ainomis.Game.States.Menu {
  using Ainomis.Game.Manager;
  using Ainomis.Game.Systems;
  using Ainomis.Game.Components;
  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Input;
  using Ainomis.Shared.Display;

  using Artemis;
  using Artemis.Manager;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  using Common = Shared.Common;
  using GameAction = Game.Action;

  class MenuState : GameState, Common.IDrawable, Common.IUpdateable {
    // Private members
    private Camera2D _camera;
    private SpriteFont _font;

    public MenuState(
      ContentManager content,
      GameStateManager gameStateManager,
      SpriteBatch spriteBatch,
      KeyActionBinder<GameAction> keyActionBinder,
      IDisplayInfo displayInfo) :
      base(content, gameStateManager, spriteBatch, keyActionBinder, displayInfo) {
    }

    public override void Enter() {
      var entity = EntityWorld.CreateEntity();
      var texture = Content.Load<Texture2D>("Menu/Background");
      _font = Content.Load<SpriteFont>("Menu/Label");

      entity.AddComponent(new TextureComponent(texture));
      entity.AddComponent(new TransformComponent(0f, 0f));
      entity.AddComponent(new SpriteComponent() {
        // Stretch the image so it fits 1:1 to the screen area
        Scale = DisplayInfo.VirtualResolution / new Vector2(texture.Width, texture.Height),
      });

      entity.Refresh();
    }

    public override void Exit() {
      // Free all entity memory
      EntityWorld.Clear();

      // Free our resources
      base.Exit();
    }

    public void Update(GameTime gameTime) {
      _camera.Update(gameTime);
      EntityWorld.Update();
    }

    public void Draw(GameTime gameTime) {
      SpriteBatch.Begin(_camera.Transform);
      EntityWorld.Draw();
      SpriteBatch.DrawString(
        _font,
        "Press any button to start",
        (Vector2) DisplayInfo.VirtualResolution / 2f,
        Color.WhiteSmoke);
      SpriteBatch.End();
    }

    protected override void SetupSystems(SystemManager manager) {
      // The camera needs to be created first since some systems may use it
      _camera = new Camera2D(DisplayInfo.RelativeScale);

      manager.SetSystem(new BasicRenderSystem(SpriteBatch), GameLoopType.Draw);
      manager.SetSystem(new TranslationSystem(), GameLoopType.Update);
    }
  }
}