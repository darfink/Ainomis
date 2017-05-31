namespace Ainomis {
  using System;
  using System.Collections.Generic;

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
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Input;
  using Microsoft.Xna.Framework.Media;

  using SimpleInjector;
  using SimpleInjector.Diagnostics;

  using GameStates = Game.States;
  using XnaGame = Microsoft.Xna.Framework.Game;

  /// <summary>Game bootstrapper.</summary>
  public class GameCore {
    private readonly XnaGame _game;
    private readonly GameStateStack _gameStateStack;
    private GameStateManager _gameStateManager;
    private InputActionBinder _inputActionBinder;
    private DisplayInfo _displayInfo;
    private SpriteBatch _spriteBatch;
    private VirtualGamePad _virtualGamePad;

    public GameCore(XnaGame game, bool touchEnabled) {
      _game = game.ThrowIfNull(nameof(game));
      _game.Content.RootDirectory = Settings.ResourcePrefix;
      _game.Window.Title = "Ainomis";
      _game.IsMouseVisible = true;

      // Manually free all game states upon exit
      _game.Exiting += (sender, e) => _gameStateStack.Clear();

      // Configure the music volume accordingly
      MediaPlayer.Volume = Settings.MusicVolume;

      // Create the default sprite batch
      _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

      // Create the display information object
      var pp = _game.GraphicsDevice.PresentationParameters;
      _displayInfo = new DisplayInfo(
        new Size(Settings.VirtualResolutionWidth, Settings.VirtualResolutionHeight),
        new Size((uint)pp.BackBufferWidth, (uint)pp.BackBufferHeight));

      // Initialize the default input system
      _inputActionBinder = CreateInputSystem(touchEnabled);

      // Initialize the game state system
      var container = CreateGameStateIoC();
      _gameStateStack = new GameStateStack();
      _gameStateManager = new GameStateManager(_gameStateStack, container);
      _gameStateManager.Changed += (sender, e) => GC.Collect();

#if DEBUG
			// Ensure the container's integrity is valid
			container.Verify();
#endif

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
      // TODO: Render debug information such as GetTotalMemory
      // Clear the backbuffer
      _game.GraphicsDevice.Clear(Color.Cyan);

      // Let the state manager handle the rendering
      _gameStateStack.Draw(gameTime);
      _virtualGamePad?.Draw(gameTime);
    }

    private Container CreateGameStateIoC() {
      // Create the IoC container
      var container = new Container();

      // Register all state dependencies
      container.Register(() => _game.Content.Allocate());
      container.RegisterSingleton(() => _gameStateManager);
      container.RegisterSingleton(() => _spriteBatch);
      container.RegisterSingleton<ICommandSource>(() => _inputActionBinder);
      container.RegisterSingleton<IDisplayInfo>(() => _displayInfo);

      // Register all game states
      container.Register<GameStates.Explore.ExploreState>();
      container.Register<GameStates.Menu.MenuState>();

      // Dispose is never called on the content managers
      var registration = container.GetRegistration(typeof(ContentManager)).Registration;
      registration.SuppressDiagnosticWarning(
        DiagnosticType.DisposableTransientComponent,
        "ContentManager is unloaded automatically by GameStateManager");

      return container;
    }

    private InputActionBinder CreateInputSystem(bool touchEnabled) {
      // TODO: Dynamically handle this (e.g if a user inserts a game pad after init)
      var inputActionBinder = new InputActionBinder();

      if (!touchEnabled) {
        inputActionBinder.AddInputDriver(new KeyboardDriver());
        ConfigureKeyboardInput(inputActionBinder);

        if (GamePadDriver.IsConnected(PlayerIndex.One)) {
          inputActionBinder.AddInputDriver(new GamePadDriver(PlayerIndex.One));
          ConfigureGamePadInput(inputActionBinder);
        }
      } else {
        _virtualGamePad = new VirtualGamePad(_game.Content, _spriteBatch, _displayInfo);
        inputActionBinder.AddInputDriver(_virtualGamePad);
        ConfigureGamePadInput(inputActionBinder);
      }

      return inputActionBinder;
    }

    private void ConfigureKeyboardInput(InputActionBinder iab) {
      var deciSecond = TimeSpan.FromMilliseconds(100);
      var commandBindings = new Dictionary<Command, KeyboardBinding>() {
        { Command.Exit, new KeyboardBinding(Keys.Escape, deciSecond) },
        { Command.Start, new KeyboardBinding(Keys.Enter, deciSecond) },
        { Command.RunUp, new KeyboardBinding(Keys.W, deciSecond, modifiers: Keys.LeftShift) },
        { Command.RunDown, new KeyboardBinding(Keys.S, deciSecond, modifiers: Keys.LeftShift) },
        { Command.RunLeft, new KeyboardBinding(Keys.A, deciSecond, modifiers: Keys.LeftShift) },
        { Command.RunRight, new KeyboardBinding(Keys.D, deciSecond, modifiers: Keys.LeftShift) },
        { Command.WalkUp, new KeyboardBinding(Keys.W, deciSecond) },
        { Command.WalkDown, new KeyboardBinding(Keys.S, deciSecond) },
        { Command.WalkLeft, new KeyboardBinding(Keys.A, deciSecond) },
        { Command.WalkRight, new KeyboardBinding(Keys.D, deciSecond) },
        { Command.TapUp, new KeyboardBinding(Keys.W, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapDown, new KeyboardBinding(Keys.S, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapLeft, new KeyboardBinding(Keys.A, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapRight, new KeyboardBinding(Keys.D, timeout: TimeSpan.FromMilliseconds(80)) },
      };

      foreach(var binding in commandBindings) {
        iab.AddInputBinding(binding.Key, binding.Value);
      }
    }

    private void ConfigureGamePadInput(InputActionBinder iab) {
      var deciSecond = TimeSpan.FromMilliseconds(100);
      var commandBindings = new Dictionary<Command, GamePadBinding>() {
        { Command.Start, new GamePadBinding(Buttons.Start) },
        { Command.RunUp, new GamePadBinding(Buttons.DPadUp, deciSecond, modifiers: Buttons.B) },
        { Command.RunDown, new GamePadBinding(Buttons.DPadDown, deciSecond, modifiers: Buttons.B) },
        { Command.RunLeft, new GamePadBinding(Buttons.DPadLeft, deciSecond, modifiers: Buttons.B) },
        { Command.RunRight, new GamePadBinding(Buttons.DPadRight, deciSecond, modifiers: Buttons.B) },
        { Command.WalkUp, new GamePadBinding(Buttons.DPadUp, deciSecond) },
        { Command.WalkDown, new GamePadBinding(Buttons.DPadDown, deciSecond) },
        { Command.WalkLeft, new GamePadBinding(Buttons.DPadLeft, deciSecond) },
        { Command.WalkRight, new GamePadBinding(Buttons.DPadRight, deciSecond) },
        { Command.TapUp, new GamePadBinding(Buttons.DPadUp, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapDown, new GamePadBinding(Buttons.DPadDown, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapLeft, new GamePadBinding(Buttons.DPadLeft, timeout: TimeSpan.FromMilliseconds(80)) },
        { Command.TapRight, new GamePadBinding(Buttons.DPadRight, timeout: TimeSpan.FromMilliseconds(80)) },
      };

      foreach(var binding in commandBindings) {
        iab.AddInputBinding(binding.Key, binding.Value);
      }
    }
  }
}
