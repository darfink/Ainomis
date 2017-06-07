namespace Ainomis.Game.States.Explore.Map {
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  using Ainomis.Extensions;
  using Ainomis.Game.Resources;
  using Ainomis.Shared;
  using Ainomis.Shared.Utility;
  using Ainomis.Shared.Viewport;

  using Artemis;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Media;

  using Common = Ainomis.Shared.Common;

  internal class MapRenderer : Common.IDrawable {
    private IViewportProvider _viewportProvider;
    private List<Texture2D> _textures;
    private List<RenderTarget2D> _layerSurfaces;
    private SpriteBatch _layerSpriteBatch;
    private SpriteBatch _screenSpriteBatch;
    private Size _mapPixelSize;
    private Map _map;

    /// <summary>Constructs a new area renderer.</summary>
    public MapRenderer(
        SpriteBatch spriteBatch,
        IViewportProvider vwProvider,
        ContentManager mapContent,
        Map map) {
      _viewportProvider = vwProvider.ThrowIfNull(nameof(vwProvider));
      _screenSpriteBatch = spriteBatch.ThrowIfNull(nameof(spriteBatch));
      _layerSpriteBatch = new SpriteBatch(_screenSpriteBatch.GraphicsDevice);

      _map = map.ThrowIfNull(nameof(map));
      _mapPixelSize = new Size(map.Width * map.TileWidth, map.Height * map.TileHeight);

      _textures = map.Tilesets.Where(t => !t.Properties.IsMeta).Select(tileset => {
        var tilesetImage = Path.ChangeExtension(tileset.Image, null);
        return mapContent.Load<Texture2D>(tilesetImage);
      }).ToList();

      if (_textures.Count == 0) {
        throw new ArgumentException("Invalid map; no associated tilesets", nameof(map));
      }
    }

    /// <summary>Renders all the areas' layers.</summary>
    public void Draw(GameTime gameTime) {
      if (_layerSurfaces == null) {
        _layerSurfaces = CreateLayerSurfaces().ToList();
      }

      // TODO: Add margin to prevent tearing in corners
      var visiblePixels = Rectangle.Intersect(_viewportProvider.Viewport.Bounds, _mapPixelSize);
      if (!visiblePixels.IsEmpty) {
        var offset = new Vector2(visiblePixels.X, visiblePixels.Y);

        for (int i = 0; i < _layerSurfaces.Count; i++) {
           float layerDepth = 1f - (i / (float)_layerSurfaces.Count);
          _screenSpriteBatch.Draw(_layerSurfaces[i], offset, visiblePixels, layerDepth);
        }
      }
    }

    /// <summary>Creates a pre-rendered texture for each map layer.</summary>
    private IEnumerable<RenderTarget2D> CreateLayerSurfaces() {
      // Save each layer as a cached graphics surface (i.e image)
      return _map.Layers.Where(l => !l.Properties.IsMeta).Select((layer) => {
        // Create a surface for the entire layer to be rendered
        var renderTarget = new RenderTarget2D(
          _layerSpriteBatch.GraphicsDevice, (int)_mapPixelSize.Width, (int)_mapPixelSize.Height);
        var layerColor = new Color(Color.White, layer.Opacity);

        using (var target = renderTarget.BeginDraw(Color.Transparent)) {
          // Use point sampling to prevent seams from appearing between the tiles
          _layerSpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
          RenderMapLayer(_layerSpriteBatch, layer, layerColor);
          _layerSpriteBatch.End();
        }

        return renderTarget;
      });
    }

    /// <summary>Renders a map layer using an associated sprite batch.</summary>
    private void RenderMapLayer(SpriteBatch spriteBatch, Map.Layer layer, Color color) {
      for (int y = 0; y < _map.Height; y++) {
        int verticalIndex = y * (int)_map.Width;

        for (int x = 0; x < _map.Width; x++) {
          // Transparent tiles have an index of zero
          uint tileIndex = layer.Data[verticalIndex + x];

          if (tileIndex != 0) {
            // TODO: Add logic for multiple tilesets
            var tileSource = _map.Tilesets.First().GetSourceRectangleForTile(tileIndex);
            var tilePosition = new Vector2(x * _map.TileWidth, y * _map.TileHeight);

            spriteBatch.Draw(_textures.First(), tilePosition, tileSource, color);
          }
        }
      }
    }
  }
}
