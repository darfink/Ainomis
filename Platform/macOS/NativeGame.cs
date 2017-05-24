namespace Ainomis.Platform.MacOS {
  using System;

  using Ainomis.Shared.Input;
  using Ainomis.Shared.Input.GamePad;
  using Ainomis.Shared.Input.Keyboard;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  using GameAction = Ainomis.Game.Action;

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

      var iab = new InputActionBinder<GameAction>();

      // Always configure keyboard input on desktop
			iab.AddInputDriver(new KeyboardDriver());
      ConfigureKeyboardInput(iab);

      // Configure the game pad if one is connected
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

    private void ConfigureKeyboardInput(InputActionBinder<GameAction> iab) {
      iab.AddInputBinding(GameAction.Exit, new KeyboardBinding(Keys.Escape, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(GameAction.MoveUp, new KeyboardBinding(Keys.W, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(GameAction.MoveDown, new KeyboardBinding(Keys.S, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(GameAction.MoveLeft, new KeyboardBinding(Keys.A, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(GameAction.MoveRight, new KeyboardBinding(Keys.D, TimeSpan.FromMilliseconds(100)));
      iab.AddInputBinding(GameAction.TapUp, new KeyboardBinding(Keys.W, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(GameAction.TapDown, new KeyboardBinding(Keys.S, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(GameAction.TapLeft, new KeyboardBinding(Keys.A, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(GameAction.TapRight, new KeyboardBinding(Keys.D, timeout: TimeSpan.FromMilliseconds(80)));
    }

    private void ConfigureGamePadInput(InputActionBinder<GameAction> iab) {
      iab.AddInputBinding(GameAction.Start, new GamePadBinding(Buttons.Start));
    }
  }
}
