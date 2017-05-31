namespace Ainomis.Extensions {
  using System;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Graphics;

  internal static class XnaExtensions {
    /// <summary>Creates a replicate from another instance.</summary>
    public static ContentManager Allocate(this ContentManager content) =>
      new ContentManager(content.ServiceProvider, content.RootDirectory);

    /// <summary>Begins a sprite batch with an associated matrix.</summary>
    /// <param name="spriteBatch">Sprite batch used for drawing.</param>
    /// <param name="matrix">Matrix used for manipulation.</param>
    public static void Begin(this SpriteBatch spriteBatch, Matrix matrix) {
      spriteBatch.Begin(
        SpriteSortMode.BackToFront,
        BlendState.AlphaBlend,
        SamplerState.LinearWrap,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        matrix);
    }

    /// <summary>Draws a texture with a position, color and scale.</summary>
    public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Color color, Vector2 scale) {
      spriteBatch.Draw(texture, position, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
    }

    /// <summary>Locks a render target, restoring environment upon dispose.</summary>
    public static IDisposable BeginDraw(
        this RenderTarget2D renderTarget,
        GraphicsDevice graphicsDevice,
        Color backgroundColor) =>
      new RenderTargetOperation(renderTarget, graphicsDevice, backgroundColor);

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
