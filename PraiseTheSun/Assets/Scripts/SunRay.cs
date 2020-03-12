using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRay : MonoBehaviour {
    LineRenderer line_renderer;
    Vector3 sun_pos;
    public Vector3 endPos;
    public Vector3 bouncyRayDir;

    public int rayLength = 40;

    private Ray startLight;
    private Ray sun_ray;

    // Start is called before the first frame update
    void Start() {
        sun_ray = new Ray();
        line_renderer = GetComponent<LineRenderer>();
        sun_pos = GameObject.FindGameObjectWithTag("Sun").transform.position;
        //endPos = transform.position * 30;
        startLight = new Ray();
        startLight.origin = transform.position;
        startLight.direction = new Vector3(-1, sun_pos.y / rayLength, 0);
        sun_ray = startLight;
    }

    // Update is called once per frame
    void Update() {
        if (CreateMirror.hasCreatedMirror) {
            sun_ray = startLight;
            line_renderer.positionCount = 2;
            line_renderer.SetPosition(line_renderer.positionCount - 1, sun_ray.direction * rayLength);
            CreateMirror.hasCreatedMirror = false;
        }
    }

    private void FixedUpdate() {

        RaycastHit hit;
        if (Physics.Linecast(sun_ray.origin, sun_ray.direction * rayLength, out hit)) {
            if (hit.collider.gameObject.tag == "mirror") {
                line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                line_renderer.positionCount++;

                sun_ray = createNewRay(ref hit);
            }
        }
        else {
            line_renderer.SetPosition(line_renderer.positionCount - 1, sun_ray.direction * rayLength);
        }
    }
    
    Ray createNewRay(ref RaycastHit hit) {
        bouncyRayDir = Vector3.Reflect(sun_ray.direction, hit.normal);
        bouncyRayDir = bouncyRayDir.normalized;
        bouncyRayDir = new Vector3(bouncyRayDir.x, sun_pos.y / rayLength, bouncyRayDir.z);
        return new Ray(hit.point, bouncyRayDir);
    }
}
