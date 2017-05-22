namespace Ainomis {
  using Ainomis.Extensions;
  using Ainomis.Game.Manager;
  using Ainomis.Shared;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Input;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  using SimpleInjector;
  using SimpleInjector.Diagnostics;

  using GameAction = Game.Action;
  using GameStates = Game.States;
  using XnaGame = Microsoft.Xna.Framework.Game;

  /// <summary>
  /// Game bootstrapper
  /// </summary>
  public class MainGame {
    // Private members
    private readonly KeyActionBinder<GameAction> _keyActionBinder;
    private readonly XnaGame _game;
    private GameStateManager _gameStateManager;
    private GameStateStack _gameStateStack;
    private DisplayInfo _displayInfo;
    private SpriteBatch _spriteBatch;

    public MainGame(XnaGame game, KeyActionBinder<GameAction> keyActionBinder) {
      // Use the same resource prefix regardless of platform
      game.Content.RootDirectory = Settings.ResourcePrefix;

      _gameStateStack = new GameStateStack();
      _keyActionBinder = keyActionBinder.ThrowIfNull(nameof(keyActionBinder));
      _game = game.ThrowIfNull(nameof(game));
    }

    public void Initialize() {
      // Setup game settings
      _game.IsMouseVisible = true;
      _game.Window.Title = "Ainomis";

      // Create the primary sprite batch
      _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

      // Setup the game's display settings
      _displayInfo = new DisplayInfo(
        new Size(
          Settings.VirtualResolutionWidth,
          Settings.VirtualResolutionHeight),
        new Size(
          (uint)_game.GraphicsDevice.PresentationParameters.BackBufferWidth,
          (uint)_game.GraphicsDevice.PresentationParameters.BackBufferHeight));

      // Setup the game state factory
      InitializeFactory();

      // The first game state is the menu
      _gameStateManager.Push<GameStates.Menu.MenuState>();
    }

    public void LoadContent(ContentManager content) {
    }

    public void UnloadContent(ContentManager content) {
    }

    public void Update(GameTime gameTime) {
      // Allow each key binding system to update
      _keyActionBinder.Update(gameTime);

      // Let the game state manager update the rest
      _gameStateStack.Update(gameTime);
    }

    public void Draw(GameTime gameTime) {
      // Clear the backbuffer
      _game.GraphicsDevice.Clear(Color.Black);

      // Let the state manager handle the rendering
      _gameStateStack.Draw(gameTime);
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
      container.Register(() => _keyActionBinder, Lifestyle.Singleton);
      container.Register<IDisplayInfo>(() => _displayInfo, Lifestyle.Singleton);

      // Register all game states
      container.Register<GameStates.Menu.MenuState>();

      // Dispose is never called on the content managers
      var registration = container.GetRegistration(typeof(ContentManager)).Registration;
      registration.SuppressDiagnosticWarning(
        DiagnosticType.DisposableTransientComponent,
        "ContentManager is unloaded automatically by GameStateManager");

      // Ensure the setup is correct
      container.Verify();
    }
  }
}
