using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "planet" || collision.gameObject.tag == "target_planet") {
            CreateMirror.hasCreatedMirror = true;
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "mirror") {
            CreateMirror createMirror = GameObject.FindGameObjectWithTag("Player").GetComponent<CreateMirror>();
            Destroy(createMirror.mirror_transform[createMirror.mirror_transform.IndexOf(gameObject.transform) - 1].gameObject);
            CreateMirror.hasCreatedMirror = true;
            Destroy(gameObject);
        }
        
    }

}
