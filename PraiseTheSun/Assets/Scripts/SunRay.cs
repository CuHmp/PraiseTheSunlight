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

    private List<sun_ray> sun_rays;
    private Ray startLight;
    private LineRenderer line_renderer;
    private Vector3 sun_pos;
    private Vector3 bouncyRayDir;
    private bool hasCollidedWithPlanet = false;


    public int rayLength = 40;

    public float ice_melting = 0;

    Renderer renderer;

    // Start is called before the first frame update
    void Start() {
        sun_rays = new List<sun_ray>();
        line_renderer = GetComponent<LineRenderer>();
        sun_pos = GameObject.FindGameObjectWithTag("Sun").transform.position;


        startLight = new Ray();
        startLight.origin = transform.position;
        startLight.direction = new Vector3(-1, sun_pos.y / rayLength, 0);
        sun_rays.Add(new sun_ray(startLight));

        StartCoroutine("SunrayBouncer");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            CreateMirror.hasCreatedMirror = true;
        }

        if (CreateMirror.hasCreatedMirror) {
            sun_rays.Clear();
            sun_rays.Add(new sun_ray(startLight));
            line_renderer.positionCount = 2;
            line_renderer.SetPosition(line_renderer.positionCount - 1, startLight.direction * rayLength);
            CreateMirror.hasCreatedMirror = false;
        }
    }

    IEnumerator SunrayBouncer() {
        while (true) {
            RaycastHit hit;
            for (int i = 0; i < sun_rays.Count; i++) {
                if (Physics.Linecast(sun_rays[i].ray.origin, sun_rays[i].ray.direction * rayLength, out hit)) {
                    string tag = hit.collider.gameObject.tag;
                    if (tag == "mirror" && !sun_rays[i].hasBounced) {
                        line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                        line_renderer.positionCount++;

                        sun_rays[i].hasBounced = true;

                        sun_rays.Add(new sun_ray(createNewRay(ref hit)));
                    }
                    else if (tag == "planet") {
                        line_renderer.positionCount = i + 2;
                        line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                        hasCollidedWithPlanet = true;
                    }
                    else if(tag == "target_planet") {
                        line_renderer.positionCount = i + 2;
                        line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                        hasCollidedWithPlanet = true;
                        if (renderer == null) {
                            renderer = hit.collider.gameObject.GetComponent<Renderer>();
                        }
                        else {
                            if (ice_melting <= 1) {
                                ice_melting += 0.01f;
                                renderer.material.SetFloat("Ice_Melt", ice_melting);
                            }
                        }
                    }

                }
                else {
                    line_renderer.SetPosition(line_renderer.positionCount - 1, sun_rays[sun_rays.Count - 1].ray.direction * rayLength);
                    if (hasCollidedWithPlanet) {
                        CreateMirror.hasCreatedMirror = true;
                        hasCollidedWithPlanet = false;
                    }
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    Ray createNewRay(ref RaycastHit hit) {
        bouncyRayDir = Vector3.Reflect(sun_rays[sun_rays.Count-1].ray.direction, hit.normal);
        bouncyRayDir = bouncyRayDir.normalized;
        bouncyRayDir = new Vector3(bouncyRayDir.x, sun_pos.y / rayLength, bouncyRayDir.z);
        return new Ray(hit.point, bouncyRayDir);
    }
}
