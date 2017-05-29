namespace Ainomis.Shared.Input.GamePad {
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;

  using Ainomis.Extensions;
  using Ainomis.Shared.Display;
  using Ainomis.Shared.Utility;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using Microsoft.Xna.Framework.Content;
  using Microsoft.Xna.Framework.Input;
#if !FNA
  using Microsoft.Xna.Framework.Input.Touch;
#endif

  public class VirtualGamePad : GamePadDriverBase, Common.IDrawable {
    private ContentManager _content;
    private IDisplayInfo _displayInfo;
    private SpriteBatch _spriteBatch;
    private Controller _controller;

    public VirtualGamePad(ContentManager content, SpriteBatch spriteBatch, IDisplayInfo displayInfo) {
      _spriteBatch = spriteBatch.ThrowIfNull(nameof(spriteBatch));
      _displayInfo = displayInfo.ThrowIfNull(nameof(displayInfo));
      _content = content.ThrowIfNull(nameof(content));

      _controller = new Controller() {
        Alpha = new Color(Color.White, 0.5f),
        State = new GamePadState(),
      };

      LoadOverlay("Default");
    }

    public override void Update(GameTime gameTime) {
      Buttons buttonsPressed = GetHotspots()
        .SelectMany(position => _controller.Overlay.Inputs
          .Where(i => i.Area.Contains((int)position.X, (int)position.Y))
          .Select(i => i.Button))
        .Aggregate((Buttons)0, (prev, next) => prev | next);

      _controller.State = new GamePadState(Vector2.Zero, Vector2.Zero, 0f, 0f, buttonsPressed);
      base.Update(gameTime);
    }

    public void Draw(GameTime gameTime) {
      _spriteBatch.Begin();
      _spriteBatch.Draw(_controller.Texture, Vector2.Zero, _controller.Alpha, _controller.Scale);
      _spriteBatch.End();
    }

    /// <summary>Returns the virtual game pads current state.</summary>
    protected override GamePadState GetCurrentState() => _controller.State;

    /// <summary>Activates a specific game pad overlay.</summary>
    public void LoadOverlay(string name) {
      var directory = Path.Combine("Inputs", name);
      var overlay = Assets.LoadJson<VirtualOverlay>(Path.Combine(directory, "Input.json"));

      var overlaySize = new Vector2(overlay.ImageWidth, overlay.ImageHeight);
      var overlayPath = Path.Combine(directory, Path.ChangeExtension(overlay.Image, null));

      _controller.Texture = _content.Load<Texture2D>(overlayPath);
      _controller.Scale = _displayInfo.Resolution / overlaySize;
      _controller.Overlay = overlay;

      foreach (var input in overlay.Inputs) {
        var area = input.Area;
        input.Area = new Rectangle(
          (int)(area.X * _controller.Scale.X),
          (int)(area.Y * _controller.Scale.Y),
          (int)(area.Width * _controller.Scale.X),
          (int)(area.Height * _controller.Scale.Y));
      }
    }

    /// <summary>Returns a collection of active spots.</summary>
    private IEnumerable<Vector2> GetHotspots() {
#if FNA
      var mouseState = Mouse.GetState();
      if (mouseState.LeftButton == ButtonState.Pressed) {
        yield return new Vector2(mouseState.X, mouseState.Y);
      }
#else
      return TouchPanel.GetState()
        .Where(t => t.State == TouchLocationState.Moved || t.State == TouchLocationState.Pressed)
        .Select(t => t.Position);
#endif
    }

    /// <summary>Controller related properties.</summary>
    private struct Controller {
      public VirtualOverlay Overlay;
      public GamePadState State;
      public Texture2D Texture;
      public Vector2 Scale;
      public Color Alpha;
    }
  }
}
