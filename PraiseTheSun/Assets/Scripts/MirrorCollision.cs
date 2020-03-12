using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "planet" || collision.gameObject.tag == "target_planet") {
            CreateMirror createMirror = GameObject.FindGameObjectWithTag("Player").GetComponent<CreateMirror>();
            int mirror_index = createMirror.mirror_transform.IndexOf(gameObject.transform);

            Destroy(createMirror.mirror_transform[mirror_index - 1].gameObject);
            Destroy(gameObject);
            createMirror.mirror_transform.RemoveAt(mirror_index);
            createMirror.mirror_transform.RemoveAt(mirror_index - 1);
            CreateMirror.hasCreatedMirror = true;
        }
        if (collision.gameObject.tag == "mirror") {
            CreateMirror createMirror = GameObject.FindGameObjectWithTag("Player").GetComponent<CreateMirror>();

            int mirror_index = createMirror.mirror_transform.IndexOf(collision.gameObject.transform);
            if (mirror_index >= 1) {
                Destroy(createMirror.mirror_transform[mirror_index].gameObject);
                Destroy(createMirror.mirror_transform[mirror_index - 1].gameObject);

                createMirror.mirror_transform.RemoveAt(mirror_index);
                createMirror.mirror_transform.RemoveAt(mirror_index - 1);
            }

            
            mirror_index = createMirror.mirror_transform.IndexOf(gameObject.transform);
            if (mirror_index >= 1) {
                Destroy(createMirror.mirror_transform[mirror_index - 1].gameObject);
                Destroy(gameObject);

                createMirror.mirror_transform.RemoveAt(mirror_index);
                createMirror.mirror_transform.RemoveAt(mirror_index - 1);
            }



            CreateMirror.hasCreatedMirror = true;
        }

    }

}
