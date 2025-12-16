using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameRecorder : MonoBehaviour {
    public static GameRecorder Instance { get; private set; }

    private RecordingData recordingData;
    private bool isRecording = false;
    private float recordingStartTime = 0f;
    private string recordingsPath;

    // Track selection state
    private Vector2 selectionStart;
    private bool selectionInProgress = false;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
        recordingsPath = Path.Combine(Application.persistentDataPath, "Recordings");
        if (!Directory.Exists(recordingsPath)) {
            Directory.CreateDirectory(recordingsPath);
        }
    }

    public void StartRecording() {
        if (isRecording) return;
        
        recordingData = new RecordingData();
        recordingStartTime = Time.time;
        isRecording = true;
        Debug.Log("Recording started");
    }

    public void StopRecording() {
        if (!isRecording) return;
        
        isRecording = false;
        recordingData.totalDuration = Time.time - recordingStartTime;
        SaveRecording();
        Debug.Log("Recording stopped. Duration: " + recordingData.totalDuration + "s");
    }

    public bool IsRecording() {
        return isRecording;
    }

    void Update() {
        if (!isRecording) return;

        float currentTime = Time.time - recordingStartTime;

        // Record mouse button down events
        if (Input.GetMouseButtonDown(0)) {
            RecordMouseClick(0, true, currentTime);
            selectionStart = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            selectionInProgress = true;
        }

        if (Input.GetMouseButtonDown(1)) {
            RecordMouseClick(1, true, currentTime);
        }

        // Record mouse button up events
        if (Input.GetMouseButtonUp(0)) {
            RecordMouseClick(0, false, currentTime);
            if (selectionInProgress) {
                Vector2 selectionEnd = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                RecordSelection(selectionStart, selectionEnd, currentTime);
                selectionInProgress = false;
            }
        }

        if (Input.GetMouseButtonUp(1)) {
            RecordMouseClick(1, false, currentTime);
        }
    }

    private void RecordMouseClick(int button, bool isDown, float time) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 worldPos = Vector3.zero;
        
        if (Physics.Raycast(ray, out hit)) {
            worldPos = hit.point;
        }

        MouseClickEvent clickEvent = new MouseClickEvent(button, worldPos, isDown);
        string jsonData = JsonUtility.ToJson(clickEvent);
        GameEvent gameEvent = new GameEvent(time, "MouseClick", jsonData);
        recordingData.events.Add(gameEvent);
    }

    private void RecordSelection(Vector2 start, Vector2 end, float time) {
        SelectionEvent selectionEvent = new SelectionEvent(start, end);
        string jsonData = JsonUtility.ToJson(selectionEvent);
        GameEvent gameEvent = new GameEvent(time, "Selection", jsonData);
        recordingData.events.Add(gameEvent);
    }

    public void RecordUnitSpawn(string unitTag, Vector3 position, float rotation) {
        if (!isRecording) return;
        
        float currentTime = Time.time - recordingStartTime;
        UnitSpawnEvent spawnEvent = new UnitSpawnEvent(unitTag, position, rotation);
        string jsonData = JsonUtility.ToJson(spawnEvent);
        GameEvent gameEvent = new GameEvent(currentTime, "UnitSpawn", jsonData);
        recordingData.events.Add(gameEvent);
    }

    private void SaveRecording() {
        string json = JsonUtility.ToJson(recordingData, true);
        string filename = "recording_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
        string filepath = Path.Combine(recordingsPath, filename);
        File.WriteAllText(filepath, json);
        Debug.Log("Recording saved to: " + filepath);
    }

    public string GetRecordingsPath() {
        return recordingsPath;
    }
}
