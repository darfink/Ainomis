namespace Ainomis.Platform.Ios {
  using Ainomis.Shared.Input;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;

  using GameAction = Ainomis.Game.Action;

  public class NativeGame : Game {
    // Private members
    private readonly MainGame _ainomisGame;

    public NativeGame() {
      // Create the graphics device
      this.Graphics = new GraphicsDeviceManager(this) {
        IsFullScreen = Settings.Fullscreen,
        PreferMultiSampling = true,
        SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
        SynchronizeWithVerticalRetrace = true,
      };

      var kab = new InputActionBinder<GameAction>();

      // Return control to the platform-agnostic game object
      _ainomisGame = new MainGame(this, kab);
    }

    // Class fields
    public GraphicsDeviceManager Graphics { get; private set; }

    protected override void Initialize() {
      _ainomisGame.Initialize();
      base.Initialize();
    }

    protected override void LoadContent() {
      _ainomisGame.LoadContent(Content);
    }

    protected override void UnloadContent() {
      _ainomisGame.UnloadContent(Content);
    }

    protected override void Update(GameTime gameTime) {
      _ainomisGame.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      _ainomisGame.Draw(gameTime);
      base.Draw(gameTime);
    }
  }
}
