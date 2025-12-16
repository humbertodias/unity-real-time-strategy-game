using UnityEngine;
using System.IO;

public class RecordReplayUI : MonoBehaviour {
    private GameRecorder recorder;
    private GameReplay replay;
    
    // UI dimensions
    private int buttonWidth = 150;
    private int buttonHeight = 30;
    private int margin = 10;
    
    // Recording list
    private string[] availableRecordings;
    private int selectedRecordingIndex = 0;
    private Vector2 scrollPosition = Vector2.zero;
    private bool showRecordingsList = false;
    
    void Start() {
        // Get or create recorder instance
        recorder = GameRecorder.Instance;
        if (recorder == null) {
            GameObject recorderObj = new GameObject("GameRecorder");
            recorder = recorderObj.AddComponent<GameRecorder>();
        }
        
        // Get or create replay instance
        replay = GameReplay.Instance;
        if (replay == null) {
            GameObject replayObj = new GameObject("GameReplay");
            replay = replayObj.AddComponent<GameReplay>();
        }
        
        RefreshRecordingsList();
    }
    
    void OnGUI() {
        // Record/Replay controls in top-right corner
        int x = Screen.width - buttonWidth - margin;
        int y = margin;
        
        // Recording controls
        if (recorder != null) {
            if (!recorder.IsRecording()) {
                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "Start Recording")) {
                    if (!replay.IsReplaying()) {
                        recorder.StartRecording();
                    } else {
                        Debug.LogWarning("Cannot record during replay");
                    }
                }
            } else {
                GUI.color = Color.red;
                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "■ Stop Recording")) {
                    recorder.StopRecording();
                    RefreshRecordingsList();
                }
                GUI.color = Color.white;
            }
        }
        
        y += buttonHeight + margin;
        
        // Replay controls
        if (replay != null) {
            if (!replay.IsReplaying()) {
                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "Load Replay")) {
                    showRecordingsList = !showRecordingsList;
                    if (showRecordingsList) {
                        RefreshRecordingsList();
                    }
                }
            } else {
                GUI.color = Color.yellow;
                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), "Stop Replay")) {
                    replay.StopReplay();
                }
                GUI.color = Color.white;
                
                y += buttonHeight + margin;
                
                // Pause button
                string pauseText = replay.IsPaused() ? "Resume" : "Pause";
                if (GUI.Button(new Rect(x, y, buttonWidth, buttonHeight), pauseText)) {
                    replay.PauseReplay();
                }
                
                y += buttonHeight + margin;
                
                // Speed controls
                GUI.Label(new Rect(x, y, buttonWidth, buttonHeight), "Speed: " + replay.GetPlaybackSpeed().ToString("F1") + "x");
                y += buttonHeight;
                
                float newSpeed = GUI.HorizontalSlider(new Rect(x, y, buttonWidth, 20), replay.GetPlaybackSpeed(), 0.1f, 5.0f);
                if (newSpeed != replay.GetPlaybackSpeed()) {
                    replay.SetPlaybackSpeed(newSpeed);
                }
                
                y += 30;
                
                // Progress bar
                GUI.Label(new Rect(x, y, buttonWidth, buttonHeight), "Progress:");
                y += buttonHeight;
                GUI.Box(new Rect(x, y, buttonWidth, 20), "");
                GUI.Box(new Rect(x, y, buttonWidth * replay.GetReplayProgress(), 20), "");
            }
        }
        
        // Recordings list
        if (showRecordingsList && availableRecordings != null && availableRecordings.Length > 0) {
            int listWidth = 300;
            int listHeight = 400;
            int listX = Screen.width / 2 - listWidth / 2;
            int listY = Screen.height / 2 - listHeight / 2;
            
            GUI.Box(new Rect(listX, listY, listWidth, listHeight), "Available Recordings");
            
            GUILayout.BeginArea(new Rect(listX + 10, listY + 30, listWidth - 20, listHeight - 80));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            
            for (int i = 0; i < availableRecordings.Length; i++) {
                if (GUILayout.Button(availableRecordings[i])) {
                    selectedRecordingIndex = i;
                    LoadSelectedRecording();
                    showRecordingsList = false;
                }
            }
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            
            if (GUI.Button(new Rect(listX + 10, listY + listHeight - 40, listWidth - 20, 30), "Close")) {
                showRecordingsList = false;
            }
        } else if (showRecordingsList) {
            int msgWidth = 250;
            int msgHeight = 100;
            int msgX = Screen.width / 2 - msgWidth / 2;
            int msgY = Screen.height / 2 - msgHeight / 2;
            
            GUI.Box(new Rect(msgX, msgY, msgWidth, msgHeight), "No Recordings Available");
            if (GUI.Button(new Rect(msgX + 50, msgY + 50, msgWidth - 100, 30), "Close")) {
                showRecordingsList = false;
            }
        }
    }
    
    void RefreshRecordingsList() {
        availableRecordings = replay.GetAvailableRecordings();
    }
    
    void LoadSelectedRecording() {
        if (availableRecordings != null && selectedRecordingIndex >= 0 && selectedRecordingIndex < availableRecordings.Length) {
            string recordingsPath = Path.Combine(Application.persistentDataPath, "Recordings");
            string filepath = Path.Combine(recordingsPath, availableRecordings[selectedRecordingIndex]);
            if (replay.LoadRecording(filepath)) {
                replay.StartReplay();
            }
        }
    }
}
