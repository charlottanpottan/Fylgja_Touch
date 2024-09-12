# 2024-09-11 (@piot)

## Unused? Maya files

A few .mb-files has no corresponding .fbx files. Maybe these are not used in the game anyway?

* `Assets/Fylgja/Props/Manmade/BronzeAgeHut_XX/BronzeAgeHouse_closed_v3.mb`
* `Assets/Fylgja/Props/Manmade/BronzeAgeHut_XX/BronzeAgeHouse_closed_v4.mb`
* `Assets/Fylgja/Props/Manmade/BronzeAgeHut_XX/BronzeAgeHouse_open_01.mb`
* `Assets/Fylgja/Props/Manmade/BirchbarkBox/BirchBarkBoxNew.mb`
* `Assets/Fylgja/Props/Manmade/BirchbarkBox/BirchBarkNoLid_new.mb`
* `Assets/Fylgja/Props/Manmade/BirchbarkBox/BirchBarkNoLid_new 1.mb`
* `Assets/Fylgja/Props/Manmade/IronAgeHouse/IronageHouseNew_final.mb`
* `Assets/Fylgja/Props/Manmade/IronAgeHouse/IronagehouseSmall_final.mb`
* `Assets/Fylgja/Props/Manmade/Smithy/NewSmithyFinal.mb`

## Polygon errors

* `Assets/Fylgja/Props/Manmade/BronzeAgeHut_XX/BronzeAgeHutOpen_02_COLLISION.fbx`

```console
A polygon of BronzeAgeHutOpen_02_COLLISION is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHutOpen_02_COLLISION is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHutOpen_02_COLLISION is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHutOpen_02_COLLISION is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHutOpen_02_COLLISION is self-intersecting and has been discarded.
```

* `Assets/Fylgja/Props/Manmade/BronzeAgeHut_XX/BronzeAgeHutOpen_02.fbx`

```console
A polygon of BronzeAgeHut_02 is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHut_02 is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHut_02 is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHut_02 is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
A polygon of BronzeAgeHut_02 is self-intersecting and has been discarded.
(Filename: /Users/bokken/buildslave/unity/build/Modules/AssetPipelineEditor/Public/ModelImporting/ModelImporter.cpp Line: 1089)
```

* Strange prefab error in `Assets/Fylgja/Minigames/StickFight/Particles/StarParticleEffect.prefab`

```console
Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_ADD') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Object GameObject (named 'Star_BASE') has multiple entries of the same Object component. Removing it!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 791)

Component could not be loaded when loading game object. Cleaning up!

(Filename: /Users/bokken/buildslave/unity/build/Runtime/BaseClasses/GameObject.cpp Line: 811)
```

## Upgrade to 2019, 2020, 2021 and 2022

* `SelectionMode.OnlyUserModifiable` to `SelectionMode.Editable`
* Delete `CreditsTextLogic.cs` since it is no longer needed with a proper Unity UI Canvas.
* Change `GUIText` to `UI.Text` for `CreditsText.cs` (not working yet).
* Delete deprecated Jetbrains dll in project (for Rider).
