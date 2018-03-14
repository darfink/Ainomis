# Ainomis

This is an aspiring implementation of a Pokémon game.  
It is developed using MonoGame/FNA in C#, allowing it to be cross-platform.

## Platforms

Adding additional platforms is a breeze, but these are supported at the
moment:

- macOS
- iOS

## Implementation

This code base has been developed with a lot of care, aspiring it to be an
awesome learning source, whilst also applying interesting patterns, such as
[ECS](https://en.wikipedia.org/wiki/Entity%E2%80%93component%E2%80%93system).

Highlighting some awesome abstractions:

- `InputActionBinder` abstracts all controller-specific input.
- `ControlComponent` abstracts all input sources (e.g. player or NPC).
- `GameStateManager` allows simple state transitions.

## MonoGame vs FNA

MonoGame is used due to it's awesome support for mobile platforms, whilst FNA
is used on desktops, due to better performance and more extensive controller
support.

## Screenshot

![In-game](https://i.imgur.com/UWrlc3w.png)
