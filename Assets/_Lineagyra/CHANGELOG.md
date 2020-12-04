# Linea Gyra Changelog

#### Last Update by Lachlan Sleight on 04/12/2020

---

## Version 0.4.0

### Notes

Added a great new feature!

### Added

  * The Pattern menu now has two new options - Time to Distance, and Fade with Distance. Try turning Time to Distance to 0.05 or so and moving the camera around!

---

## Version 0.3.1-vr

### Notes

Just fixing up some controller bindings stuff

### Fixed

  * Controller bindings should now work for all headsets

---

## Version 0.3.0-vr

### Notes

This is version 0.3.0 (more or less), only in VR

### Added

  * Added VR support, obviously!
  * Added a welcome screen to explain the controls for those who can't check out the readme
  * Added basic 6DOF locomotion
  * Made the UI movable in 3D space
  * Added some particles to the environment
  * Added a looping audio track

### Changed

  * The Line and Fill Opacity sliders had their range changed to 0% - 25% (from 0% - 100%)

---

## Version 0.3.0

### Notes

Just some minor improvements to 0.2.0, mostly focused on the UI and camera controls

### Added

  * Added a new 'general settings' option in the Advanced menu, which allows changing the current pattern's LineCount, and whether is uses spherical or cartesian coordinates

### Changed

  * The app is now called Linea Gyra!
  * Reduced opacity of main menu, and moved it to the left of the screen
  * Made it so that the camera can still be controlled while the main menu is open
  * For clarity, made it so that when an oscillator is in Constant mode, the Amplitude, Period and Phase sliders are invisible

### Fixed

  * Fixed a bug where the labels for ColorRange and ColorOffset target parameters were switched around in 0.2.0

---

## Version 0.2.0

### Notes

Features new camera controls, advanced UI, and a few bugfixes and improvements

### Added

  * The camera can now be controlled with the mouse. Middle mouse click and drag to pan, mousewheel to scroll and right mouse click and drag to orbit (in 3D mode)
  * Advanced Controls allow the current pattern to be controlled with a great deal of granularity
  * AutoScaleLines UI control now controls the pattern line auto scale property
  * LineDensity now controls how dense the pattern lines are
  * Pattern animation and shuffling can be paused and unpaused using the space bar
  * App version now appears below the UI title

### Removed

  * Removed Pattern zoom slider, as its functionality was replicated by camera zoom

### Fixed

  * Fixed a bug where the pattern was shuffling even when Auto Shuffle was turned off
  * Fixed a few small UI layout bugs and issues

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