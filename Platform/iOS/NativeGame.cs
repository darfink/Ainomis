namespace Ainomis.Platform.Ios {
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;

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

      // Return control to the platform-agnostic game object
      _ainomisGame = new MainGame(this, touchEnabled: true);
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
