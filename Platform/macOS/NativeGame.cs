namespace Ainomis.Platform.MacOS {
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;

  public class NativeGame : Game {
    // Private members
    private readonly MainGame _ainomisGame;

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

      // Return control to the platform-agnostic game object
      _ainomisGame = new MainGame(this, touchEnabled: false);
    }

    // Class fields
    public GraphicsDeviceManager Graphics { get; private set; }

    protected override void Initialize() {
      _ainomisGame.Initialize();
      base.Initialize();
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
