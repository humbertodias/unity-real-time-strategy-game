using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
    public int current = 4;

    public Color color = Color.red;

    // Update is called once per frame
    void Update () {
        // set 3d text to '----', '---', '--', '-' or ''
        TextMesh tm = GetComponentInChildren<TextMesh>();
        tm.text = new string('-', current);
        
        // set 3d text color
//		tm.renderer.material.color = color;
		tm.GetComponent<Renderer>().material.color = color;
        
        // optional: make it look horizontally by facing the camera
        // (uncomment it to see how it looks otherwise)
        tm.transform.forward = Camera.main.transform.forward;
    }

    // LateUpdate is called after all Update functions have been called
    void LateUpdate() {
        // dead?     
        if (current <= 0) {
            Destroy(gameObject);
        }
    }
}
