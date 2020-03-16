using UnityEngine;

public class MirrorCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.tag == "planet" || collision.gameObject.tag == "target_planet" || collision.gameObject.tag == "mirror") {
            CreateMirror createMirror = GameObject.FindGameObjectWithTag("Player").GetComponent<CreateMirror>();
            int mirror_index = createMirror.mirror_transform.IndexOf(gameObject.transform);
            if (mirror_index == -1) {
                return;
            }

            if (mirror_index >= 1 && (createMirror.mirror_transform[mirror_index] != null || createMirror.mirror_transform[mirror_index - 1] != null)) {
                Destroy(createMirror.mirror_transform[mirror_index - 1].gameObject);
                Destroy(gameObject);
                createMirror.mirror_transform.RemoveAt(mirror_index);
                createMirror.mirror_transform.RemoveAt(mirror_index - 1);
            }
            CreateMirror.hasCreatedMirror = true;
        }
    }

}
