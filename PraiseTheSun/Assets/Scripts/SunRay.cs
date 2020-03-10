using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRay : MonoBehaviour {
    LineRenderer line_renderer;
    Vector3 sun_pos;
    public Vector3 endPos;
    bool hasHitMirror = false;
    private Ray ray;
    private Vector3 inDirection;
    // Start is called before the first frame update
    void Start() {
        line_renderer = GetComponent<LineRenderer>();
        sun_pos = GameObject.FindGameObjectWithTag("Sun").transform.position;
        endPos = transform.position * 30;
        endPos.y = sun_pos.y;
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, endPos, out hit)) {
            Debug.DrawLine(transform.position, endPos, Color.green);
        }
        else {
            Debug.DrawLine(transform.position, endPos, Color.red);
        }

    }
   

    Vector3 Direction(Vector3 from,Vector3 to ) {
        Vector3 res = to - from;
        return res.normalized;
    }
}
