namespace Ainomis {
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  internal static class SpriteBatchExtensions {
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
  }
}
