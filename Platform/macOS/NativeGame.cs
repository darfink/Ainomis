namespace Ainomis.Platform.MacOS {
  using System;

  using Ainomis.Shared.Command;
  using Ainomis.Shared.Input;
  using Ainomis.Shared.Input.GamePad;
  using Ainomis.Shared.Input.Keyboard;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

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

      var iab = new InputActionBinder();

      // Always configure keyboard input on desktop
      iab.AddInputDriver(new KeyboardDriver());
      ConfigureKeyboardInput(iab);

      // Configure the game pad only if one is connected
      if (GamePadDriver.IsConnected(PlayerIndex.One)) {
        iab.AddInputDriver(new GamePadDriver(PlayerIndex.One));
        ConfigureGamePadInput(iab);
      }

      // Return control to the platform-agnostic game object
      _ainomisGame = new MainGame(this, iab);
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

    private void ConfigureKeyboardInput(InputActionBinder iab) {
      iab.AddInputBinding(Command.Exit, new KeyboardBinding(Keys.Escape, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(Command.RunUp, new KeyboardBinding(Keys.W, TimeSpan.FromMilliseconds(100), modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunDown, new KeyboardBinding(Keys.S, TimeSpan.FromMilliseconds(100), modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunLeft, new KeyboardBinding(Keys.A, TimeSpan.FromMilliseconds(100), modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunRight, new KeyboardBinding(Keys.D, TimeSpan.FromMilliseconds(100), modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.MoveUp, new KeyboardBinding(Keys.W, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(Command.MoveDown, new KeyboardBinding(Keys.S, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(Command.MoveLeft, new KeyboardBinding(Keys.A, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(Command.MoveRight, new KeyboardBinding(Keys.D, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(Command.TapUp, new KeyboardBinding(Keys.W, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapDown, new KeyboardBinding(Keys.S, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapLeft, new KeyboardBinding(Keys.A, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapRight, new KeyboardBinding(Keys.D, timeout: TimeSpan.FromMilliseconds(80)));
    }

    private void ConfigureGamePadInput(InputActionBinder iab) {
      iab.AddInputBinding(Command.Start, new GamePadBinding(Buttons.Start));
    }
  }
}
