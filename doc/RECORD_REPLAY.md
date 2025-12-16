# Record/Replay System

## Overview

This Unity RTS game now includes a complete record/replay system that allows players to record their gameplay sessions and replay them later.

## Features

- **Recording**: Capture all player inputs including mouse clicks, unit selections, and movements
- **Playback**: Replay recorded sessions with full accuracy
- **Replay Controls**: Pause, resume, and control playback speed (0.1x to 5.0x)
- **Progress Tracking**: Visual progress bar showing replay progress
- **Multiple Recordings**: Save and load multiple recording sessions

## How to Use

### Setting Up

1. Add the `RecordReplayUI` script to any GameObject in your scene (e.g., Main Camera or an empty GameObject)
2. The system will automatically create `GameRecorder` and `GameReplay` instances when the game starts

### Recording a Game Session

1. Click the **"Start Recording"** button in the top-right corner of the screen
2. Play the game normally - all your actions will be recorded
3. Click the **"Stop Recording"** button when finished
4. Your recording will be saved automatically to the Recordings folder

### Replaying a Recording

1. Click the **"Load Replay"** button in the top-right corner
2. Select a recording from the list
3. The replay will start automatically
4. Use the replay controls:
   - **Pause/Resume**: Pause or resume the playback
   - **Speed Slider**: Adjust playback speed from 0.1x to 5.0x
   - **Progress Bar**: Shows the current position in the replay

### File Location

Recordings are saved as JSON files in:
- **Windows**: `%USERPROFILE%\AppData\LocalLow\DefaultCompany\ProjectName\Recordings\`
- **macOS**: `~/Library/Application Support/DefaultCompany/ProjectName/Recordings/`
- **Linux**: `~/.config/unity3d/DefaultCompany/ProjectName/Recordings/`

## Technical Details

### Components

- **GameEvent.cs**: Data structures for different event types (mouse clicks, selections, unit spawns)
- **RecordingData.cs**: Container for all recorded events
- **GameRecorder.cs**: Records player inputs and game events
- **GameReplay.cs**: Plays back recorded sessions
- **RecordReplayUI.cs**: User interface for recording and replay controls

### Recorded Events

The system records:
- Mouse button down/up events (left and right clicks)
- Unit selection rectangles
- Unit movement commands
- Unit spawn events from castles

### Limitations

- Recordings capture deterministic player inputs, so replays will be accurate as long as the game state is deterministic
- Random events (like unit spawn positions) are recorded at spawn time to ensure accurate replay
- AI opponent behavior is not recorded and will play independently during replay

## Integration Notes

The system integrates seamlessly with existing game scripts:
- Modified `UnitSpawner.cs` to record spawn events
- Works with existing `Selection.cs`, `MoveByPlayer.cs`, and `CastlePlayer.cs` scripts
- No changes required to existing gameplay logic

## Future Enhancements

Possible improvements:
- Add replay seeking (jump to specific time)
- Support for saving/loading replays with custom names
- Replay thumbnail previews
- Multiplayer replay support
- Replay data compression
