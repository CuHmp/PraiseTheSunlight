using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class sun_ray {
    public sun_ray(Ray origin, bool hasBounced = false) {
        this.ray = origin;
        this.hasBounced = hasBounced;
    }

    public Ray ray { get; set; }
    public bool hasBounced { get; set; }
}

public class SunRay : MonoBehaviour {


    LineRenderer line_renderer;
    Vector3 sun_pos;
    public Vector3 endPos;
    public Vector3 bouncyRayDir;

    public int rayLength = 40;

    private Ray startLight;
    private List<sun_ray> sun_rays;

    // Start is called before the first frame update
    void Start() {
        sun_rays = new List<sun_ray>();
        line_renderer = GetComponent<LineRenderer>();
        sun_pos = GameObject.FindGameObjectWithTag("Sun").transform.position;
        //endPos = transform.position * 30;
        startLight = new Ray();
        startLight.origin = transform.position;
        startLight.direction = new Vector3(-1, sun_pos.y / rayLength, 0);
        sun_rays.Add(new sun_ray(startLight));
    }

    // Update is called once per frame
    void Update() {
        if (CreateMirror.hasCreatedMirror) {
            sun_rays.Clear();
            sun_rays.Add(new sun_ray(startLight));
            line_renderer.positionCount = 2;
            line_renderer.SetPosition(line_renderer.positionCount - 1, startLight.direction * rayLength);
            CreateMirror.hasCreatedMirror = false;
        }
    }

    private void FixedUpdate() {

        RaycastHit hit;
        for (int i = 0; i < sun_rays.Count; i++) {
            if (Physics.Linecast(sun_rays[i].ray.origin, sun_rays[i].ray.direction * rayLength, out hit)) {
                if (hit.collider.gameObject.tag == "mirror" && !sun_rays[i].hasBounced) {
                    line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                    line_renderer.positionCount++;

                    sun_rays[i].hasBounced = true;

                    sun_rays.Add(new sun_ray(createNewRay(ref hit)));
                }
                if (hit.collider.gameObject.tag == "planet") {
                    line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                    line_renderer.positionCount = i;
                }
            }
            else {
                line_renderer.SetPosition(line_renderer.positionCount - 1, sun_rays[sun_rays.Count - 1].ray.direction * rayLength);
            }
        }
    }
    
    Ray createNewRay(ref RaycastHit hit) {
        bouncyRayDir = Vector3.Reflect(sun_rays[sun_rays.Count-1].ray.direction, hit.normal);
        bouncyRayDir = bouncyRayDir.normalized;
        bouncyRayDir = new Vector3(bouncyRayDir.x, sun_pos.y / rayLength, bouncyRayDir.z);
        return new Ray(hit.point, bouncyRayDir);
    }
}
