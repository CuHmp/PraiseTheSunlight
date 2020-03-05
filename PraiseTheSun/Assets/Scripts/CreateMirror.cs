using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMirror : MonoBehaviour {

    public Vector3 mousepos;
    Vector3 sun_position;

    // Start is called before the first frame update
    void Start() {
        sun_position = GameObject.FindGameObjectWithTag("Sun").transform.position;
    }

    // Update is called once per frame
    void Update() {
        
        if (Input.GetMouseButtonDown(0)) {

            mousepos = new Vector3(Input.mousePosition.x, sun_position.y, Input.mousePosition.z);
            Vector3 newPos = Camera.main.WorldToViewportPoint(mousepos);

            newPos.y = sun_position.y;

            GameObject startPoint = new GameObject();
            startPoint.transform.position = newPos;

        }
    }
}
