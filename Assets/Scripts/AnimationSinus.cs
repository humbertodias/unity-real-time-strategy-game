using UnityEngine;
using System.Collections;

public class AnimationSinus : MonoBehaviour {
    // parameters
    float animRemaining = 0.0f;
    public float animSpeed = 20.0f;
    public float intensity = 1.0f;

    // start scale
    Vector3 startScale;

	// Use this for initialization
	void Start () {
	   startScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
        // animation still in progress?
        if (animRemaining > 0.0f) {
            animRemaining -= animSpeed * Time.deltaTime;
            
            // calculate current scale:
            //   Mathf.Sin so it goes higher and lower
            //   * intensity so the effect is not too strong
            float s = Mathf.Sin(animRemaining) * intensity;
            transform.localScale = startScale + new Vector3(s, s, s);
        }
	}

    public void toggle() {
        // start a new animation
        animRemaining = 2 * Mathf.PI;
    }

    public bool inProgress() {
        return animRemaining > 0.0f;
    }
}
