using UnityEngine;
using System.Collections;

public class MoveByNpc : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        // find the attack range
        float range = GetComponent<Attack>().range;

        // find all enemy units
        string enemyTag = GetComponent<Attack>().enemyTag;
        GameObject[] units = GameObject.FindGameObjectsWithTag(enemyTag);
        
        // is there any unit that is in attack range already? if so, then there
        // is nothing to do right now
        foreach (GameObject g in units) {
            // still alive?
            if (g != null) {
                if (Vector3.Distance(transform.position, g.transform.position) <= range) {
                    return;
                }
            }
        }

        // already moving somewhere? then do nothing for now
        if (GetComponent<UnityEngine.AI.NavMeshAgent>().hasPath) {
            return;
        }


        // pick a random target (if there are any)
        if (units.Length > 0) {
            int index = Random.Range(0, units.Length);
            GameObject u = units[index];
            
            // still alive?
            if (u != null) {
                // move close enough so we are in attack range
                Vector3 pos = transform.position;
                Vector3 target = u.transform.position;
                
                Vector3 dir = target - pos;
                dir = dir.normalized;
                Vector3 dest = pos + dir * (Vector3.Distance(target, pos) - range);

                // tell the navmesh agent to go there
				// FIXED
//                GetComponent<NavMeshAgent>().destination = dest;
				GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(dest);

            }
        }
	}
}
