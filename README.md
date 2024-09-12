# Fylgja

A [Unity](https://unity.com/) game produced by [Mölndals Museum](https://www.molndal.se/molndals-stadsmuseum/samlingar/fylgja/om-spelet-fylgja.html) and funded by [Söderbergs Stiftelse](https://torstensoderbergsstiftelse.se/). Developed by Liminal Studio [Charlotte Heyman](https://www.heyman.nu/) and [Outbreak Studios](https://outbreakstudios.com).

## About

This repository is the touch-version that has code changes made for tablet and phones and reduced assets. It is based on the repository [Fylgja_Pc](https://github.com/charlottanpottan/Fylgja_Pc), which contains the desktop version with higher resolution textures and meshes.

## Features

* Optimized for mobile devices (tablets and phones).
* Reduced textures and meshes for better performance.

## Getting Started

### Prerequisites

* You need to download [Unity Hub](https://unity.com/download) and install `2018.4.36f1` with the platforms that you want to build to (Android, iOS, etc).

## Build

### Android

* Select File / Build Settings in the Unity menu.
* Select Android as the active platform and press the `Switch Platform` button. _It might take a few minutes for it to compile assets_.

#### Build Settings

<img src="docs/images/build_settings_android.png" alt="description" height="400px">

* Make sure all Scenes are added and enabled.
* Enable **Build App Bundle (Google Play)**. (disable if you want an `APK` formatted build file).
* Make sure that Development Build and debugging options are disabled.

* Press **Player Settings...**, which opens up a dialog with a lot of properties.

#### Player Settings

<img src="docs/images/player_settings_android.png" alt="description" height="600px">

* **Version**: should be three numbers separated with a `.` in the [semver](https://semver.org/)-format `MAJOR.MINOR.PATCH`. Currently it is `1.1.1`.

* **Bundle Version Code**: is the build number that the Google Play Console is interested in. It should be increased with one for every release. (Currently 66 for version 1.1.1).

* **Minimum API Level** and **Target API Level**: Set to level 34, as recommended by Google Play Console.

* **Scripting Backend**: IL2CPP.
* **C++ Compiler Configuration**: Release (it is possible to use final, but it takes a _long_ time to build).

* **Target Architectures**: Enable both Armv7 and Arm64.

#### Publishing Settings

* Enter the **Keystore Password** (not included here, ask the team for it). Then select the proper key from the list of alias. This is known as the upload key for the [Google Play Console](https://play.google.com/console).

## Development Instructions

* `SplashScreen` is the first scene, but during development it is reommended to use `EntryLevel` to access the main menu directly, or start with the `BronzeAge` or `IronAge` scenes.

## Known Issues & To-do

* **Unity Warnings**. Fix the Unity warnings. Extra .mb-files, some prefab properties, specifics are listed in [DEVLOG.md](docs/DEVLOG.md).

* **UI Bugs**. Update the code for the scenes `SplashScreen` and `Credits` to use the normal Unity UI, so it works in Unity 2019 and later.

* **Repository Consolidation**. "Merge" the `Fylgja_Pc` and `Fylgja_Touch` repos into one. Maintain different quality settings and platform overrides (e.g., Texture Import Settings) for each platform, allowing all builds from the main branch in a single repository called `Fylgja`.
