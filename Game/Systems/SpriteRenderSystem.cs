namespace Ainomis.Game.Systems {
  using Ainomis.Extensions;
  using Ainomis.Game.Components;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>Basic render system.</summary>
  internal class SpriteRenderSystem : EntityComponentProcessingSystem<TransformComponent, TextureComponent> {
    // Class properties
    private SpriteBatch _spriteBatch;
    private SpriteComponent _defaultSprite;

    public SpriteRenderSystem(SpriteBatch spriteBatch) {
      _defaultSprite = new SpriteComponent();
      _spriteBatch = spriteBatch.ThrowIfNull(nameof(spriteBatch));
    }

    public override void Process(Entity entity, TransformComponent transform, TextureComponent texture) {
      // The sprite component is not required for rendering
      var sprite = entity.GetComponent<SpriteComponent>() ?? _defaultSprite;

      // The tileset component is optional as well
      var tileset = entity.GetComponent<TilesetComponent>();

      // In case a tileset is provided, its source rectangle overrides the
      // sprite's source rectangle. All other options are used as default.
      Rectangle? sourceRectangle = tileset?.Source ?? sprite?.Source;

      _spriteBatch.Draw(
          texture.Texture,
          transform.Position + sprite.Offset,
          sourceRectangle,
          sprite.Color,
          transform.Rotation,
          sprite.Origin,
          sprite.Scale,
          sprite.Effects,
          sprite.Layer);
    }
  }
}
