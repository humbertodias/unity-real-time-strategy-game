using System;
using System.Collections.Generic;

[Serializable]
public class RecordingData {
    public List<GameEvent> events;
    public float totalDuration;
    public string recordingDate;

    public RecordingData() {
        events = new List<GameEvent>();
        totalDuration = 0f;
        recordingDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
