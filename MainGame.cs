namespace Ainomis {
  using System;

  using Ainomis.Extensions;
  using Ainomis.Game.Manager;
  using Ainomis.Shared;
  using Ainomis.Shared.Command;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Input;
  using Ainomis.Shared.Input.GamePad;
  using Ainomis.Shared.Input.Keyboard;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Input;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using SimpleInjector;
  using SimpleInjector.Diagnostics;

  using GameStates = Game.States;
  using XnaGame = Microsoft.Xna.Framework.Game;

  /// <summary>Game bootstrapper.</summary>
  public class MainGame {
    // Private members
    private readonly XnaGame _game;
    private readonly GameStateStack _gameStateStack;
    private GameStateManager _gameStateManager;
    private InputActionBinder _inputActionBinder;
    private DisplayInfo _displayInfo;
    private SpriteBatch _spriteBatch;
    private VirtualGamePad _virtualGamePad;
    private bool _touchEnabled;

    public MainGame(XnaGame game, bool touchEnabled) {
      _game = game.ThrowIfNull(nameof(game));
      _gameStateStack = new GameStateStack();
      _touchEnabled = touchEnabled;

      // Use the user defined music volume
      MediaPlayer.Volume = Settings.MusicVolume;

      // Use the same resource prefix regardless of platform
      _game.Content.RootDirectory = Settings.ResourcePrefix;

      // Manually dispose all states upon exit
      _game.Exiting += (sender, e) => _gameStateStack.Clear();
    }

    public void Initialize() {
      // Setup game settings
      _game.IsMouseVisible = true;
      _game.Window.Title = "Ainomis";

      // Create the primary sprite batch
      _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

      // Setup the game's display settings
      var pp = _game.GraphicsDevice.PresentationParameters;
      _displayInfo = new DisplayInfo(
        new Size(Settings.VirtualResolutionWidth, Settings.VirtualResolutionHeight),
        new Size((uint)pp.BackBufferWidth, (uint)pp.BackBufferHeight));

      // Setup the game input and factory
      InitializeInput();
      InitializeFactory();

      // The first game state is the menu
      _gameStateManager.Push<GameStates.Menu.MenuState>();
    }

    public void Update(GameTime gameTime) {
      // Allow each key binding system to update
      _inputActionBinder.Update(gameTime);

      // Let the game state manager update the rest
      _gameStateStack.Update(gameTime);
    }

    public void Draw(GameTime gameTime) {
      // Clear the backbuffer
      _game.GraphicsDevice.Clear(Color.Cyan);

      // Let the state manager handle the rendering
      _gameStateStack.Draw(gameTime);
      _virtualGamePad?.Draw(gameTime);
    }

    private void InitializeFactory() {
      // Create the IoC container
      var container = new Container();

      // Associate the manager with the to-be initialized container
      _gameStateManager = new GameStateManager(_gameStateStack, container);

      // Register all state dependencies
      container.Register(() => _game.Content.Allocate(), Lifestyle.Transient);
      container.Register(() => _gameStateManager, Lifestyle.Singleton);
      container.Register(() => _spriteBatch, Lifestyle.Singleton);
      container.Register<ICommandSource>(() => _inputActionBinder, Lifestyle.Singleton);
      container.Register<IDisplayInfo>(() => _displayInfo, Lifestyle.Singleton);

      // Register all game states
      container.Register<GameStates.Explore.ExploreState>();
      container.Register<GameStates.Menu.MenuState>();

      // Dispose is never called on the content managers
      var registration = container.GetRegistration(typeof(ContentManager)).Registration;
      registration.SuppressDiagnosticWarning(
        DiagnosticType.DisposableTransientComponent,
        "ContentManager is unloaded automatically by GameStateManager");

      // Ensure the setup is correct
      container.Verify();
    }

    private void InitializeInput() {
      // TODO: Dynamically handle this (e.g if a user inserts a game pad after init)
      _inputActionBinder = new InputActionBinder();

      if (!_touchEnabled) {
        _inputActionBinder.AddInputDriver(new KeyboardDriver());
        ConfigureKeyboardInput(_inputActionBinder);

        if (GamePadDriver.IsConnected(PlayerIndex.One)) {
          _inputActionBinder.AddInputDriver(new GamePadDriver(PlayerIndex.One));
          ConfigureGamePadInput(_inputActionBinder);
        }
      } else {
        _virtualGamePad = new VirtualGamePad(_game.Content, _spriteBatch, _displayInfo);
        _inputActionBinder.AddInputDriver(_virtualGamePad);
        ConfigureGamePadInput(_inputActionBinder);
      }
    }

    private void ConfigureKeyboardInput(InputActionBinder iab) {
      var deciSecond = TimeSpan.FromMilliseconds(100);

      iab.AddInputBinding(Command.Exit, new KeyboardBinding(Keys.Escape, deciSecond));
      iab.AddInputBinding(Command.Start, new KeyboardBinding(Keys.Enter, deciSecond));
      iab.AddInputBinding(Command.RunUp, new KeyboardBinding(Keys.W, deciSecond, modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunDown, new KeyboardBinding(Keys.S, deciSecond, modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunLeft, new KeyboardBinding(Keys.A, deciSecond, modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.RunRight, new KeyboardBinding(Keys.D, deciSecond, modifiers: Keys.LeftShift));
      iab.AddInputBinding(Command.MoveUp, new KeyboardBinding(Keys.W, deciSecond));
      iab.AddInputBinding(Command.MoveDown, new KeyboardBinding(Keys.S, deciSecond));
      iab.AddInputBinding(Command.MoveLeft, new KeyboardBinding(Keys.A, deciSecond));
      iab.AddInputBinding(Command.MoveRight, new KeyboardBinding(Keys.D, deciSecond));
      iab.AddInputBinding(Command.TapUp, new KeyboardBinding(Keys.W, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapDown, new KeyboardBinding(Keys.S, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapLeft, new KeyboardBinding(Keys.A, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapRight, new KeyboardBinding(Keys.D, timeout: TimeSpan.FromMilliseconds(80)));
    }

    private void ConfigureGamePadInput(InputActionBinder iab) {
      var deciSecond = TimeSpan.FromMilliseconds(100);

      iab.AddInputBinding(Command.Start, new GamePadBinding(Buttons.Start));
      iab.AddInputBinding(Command.RunUp, new GamePadBinding(Buttons.DPadUp | Buttons.B, deciSecond));
      iab.AddInputBinding(Command.RunDown, new GamePadBinding(Buttons.DPadDown | Buttons.B, deciSecond));
      iab.AddInputBinding(Command.RunLeft, new GamePadBinding(Buttons.DPadLeft | Buttons.B, deciSecond));
      iab.AddInputBinding(Command.RunRight, new GamePadBinding(Buttons.DPadRight | Buttons.B, deciSecond));
      iab.AddInputBinding(Command.MoveUp, new GamePadBinding(Buttons.DPadUp, deciSecond));
      iab.AddInputBinding(Command.MoveDown, new GamePadBinding(Buttons.DPadDown, deciSecond));
      iab.AddInputBinding(Command.MoveLeft, new GamePadBinding(Buttons.DPadLeft, deciSecond));
      iab.AddInputBinding(Command.MoveRight, new GamePadBinding(Buttons.DPadRight, deciSecond));
      iab.AddInputBinding(Command.MoveUp, new GamePadBinding(Buttons.DPadUp, deciSecond));
      iab.AddInputBinding(Command.MoveDown, new GamePadBinding(Buttons.DPadDown, deciSecond));
      iab.AddInputBinding(Command.MoveLeft, new GamePadBinding(Buttons.DPadLeft, deciSecond));
      iab.AddInputBinding(Command.MoveRight, new GamePadBinding(Buttons.DPadRight, deciSecond));
      iab.AddInputBinding(Command.TapUp, new GamePadBinding(Buttons.DPadUp, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapDown, new GamePadBinding(Buttons.DPadDown, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapLeft, new GamePadBinding(Buttons.DPadLeft, timeout: TimeSpan.FromMilliseconds(80)));
      iab.AddInputBinding(Command.TapRight, new GamePadBinding(Buttons.DPadRight, timeout: TimeSpan.FromMilliseconds(80)));
    }
  }
}
