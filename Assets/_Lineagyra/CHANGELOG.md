# Lineagyra Changelog

#### Last Update by Lachlan Sleight on 19/09/2020

---

## Version 0.1.1

### Notes

Identical to version 0.1.0, except the UI isn't broken - I reimplemented it in the old Unity UI system

### Changed

  * Made 2D mode default
  * Changed a few UI control names

### Fixed

  * Redid the UI to use the legacy Unity UI system which actually works.
  * Fixed a small bug which was occasionally causing super crazy patterns to generate
  
### Known bugs
 
  * Line Density slider does nothing
  * Auto Scale Lines checkbox does nothing
  
---

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
    
---