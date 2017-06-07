namespace Ainomis.Extensions {
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  internal static class XnaExtensions {
    /// <summary>Creates a replicate from another instance.</summary>
    public static ContentManager Allocate(this ContentManager content) =>
      new ContentManager(content.ServiceProvider, content.RootDirectory);

    /// <summary>Converts a Point to a Vector2.</summary>
    public static Vector2 ToVector2(this Point point) => new Vector2(point.X, point.Y);

    /// <summary>Begins a sprite batch with an associated matrix.</summary>
    public static void Begin(this SpriteBatch spriteBatch, Matrix matrix) {
      spriteBatch.Begin(
        SpriteSortMode.BackToFront,
        BlendState.AlphaBlend,
        SamplerState.LinearClamp,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        matrix);
    }

    /// <summary>Begins a sprite batch with an associated blend and sampler state.</summary>
    public static void Begin(this SpriteBatch spriteBatch, BlendState blendState, SamplerState samplerState) {
      spriteBatch.Begin(SpriteSortMode.BackToFront, blendState, samplerState, DepthStencilState.Default, RasterizerState.CullNone);
    }

    /// <summary>Draws a texture with a position, source rectangle and layer depth.</summary>
    public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle source, float layerDepth) {
      spriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layerDepth);
    }

    /// <summary>Draws a texture with a position, color and scale.</summary>
    public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, Vector2 scale) {
      spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
    }

    /// <summary>Locks a render target, restoring environment upon dispose.</summary>
    public static IDisposable BeginDraw(
        this RenderTarget2D renderTarget,
        Color backgroundColor) =>
      new RenderTargetOperation(renderTarget, renderTarget.GraphicsDevice, backgroundColor);

    private class RenderTargetOperation : IDisposable {
      private readonly RenderTargetUsage _previousRenderTargetUsage;
      private readonly GraphicsDevice _graphicsDevice;
      private readonly Viewport _viewport;

      public RenderTargetOperation(RenderTarget2D renderTarget, GraphicsDevice graphicsDevice, Color backgroundColor) {
        _graphicsDevice = graphicsDevice;
        _viewport = _graphicsDevice.Viewport;
        _previousRenderTargetUsage = _graphicsDevice.PresentationParameters.RenderTargetUsage;

        _graphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        _graphicsDevice.SetRenderTarget(renderTarget);
        _graphicsDevice.Clear(backgroundColor);
      }

      public void Dispose() {
        _graphicsDevice.SetRenderTarget(null);
        _graphicsDevice.PresentationParameters.RenderTargetUsage = _previousRenderTargetUsage;
        _graphicsDevice.Viewport = _viewport;
      }
    }
  }
}
