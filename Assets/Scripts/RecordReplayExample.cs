using UnityEngine;

/// <summary>
/// Example script showing how to integrate the Record/Replay system
/// Add this component to any GameObject in your scene (e.g., Main Camera)
/// </summary>
public class RecordReplayExample : MonoBehaviour {
    
    void Start() {
        // The RecordReplayUI component automatically creates GameRecorder and GameReplay instances
        // You can also access them directly:
        
        // Example: Start recording programmatically
        // GameRecorder.Instance.StartRecording();
        
        // Example: Stop recording after 60 seconds
        // Invoke("StopRecordingExample", 60f);
        
        // Example: Load and play a specific recording
        // LoadSpecificRecording("recording_20231216_120000.json");
    }
    
    void Update() {
        // Example: Use keyboard shortcuts for record/replay
        if (Input.GetKeyDown(KeyCode.R)) {
            // Toggle recording
            if (GameRecorder.Instance != null) {
                if (GameRecorder.Instance.IsRecording()) {
                    GameRecorder.Instance.StopRecording();
                    Debug.Log("Recording stopped by keyboard shortcut");
                } else {
                    GameRecorder.Instance.StartRecording();
                    Debug.Log("Recording started by keyboard shortcut");
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P)) {
            // Toggle replay pause
            if (GameReplay.Instance != null && GameReplay.Instance.IsReplaying()) {
                GameReplay.Instance.PauseReplay();
                Debug.Log("Replay paused/resumed by keyboard shortcut");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            // Stop replay
            if (GameReplay.Instance != null && GameReplay.Instance.IsReplaying()) {
                GameReplay.Instance.StopReplay();
                Debug.Log("Replay stopped by keyboard shortcut");
            }
        }
    }
    
    void StopRecordingExample() {
        if (GameRecorder.Instance != null && GameRecorder.Instance.IsRecording()) {
            GameRecorder.Instance.StopRecording();
            Debug.Log("Auto-stopped recording after time limit");
        }
    }
    
    void LoadSpecificRecording(string filename) {
        if (GameReplay.Instance != null) {
            string recordingsPath = System.IO.Path.Combine(Application.persistentDataPath, "Recordings");
            string filepath = System.IO.Path.Combine(recordingsPath, filename);
            
            if (GameReplay.Instance.LoadRecording(filepath)) {
                GameReplay.Instance.StartReplay();
                Debug.Log("Started replay from: " + filename);
            }
        }
    }
}
