using UnityEngine;
using System.Collections;

public class Selection : MonoBehaviour {    

	// Selection rectangle
    Vector2 start = new Vector2(0, 0);
    Vector2 cur = new Vector2(0, 0);
    bool visible = false;
    
    Rect currentRect() {
        // Create a rect from the current start & cur points
        Vector2 min = new Vector2(Mathf.Min(start.x, cur.x),
                                  Mathf.Min(start.y, cur.y));
        Vector2 max = new Vector2(Mathf.Max(start.x, cur.x),
                                  Mathf.Max(start.y, cur.y));
        return new Rect(min.x, min.y, max.x-min.x, max.y-min.y);
    }
        
	// Update is called once per frame
	void Update () {
        // Multi Selection started
        if (Input.GetMouseButtonDown(0)) {
            // Set rect position to current mouse position
            start.x = Input.mousePosition.x;
            start.y = Screen.height - Input.mousePosition.y;
            visible = true;
        }
    
        // Multi Selection in progress?
        if (Input.GetMouseButton(0)) {
            // Update rect
            cur.x = Input.mousePosition.x;
            cur.y = Screen.height - Input.mousePosition.y;
        }
        
        // Multi Selection finished?
        if (Input.GetMouseButtonUp(0)) { 
            // Set select status for each of the player's units:
            GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject g in units) {
                // Still alive?
                if (g != null) {
                    // World to screen position
					Camera cam = Camera.main;
//					Vector3 p = camera.WorldToScreenPoint(g.transform.position);
					Vector3 p = cam.WorldToScreenPoint(g.transform.position);
                    Vector2 screenPos = new Vector2(p.x, Screen.height - p.y);
                    
                    Debug.Log("single selection break2" + g.name);

                    // Is the position in the rectangle?
                    if (currentRect().Contains(screenPos)) {
                        setSelectionCircleVisiblity(g, true);
                    } else {
                        setSelectionCircleVisiblity(g, false);
                    }
                }
            }
            
            // Don't draw the rectangle anymore
            visible = false;
        }
	}

    void setSelectionCircleVisiblity(GameObject g, bool visible) {
        MeshRenderer[] children = g.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in children) {
            if (r.gameObject.name == "SelectionCircle") {                
                // Make the selection circle visible or invisible by enabling or
                // disabling its renderer
                r.enabled = visible;
            }
        }
    }
        
    void OnGUI() {
        // Selection Visible and mouse moved a bit? Then draw the rectangle
        if (visible && !cur.Equals(start)) {
            GUI.Box(currentRect(), "");
        }
    }
}