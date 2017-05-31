namespace Ainomis.Platform.Ios {
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;

  public class NativeGame : Game {
    private GameCore _gameCore;

    public NativeGame() {
      // Create the graphics device
      this.Graphics = new GraphicsDeviceManager(this) {
        IsFullScreen = Settings.Fullscreen,
        PreferMultiSampling = true,
        SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
        SynchronizeWithVerticalRetrace = true,
      };
    }

    public GraphicsDeviceManager Graphics { get; private set; }

    protected override void Initialize() {
      // Return control to the platform-agnostic game object
      _gameCore = new GameCore(this, touchEnabled: true);
      base.Initialize();
    }

    protected override void Update(GameTime gameTime) {
      _gameCore.Update(gameTime);
      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
      _gameCore.Draw(gameTime);
      base.Draw(gameTime);
    }
  }
}
