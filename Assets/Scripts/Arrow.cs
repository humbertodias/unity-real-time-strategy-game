using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
    // The target will be set from whoever uses the arrow.
    // The arrow will fly towards the target and deal damage.
    public Transform target;

    // The fly speed
    public float speed = 5.0f;

	// Update is called once per frame
	void Update () {
       // target still alive?
       if (target != null) {
            // look at it
            transform.LookAt(target);

            // move towards it a bit more
            transform.position = Vector3.MoveTowards(transform.position,
                                                     target.position,
                                                     speed * Time.deltaTime);
            // reached it?
            if (transform.position == target.position) {
                // deal damage (this decrease the target's health by one)
                --target.GetComponent<Health>().current;

                // destroy arrow
                Destroy(gameObject);
            }
        } else {
            // if the target is dead, we are done
            Destroy(gameObject);
        }
    }
}
