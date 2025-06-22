# 🏈 Visualizer-Prototype

A Unity-based immersive football visualizer built with the XR Interaction Toolkit. Designed to simulate and replay real football plays from multiple perspectives — including quarterback, wide receiver, and running back — using XR headsets.

## 🚀 Features

- 🎮 **Multi-POV Support**  
  Switch between the Quarterback, Wide Receiver, Running Back, and a Referee-style Free Cam (REF) with full locomotion.

- ⏮️ **Replay Engine**  
  Loads and animates pre-recorded football plays (passing/running) from JSON data. Includes position and rotation interpolation for smooth movement.

- 🧠 **XR Interaction Toolkit**  
  Fully compatible with XR Toolkit for seamless hand/controller tracking and locomotion.

- 🧭 **In-Game UI Menu**  
  Trigger menu via A/X button. Menu appears dynamically in front of the player with contextual info like play name and duration.

- 📦 **3D Assets & Audio**  
  Football, players, crowd reactions, and voice-over audio included.

- 🎯 **Teleportation Anchors**  
  Teleport between key field points using ray-based teleportation.

## 🛠️ Tech Stack

- Unity 2022+
- XR Interaction Toolkit
- C#
- Android XR Devices (Quest, Vive Focus 3, etc.)
- JSON Data Playback
- Git LFS for large 3D assets

## 🧩 Input Actions

Configured via `InputActionAsset`. Custom `MoveMenu` map handles UI activation with A/X buttons.

## 🧪 Testing & Deployment

- Test in Unity Editor with mock XR device or directly on Android XR headset.
- Menu offset and playback speed customizable.
- Player reset logic included for clean replays.

## 📝 License

Private/internal use only – property of StatusPro.

