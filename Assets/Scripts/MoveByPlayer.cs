using UnityEngine;
using System.Collections;

public class MoveByPlayer : MonoBehaviour {
	// Update is called once per frame
	void Update () {
	    // Rightclicked while selected?
        if (Input.GetMouseButtonDown(1) && isSelected()) {
            // Find out where the user clicked in the 3D world
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GetComponent<UnityEngine.AI.NavMeshAgent>().destination = hit.point;
            }
        }
	}

    // Find out if its selected (if the selection circle is visible)
    bool isSelected() {
        MeshRenderer[] children = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in children) {
            if (r.gameObject.name == "SelectionCircle" && r.enabled) {                
                return true;
            }
        }
        return false;
    }
}