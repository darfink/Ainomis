namespace Ainomis.Platform.MacOS {
  using System;

  using Ainomis.Shared.Input;
  using Ainomis.Shared.Input.Joystick;
  using Ainomis.Shared.Input.Keyboard;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;

  using GameAction = Ainomis.Game.Action;

  public class MacGame : Game {
    // Private members
    private readonly MainGame _ainomisGame;

    public MacGame() {
      // Create the graphics device
      this.Graphics = new GraphicsDeviceManager(this) {
        IsFullScreen = Settings.Fullscreen,
        SynchronizeWithVerticalRetrace = true,
        PreferMultiSampling = true,
      };

      var kab = new InputActionBinder<GameAction>();

      // Setup the keyboard bindings
      kab.AddInputDriver(new KeyboardDriver());
      kab.AddInputBinding(GameAction.Exit, new KeyboardBinding(Keys.Escape, TimeSpan.FromMilliseconds(100)));
      kab.AddInputBinding(GameAction.MoveUp, new KeyboardBinding(Keys.W, TimeSpan.FromMilliseconds(100)));
      kab.AddInputBinding(GameAction.MoveDown, new KeyboardBinding(Keys.S, TimeSpan.FromMilliseconds(100)));
      kab.AddInputBinding(GameAction.MoveLeft, new KeyboardBinding(Keys.A, TimeSpan.FromMilliseconds(100)));
      kab.AddInputBinding(GameAction.MoveRight, new KeyboardBinding(Keys.D, TimeSpan.FromMilliseconds(100)));
      kab.AddInputBinding(GameAction.TapUp, new KeyboardBinding(Keys.W, timeout: TimeSpan.FromMilliseconds(80)));
      kab.AddInputBinding(GameAction.TapDown, new KeyboardBinding(Keys.S, timeout: TimeSpan.FromMilliseconds(80)));
      kab.AddInputBinding(GameAction.TapLeft, new KeyboardBinding(Keys.A, timeout: TimeSpan.FromMilliseconds(80)));
      kab.AddInputBinding(GameAction.TapRight, new KeyboardBinding(Keys.D, timeout: TimeSpan.FromMilliseconds(80)));

      // Setup the joystick bindings if a controller is connected
      if (JoystickDriver.IsConnected(PlayerIndex.One)) {
        kab.AddInputDriver(new JoystickDriver(PlayerIndex.One));
        kab.AddInputBinding(GameAction.Start, new JoystickBinding(Buttons.Start));
      }

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
