using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GameReplay : MonoBehaviour {
    public static GameReplay Instance { get; private set; }

    private RecordingData recordingData;
    private bool isReplaying = false;
    private float replayStartTime = 0f;
    private int currentEventIndex = 0;
    private float playbackSpeed = 1.0f;
    private bool isPaused = false;
    private float pauseTime = 0f;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public bool LoadRecording(string filepath) {
        if (!File.Exists(filepath)) {
            Debug.LogError("Recording file not found: " + filepath);
            return false;
        }

        try {
            string json = File.ReadAllText(filepath);
            recordingData = JsonUtility.FromJson<RecordingData>(json);
            Debug.Log("Recording loaded: " + recordingData.events.Count + " events, Duration: " + recordingData.totalDuration + "s");
            return true;
        } catch (System.Exception e) {
            Debug.LogError("Error loading recording: " + e.Message);
            return false;
        }
    }

    public void StartReplay() {
        if (recordingData == null || recordingData.events.Count == 0) {
            Debug.LogWarning("No recording data to replay");
            return;
        }

        isReplaying = true;
        replayStartTime = Time.time;
        currentEventIndex = 0;
        isPaused = false;
        Debug.Log("Replay started");
    }

    public void StopReplay() {
        isReplaying = false;
        currentEventIndex = 0;
        isPaused = false;
        Debug.Log("Replay stopped");
    }

    public void PauseReplay() {
        if (!isReplaying) return;
        
        isPaused = !isPaused;
        if (isPaused) {
            pauseTime = Time.time;
            Debug.Log("Replay paused");
        } else {
            replayStartTime += (Time.time - pauseTime);
            Debug.Log("Replay resumed");
        }
    }

    public void SetPlaybackSpeed(float speed) {
        playbackSpeed = Mathf.Clamp(speed, 0.1f, 5.0f);
    }

    public bool IsReplaying() {
        return isReplaying;
    }

    public bool IsPaused() {
        return isPaused;
    }

    public float GetPlaybackSpeed() {
        return playbackSpeed;
    }

    public float GetReplayProgress() {
        if (recordingData == null || recordingData.totalDuration <= 0) return 0f;
        float currentTime = (Time.time - replayStartTime) * playbackSpeed;
        return Mathf.Clamp01(currentTime / recordingData.totalDuration);
    }

    void Update() {
        if (!isReplaying || isPaused || recordingData == null) return;

        float currentTime = (Time.time - replayStartTime) * playbackSpeed;

        // Process all events that should have occurred by now
        while (currentEventIndex < recordingData.events.Count) {
            GameEvent gameEvent = recordingData.events[currentEventIndex];
            
            if (gameEvent.timestamp <= currentTime) {
                ProcessEvent(gameEvent);
                currentEventIndex++;
            } else {
                break;
            }
        }

        // Check if replay is finished
        if (currentEventIndex >= recordingData.events.Count) {
            Debug.Log("Replay finished");
            StopReplay();
        }
    }

    private void ProcessEvent(GameEvent gameEvent) {
        switch (gameEvent.eventType) {
            case "MouseClick":
                ProcessMouseClick(gameEvent.data);
                break;
            case "Selection":
                ProcessSelection(gameEvent.data);
                break;
            case "UnitSpawn":
                ProcessUnitSpawn(gameEvent.data);
                break;
            default:
                Debug.LogWarning("Unknown event type: " + gameEvent.eventType);
                break;
        }
    }

    private void ProcessMouseClick(string jsonData) {
        MouseClickEvent clickEvent = JsonUtility.FromJson<MouseClickEvent>(jsonData);
        Vector3 worldPos = new Vector3(clickEvent.x, clickEvent.y, clickEvent.z);
        
        // Simulate the click by directly calling the appropriate game logic
        if (clickEvent.button == 0 && clickEvent.isDown) {
            // Left click down - handled by Selection component
            SimulateLeftClickDown(worldPos);
        } else if (clickEvent.button == 0 && !clickEvent.isDown) {
            // Left click up - handled by Selection component
            SimulateLeftClickUp(worldPos);
        } else if (clickEvent.button == 1 && clickEvent.isDown) {
            // Right click - movement command
            SimulateRightClick(worldPos);
        }
    }

    private void SimulateLeftClickDown(Vector3 worldPos) {
        // Check if clicking on a castle to spawn units
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(worldPos));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            CastlePlayer castle = hit.transform.GetComponent<CastlePlayer>();
            if (castle != null) {
                castle.SendMessage("OnMouseDown", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void SimulateLeftClickUp(Vector3 worldPos) {
        // Selection is handled by the Selection component
    }

    private void SimulateRightClick(Vector3 worldPos) {
        // Find selected units and move them
        GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject unit in units) {
            if (unit != null) {
                MeshRenderer[] children = unit.GetComponentsInChildren<MeshRenderer>();
                bool isSelected = false;
                foreach (MeshRenderer r in children) {
                    if (r.gameObject.name == "SelectionCircle" && r.enabled) {
                        isSelected = true;
                        break;
                    }
                }
                
                if (isSelected) {
                    UnityEngine.AI.NavMeshAgent agent = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
                    if (agent != null) {
                        agent.destination = worldPos;
                    }
                }
            }
        }
    }

    private void ProcessSelection(string jsonData) {
        SelectionEvent selectionEvent = JsonUtility.FromJson<SelectionEvent>(jsonData);
        // The actual selection is handled by the Selection component
        // This event is mainly for tracking purposes
    }

    private void ProcessUnitSpawn(string jsonData) {
        UnitSpawnEvent spawnEvent = JsonUtility.FromJson<UnitSpawnEvent>(jsonData);
        Vector3 position = new Vector3(spawnEvent.x, spawnEvent.y, spawnEvent.z);
        // Unit spawning is triggered by castle clicks, so we don't need to recreate units here
        // The recording captures the click events that trigger spawns
    }

    public string[] GetAvailableRecordings() {
        string recordingsPath = Path.Combine(Application.persistentDataPath, "Recordings");
        if (!Directory.Exists(recordingsPath)) {
            return new string[0];
        }

        string[] files = Directory.GetFiles(recordingsPath, "*.json");
        return files.Select(f => Path.GetFileName(f)).ToArray();
    }
}
