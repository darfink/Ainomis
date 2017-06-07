namespace Ainomis.Game.States.Explore {
  using System;
  using System.IO;

  using Ainomis.Extensions;
  using Ainomis.Game.Components;
  using Ainomis.Game.Manager;
  using Ainomis.Game.Resources;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Game.States.Explore.Map;
  using Ainomis.Game.States.Explore.Systems;
  using Ainomis.Game.Systems;
  using Ainomis.Shared;
  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Command;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Utility;
  using Ainomis.Shared.Viewport;

  using Artemis;
  using Artemis.Manager;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using Common = Shared.Common;

  internal class ExploreState : GameState, Common.IDrawable, Common.IUpdateable {
    private Camera2D _camera;
    private MapContext _mapContext;
    private Entity _playerEntity;

    public ExploreState(
        ContentManager content,
        GameStateManager gameStateManager,
        SpriteBatch spriteBatch,
        ICommandSource commandSystem,
        IDisplayInfo displayInfo) :
      base(content, gameStateManager, spriteBatch, commandSystem, displayInfo) {
    }

    public override void Enter() {
      _mapContext = LoadMap("FalletTown");

      _playerEntity = InitCharacter(EntityWorld.CreateEntity(), "MaleProtagonist");
      _playerEntity.AddComponent(new ControlComponent(CommandSystem));
      _mapContext.Associate(_playerEntity, tile: 964);
      _playerEntity.Refresh();

      // Focus the camera on the player
      _camera.Focus = _playerEntity.GetComponent<TransformComponent>();
    }

    public override void Exit() {
      _mapContext.Dispose();
      base.Exit();
    }

    public void Update(GameTime gameTime) {
      _camera.Update(gameTime);
      EntityWorld.Update();
    }

    public void Draw(GameTime gameTime) {
      SpriteBatch.Begin(_camera.Transform);
      _mapContext.Draw(gameTime);
      EntityWorld.Draw();
      SpriteBatch.End();
    }

    protected override void SetupSystems(SystemManager manager) {
      // TODO: Add the explore state scale as a setting
      _camera = new Camera2D(DisplayInfo.RelativeScale) {
        ScreenCenter = (Vector2)DisplayInfo.VirtualResolution / 2,
        Scale = 4f
      };

      // Graphical systems
      manager.SetSystem(new SpriteRenderSystem(SpriteBatch), GameLoopType.Draw);

      // Logical systems
      manager.SetSystem(new TranslationSystem(), GameLoopType.Update);
      manager.SetSystem(new StateAnimationSystem(), GameLoopType.Update);
      manager.SetSystem(new TileMovementSystem(), GameLoopType.Update);
      manager.SetSystem(new AnimationSystem(), GameLoopType.Update);
    }

    private Entity InitCharacter(Entity entity, string name) {
      var directory = Path.Combine("Explore/Characters", name);
      var character = Assets.LoadJson<Resources.Character>(Path.Combine(directory, "Character.json"));

      if (string.IsNullOrWhiteSpace(character.Tileset?.Image)) {
        throw new ArgumentException("Invalid character; no associated tileset", nameof(name));
      }

      var tilesetPath = Path.Combine(directory, Path.ChangeExtension(character.Tileset.Image, null));
      var tilesetTexture = Content.Load<Texture2D>(tilesetPath);

      entity.AddComponent(new AnimationComponent(character.Animations));
      entity.AddComponent(new TilesetComponent(character.Tileset));
      entity.AddComponent(new TextureComponent(tilesetTexture));
      entity.AddComponent(new TransformComponent());
      entity.AddComponent(new SpriteComponent() { Origin = character.Origin });

      return entity;
    }

    private MapContext LoadMap(string name) => new MapContext(
        SpriteBatch, Content.ServiceProvider, new CameraDisplayView(_camera, DisplayInfo), name);
  }
}
