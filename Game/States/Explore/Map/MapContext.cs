namespace Ainomis.Game.States.Explore.Map {
  using System;
  using System.IO;

  using Ainomis.Extensions;
  using Ainomis.Game.Components;
  using Ainomis.Game.Resources;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Shared;
  using Ainomis.Shared.Viewport;
  using Ainomis.Shared.Utility;

  using Artemis;
  using Artemis.Interface;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Content;

  using Common = Shared.Common;

  internal class MapContext : IDisposable, Common.IDrawable {
    private ContentManager _content;
    private MapRenderer _renderer;
    private MapController _controller;
    private Map _map;

    /// <summary>Constructs a new map context.</summary>
    public MapContext(
        SpriteBatch spriteBatch,
        IServiceProvider serviceProvider,
        IViewportProvider viewportProvider,
        string name) {
      var directory = Path.Combine("Explore/Maps", name);

      _map = Assets.LoadJson<Map>(Path.Combine(directory, "Map.json"));
      _content = new ContentManager(serviceProvider, Path.Combine(Settings.ResourcePrefix, directory));
      _renderer = new MapRenderer(spriteBatch, viewportProvider, _content, _map);
      _controller = new MapController(_map);
    }

    /// <summary>Associates a component with the map.</summary>
    public void Associate(Entity entity, uint tile = 0) {
      entity.AddComponent(new NodeComponent(_controller, tile));
      var transform = entity.GetComponent<TransformComponent>().ThrowIfNull(nameof(entity));
      transform.Position = _controller.GetTileOffset(tile).ToVector2();
    }

    /// <inheritdoc />
    public void Draw(GameTime gameTime) => _renderer.Draw(gameTime);

    /// <summary>Release the map's associated resources.</summary>
    public void Dispose() => _content.Dispose();
  }
}
