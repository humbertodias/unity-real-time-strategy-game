using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour {    
    // This is the GUI size
    public int width = 200;
    public int height = 35;

    // This is the casle prefab, to be set in the inspector
    public GameObject prefab;
    
    // This holds the game-world instance of the prefab
    GameObject instance;

    void Update() {
        // Is the player currently selectin a place to build the castle? Or in
        // other words, was the instance variable set?
        if (instance != null) {
            // Find out the 3D world coordinates under the cursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.name == "Ground") {
                    // Refresh the instance position
                    instance.transform.position = hit.point;
                    
                    // Note: if your castle appears to be 'in' the Ground
                    //       instead of 'on' the ground, you may have to adjust
                    //       the y coordinate like so:
                    //instance.transform.position += new Vector3(0, 1.23f, 0);
                }
            }
            
            // Player clicked? Then stop positioning the castle by simply
            // loosing track of our instance. Its still in the game world after-
            // wards, but we just can't position it anymore.
            if (Input.GetMouseButton(0)) {
                instance = null;
            }
        }
    }
    
    void OnGUI() {
        GUILayout.BeginArea(new Rect(Screen.width/2 - width/2,
                                     Screen.height - height,
                                     width,
                                     height), "", "box");
        
        // Disable the building button if we are currently building something.
        // Note: this enables GUIs if we have no instance at the moment, and
        //       it disables GUIs if we currently have one. Its just written in
        //       a fancy way. (it can also be done with a if-else construct)
        GUI.enabled = (instance == null);
        if (GUILayout.Button("BUILD CASTLE")) {
            // Instantiate the prefab and keep track of it by assigning it to
            // our instance variable.
            instance = (GameObject)GameObject.Instantiate(prefab);
        }
        GUILayout.EndArea();
    }
}