namespace Ainomis.Game.States.Explore.Systems {
  using System;
  using System.Linq;

  using Ainomis.Extensions;
  using Ainomis.Game.Components;
  using Ainomis.Game.States.Explore.Components;
  using Ainomis.Shared;

  using Artemis;
  using Artemis.System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  internal class AreaRenderSystem : EntityComponentProcessingSystem<AreaComponent, TextureComponent, TransformComponent> {
    private RenderArea _renderArea;
    private SpriteBatch _spriteBatch;

    /// <summary>Creates a new area render system.</summary>
    public AreaRenderSystem(SpriteBatch spriteBatch, RenderArea renderArea) {
      _spriteBatch = spriteBatch.ThrowIfNull(nameof(spriteBatch));
      _renderArea = renderArea.ThrowIfNull(nameof(renderArea));
    }

    public override void Process(
      Entity entity,
      AreaComponent area,
      TextureComponent image,
      TransformComponent transform) {
      var areaBounds = new Rectangle(
        (int)transform.Position.X,
        (int)transform.Position.Y,
        (int)(area.Width * area.TileWidth),
        (int)(area.Height * area.TileHeight));

      // Determine whether any part of the area is visible within the camera bounds
      var visibleTiles = Rectangle.Intersect(_renderArea.GetBounds(), areaBounds);
      if (visibleTiles.IsEmpty) {
        return;
      }

      // Convert the units from screen pixels to area tiles
      visibleTiles.X = RoundDown((int)Math.Abs(visibleTiles.X - transform.Position.X), (int)area.TileWidth);
      visibleTiles.Y = RoundDown((int)Math.Abs(visibleTiles.Y - transform.Position.Y), (int)area.TileHeight);
      visibleTiles.Width = Math.Min(RoundUp(visibleTiles.Width, (int)area.TileWidth) + visibleTiles.X + 1, (int)area.Width);
      visibleTiles.Height = Math.Min(RoundUp(visibleTiles.Height, (int)area.TileHeight) + visibleTiles.Y + 1, (int)area.Height);

      // Only one tileset per area is supported at the moment
      var tileset = area.Tilesets.First();
      var tilePosition = Vector2.Zero;

      for (int z = 0; z < area.Layers.Count; z++) {
        var layer = area.Layers[z];
        var layerDepth = 1f - (z / (float)area.Layers.Count);
        var tileColor = new Color(Color.White, layer.Opacity);

        for (int y = visibleTiles.Top; y < visibleTiles.Height; y++) {
          // Calculate the tile's vertical position
          tilePosition.Y = transform.Position.Y + (y * area.TileHeight);
          int verticalIndex = y * (int)area.Width;

          for (int x = visibleTiles.Left; x < visibleTiles.Width; x++) {
            // Transparent tiles have an index of zero
            uint tileIndex = layer.Data[verticalIndex + x];

            if (tileIndex == 0) {
              continue;
            }

            // Calculate the tile's horizontal position
            tilePosition.X = transform.Position.X + (x * area.TileWidth);

            // Retrieve the source used for the tileset texture
            Rectangle tileSource = tileset.GetSourceRectangleForTile(tileIndex);

            _spriteBatch.Draw(
              image.Texture,
              tilePosition,
              tileSource,
              tileColor,
              0f,
              Vector2.Zero,
              Vector2.One,
              SpriteEffects.None,
              layerDepth);
          }
        }
      }
    }

    /// <summary>Rounds a number up to a closest multiple.</summary>
    private int RoundUp(int toRound, int multiple) =>
      ((multiple - (toRound % multiple)) + toRound) / multiple;

    /// <summary>Rounds a number down to a closest multiple.</summary>
    private int RoundDown(int toRound, int multiple) =>
      (toRound - (toRound % multiple)) / multiple;
  }
}
