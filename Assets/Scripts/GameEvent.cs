using UnityEngine;
using System;

[Serializable]
public class GameEvent {
    public float timestamp;
    public string eventType;
    public string data;

    public GameEvent(float time, string type, string eventData) {
        timestamp = time;
        eventType = type;
        data = eventData;
    }
}

[Serializable]
public class MouseClickEvent {
    public int button; // 0 = left, 1 = right
    public float x;
    public float y;
    public float z;
    public bool isDown; // true for down, false for up

    public MouseClickEvent(int btn, Vector3 worldPos, bool down) {
        button = btn;
        x = worldPos.x;
        y = worldPos.y;
        z = worldPos.z;
        isDown = down;
    }
}

[Serializable]
public class SelectionEvent {
    public float startX;
    public float startY;
    public float endX;
    public float endY;

    public SelectionEvent(Vector2 start, Vector2 end) {
        startX = start.x;
        startY = start.y;
        endX = end.x;
        endY = end.y;
    }
}

[Serializable]
public class UnitSpawnEvent {
    public string unitTag; // "Player" or "Npc"
    public float x;
    public float y;
    public float z;
    public float rotation;

    public UnitSpawnEvent(string tag, Vector3 pos, float rot) {
        unitTag = tag;
        x = pos.x;
        y = pos.y;
        z = pos.z;
        rotation = rot;
    }
}
