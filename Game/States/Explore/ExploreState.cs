namespace Ainomis.Game.States.Explore {
  using System;
  using System.IO;

  using Ainomis.Extensions;
  using Ainomis.Game.Components;
  using Ainomis.Game.Manager;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Game.States.Explore.Systems;
  using Ainomis.Game.Systems;
  using Ainomis.Shared;
  using Ainomis.Shared.Camera;
  using Ainomis.Shared.Command;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Utility;

  using Artemis;
  using Artemis.Manager;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using Common = Shared.Common;

  internal class ExploreState : GameState, Common.IDrawable, Common.IUpdateable {
    private Camera2D _camera;
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
      var falletTown = LoadArea(EntityWorld.CreateEntity(), "FalletTown");
      var area = falletTown.GetComponent<AreaComponent>();
      falletTown.Refresh();

      _playerEntity = LoadCharacter(EntityWorld.CreateEntity(), "MaleProtagonist");
      _playerEntity.AddComponent(new ControlComponent(CommandSystem));
      _playerEntity.AddComponent(new TileComponent(area, 0));
      _playerEntity.Refresh();

      // Focus the camera on the player
      _camera.Focus = _playerEntity.GetComponent<TransformComponent>();
    }

    public void Update(GameTime gameTime) {
      _camera.Update(gameTime);
      EntityWorld.Update();
    }

    public void Draw(GameTime gameTime) {
      SpriteBatch.Begin(_camera.Transform);
      EntityWorld.Draw();
      SpriteBatch.End();
    }

    protected override void SetupSystems(SystemManager manager) {
      // TODO: Add the explore state scale as a setting
      _camera = new Camera2D(DisplayInfo.RelativeScale) {
        ScreenCenter = (Vector2)DisplayInfo.VirtualResolution / 2,
        Scale = 4f
      };

      var renderArea = new RenderArea(DisplayInfo, _camera);

      manager.SetSystem(new AreaRenderSystem(SpriteBatch, renderArea), GameLoopType.Draw);
      manager.SetSystem(new BasicRenderSystem(SpriteBatch), GameLoopType.Draw);

      manager.SetSystem(new TranslationSystem(), GameLoopType.Update);
      manager.SetSystem(new StateAnimationSystem(), GameLoopType.Update);
      manager.SetSystem(new TileMovementSystem(), GameLoopType.Update);
      manager.SetSystem(new AnimationSystem(), GameLoopType.Update);
    }

    private Entity LoadCharacter(Entity entity, string name) {
      var directory = Path.Combine("Explore/Characters", name);
      var character = Assets.LoadJson<Resources.Character>(Path.Combine(directory, "Character.json"));

      if (string.IsNullOrWhiteSpace(character.Tileset?.Image)) {
        throw new ArgumentException("Invalid character; no associated tilesets", nameof(name));
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

    private Entity LoadArea(Entity entity, string name) {
      var directory = Path.Combine("Explore/Areas", name);
      var area = Assets.LoadJson<AreaComponent>(Path.Combine(directory, "Area.json"));

      if (area.Tilesets.Count == 0) {
        throw new ArgumentException("Invalid area; no associated tilesets", nameof(name));
      }

      if (!string.IsNullOrWhiteSpace(area.Properties.MusicTheme)) {
        var themePath = Path.Combine(directory, Path.ChangeExtension(area.Properties.MusicTheme, null));
        var themeMusic = Content.Load<Song>(themePath);
        MediaPlayer.Play(themeMusic);
      }

      var tilesetPath = Path.Combine(directory, Path.ChangeExtension(area.Tilesets[0].Image, null));
      var tilesetTexture = Content.Load<Texture2D>(tilesetPath);

      entity.AddComponent(new TransformComponent());
      entity.AddComponent(new TextureComponent(tilesetTexture));
      entity.AddComponent(area);

      return entity;
    }
  }
}
