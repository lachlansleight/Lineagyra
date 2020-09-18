# Lineagyra Changelog

#### Last Update by Lachlan Sleight on 18/09/2020

## Version 0.1.0

### Notes

Initial proof-of-concept version. Turns out Unity's UI Builder just straight up doesn't work, which is a bummer.

### Added

  * New UI, giving much more control over the pattern, generator and camera settings than in LineCircles v2.x
  * Bloom camera effect

### Removed

  * Camera controls (to be re-added soon)

### Fixed

  * Many improvements and bugfixes to the core LineCircle object system, too many to mention here, and nothing drastically noticeable

### Known bugs

  * The UI is totally broken - I was trying to use Unity's new UI system but it just doesn't work. 
    * All the absolutely positioned items are being positioned relatively
    * The main UI panel is being stretched across the screen rather than being a fixed width in the middle
    * All the row flex items are being displayed as flex columns, breaking more of the UI
    * All the hidden objects which are for parameters I haven't made controllable yet are being displayed for some reason