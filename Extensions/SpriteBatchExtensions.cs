namespace Ainomis {
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  internal static class SpriteBatchExtensions {
    /// <summary>
    /// Begins a sprite batch with an associated matrix.
    /// </summary>
    /// <param name="spriteBatch">Sprite batch used for drawing.</param>
    /// <param name="matrix">Matrix used for manipulation.</param>
    public static void Begin(
        this SpriteBatch spriteBatch,
        Matrix matrix) {
      spriteBatch.Begin(
        SpriteSortMode.FrontToBack,
        BlendState.AlphaBlend,
        SamplerState.LinearWrap,
        DepthStencilState.Default,
        RasterizerState.CullNone,
        null,
        matrix);
    }
  }
}
