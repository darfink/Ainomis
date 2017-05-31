namespace Ainomis.Platform.MacOS {
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;

  public class NativeGame : Game {
    private GameCore _gameCore;

    public NativeGame() {
      // Create the graphics device
      this.Graphics = new GraphicsDeviceManager(this) {
        IsFullScreen = Settings.Fullscreen,
        SynchronizeWithVerticalRetrace = true,
        PreferMultiSampling = true,
      };

#if FNA
      // The FNA framework does not use the platform-specific folder by default
      Settings.PlatformResourcePrefix = "../Resources";
#endif
    }

    public GraphicsDeviceManager Graphics { get; private set; }

    protected override void Initialize() {
      // Return control to the platform-agnostic game object
      _gameCore = new GameCore(this, touchEnabled: false);
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
