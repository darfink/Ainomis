namespace Ainomis.Game.Systems {
  using Ainomis.Game.Components;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>
  /// Tileset render system.
  /// </summary>
  public class BasicRenderSystem : EntityProcessingSystem {
    public static readonly Aspect Requirement = Aspect
      .All(typeof(TextureComponent), typeof(TransformComponent));

    // Class properties
    private SpriteBatch _spriteBatch;
    private SpriteComponent _defaultSprite;

    // To be included in the basic render system, an entity must include a
    // texture and a transform component.
    public BasicRenderSystem(SpriteBatch spriteBatch) : base(Requirement) {
      System.Diagnostics.Debug.Assert(spriteBatch != null);
      _defaultSprite = new SpriteComponent();
      _spriteBatch = spriteBatch;
    }

    public override void Process(Entity entity) {
      TransformComponent transform = entity.GetComponent<TransformComponent>();
      TextureComponent texture = entity.GetComponent<TextureComponent>();

      // The sprite component is not required for rendering
      SpriteComponent sprite = entity.GetComponent<SpriteComponent>() ?? _defaultSprite;

      // The tileset component is optional as well
      TilesetComponent tileset = entity.GetComponent<TilesetComponent>();

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
          sprite.Mirroring,
          sprite.Layer);
    }
  }
}
