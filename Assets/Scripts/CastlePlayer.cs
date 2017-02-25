using UnityEngine;
using System.Collections;

public class CastlePlayer : MonoBehaviour {       
    // Note: OnMouseDown only works if object has a collider
    void OnMouseDown() {
        // Only if animation finished
        if (!GetComponent<AnimationSinus>().inProgress()) {            
            // use UnitSpawner
            GetComponent<UnitSpawner>().spawn();
        }
    }
}