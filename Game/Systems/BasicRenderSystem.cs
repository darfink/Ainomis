namespace Ainomis.Game.Systems {
  using Ainomis.Extensions;
  using Ainomis.Game.Components;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>Basic render system.</summary>
  internal class BasicRenderSystem : EntityProcessingSystem {
    // To be included in the basic render system, an entity must include a
    // texture and a transform component.
    public static readonly Aspect Requirement = Aspect
      .All(typeof(TextureComponent), typeof(TransformComponent))
      .GetExclude(typeof(States.Explore.Components.AreaComponent));

    // Class properties
    private SpriteBatch _spriteBatch;
    private SpriteComponent _defaultSprite;

    public BasicRenderSystem(SpriteBatch spriteBatch) : base(Requirement) {
      _defaultSprite = new SpriteComponent();
      _spriteBatch = spriteBatch.ThrowIfNull(nameof(spriteBatch));
    }

    public override void Process(Entity entity) {
      var transform = entity.GetComponent<TransformComponent>();
      var texture = entity.GetComponent<TextureComponent>();

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
