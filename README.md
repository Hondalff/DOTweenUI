# DOTween UI

Extensible UI animation package for Unity based on DOTween.

## Features

- Trigger-based UI animations
- Move
- Scale
- Rotate
- CanvasGroup fade
- Custom editor drawer for animation entries
- UPM install via Git URL

## Requirements

- Unity 2021.3+
- DOTween installed in project

## Installation

### Via Git URL

Open Unity Package Manager and add package from Git URL:

`https://github.com/Hondalff/DOTweenUIAnimationInspector.git#0.1.0`

If package is stored in a subfolder:

`https://github.com/Hondalff/DOTweenUIAnimationInspector.git?path=/Packages/com.dotweenui#0.1.0`

## Basic Usage

Add `DOTweenUI` to a UI object and configure animation entries in the inspector.

Supported triggers:
- OnEnable
- OnDisable
- OnStart
- PointerEnter
- PointerExit
- PointerDown
- PointerUp
- Click
- Manual

Supported animation types:
- Move
- Scale
- Rotate
- CanvasGroup

## Notes

This package expects DOTween to be already installed in the project.