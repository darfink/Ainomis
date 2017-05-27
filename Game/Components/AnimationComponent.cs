namespace Ainomis.Game.Components {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Ainomis.Extensions;
  using Ainomis.Game.Resources;

  using Artemis.Interface;

  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;

  /// <summary>Animation component.</summary>
  internal class AnimationComponent : IComponent {
    // Animations stored by name for performance
    private Dictionary<string, Animation> _animations;
    private int _frameIndex;

    /// <summary>Creates a new animation component.</summary>
    public AnimationComponent(List<Animation> animations) {
      _animations = animations.ThrowIfNull(nameof(animations)).ToDictionary(x => x.Name, x => x);
      Name = animations.First().Name;
    }

    /// <summary>Gets or sets the current animation.</summary>
    public string Name {
      get => Animation.Name;
      set {
        // Only change animation if they differ
        if (Animation?.Name != value) {
          Animation = _animations[value];
          FrameIndex = 0;
        }
      }
    }

    /// <summary>Gets the current animation object.</summary>
    public Animation Animation { get; private set; }

    /// <summary>Gets the current animation frame.</summary>
    public Animation.Frame Frame { get; private set; }

    /// <summary>Gets or sets the current frame time.</summary>
    public TimeSpan FrameTime { get; set; }

    /// <summary>Gets or sets the current frame index.</summary>
    public int FrameIndex {
      get => _frameIndex;
      set {
        Frame = Animation.Frames[value];
        _frameIndex = value;
      }
    }

    /// <summary>Advances to the next frame of the current animation.</summary>
    public void NextFrame() => FrameIndex = (_frameIndex + 1) % Animation.Frames.Count;
  }
}