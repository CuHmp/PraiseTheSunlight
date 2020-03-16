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

    private List<Transform> tree_scale;

    private Vector3 sun_pos;
    private Vector3 bouncyRayDir;
    private List<Vector3> goal_tree_scale;

    private float ice_melting = 0;
    private float grass_growing = 0;
    private float goal_timer = 3.0f;


    public int rayLength = 40;
    public float grow_value = 0.01f;
    public bool tree_done_growing;

    public GameObject WinText;

    Renderer renderer;

    // Start is called before the first frame update
    void Start() {
        WinText.SetActive(false);
        sun_rays = new List<sun_ray>();

        tree_scale = new List<Transform>();
        goal_tree_scale = new List<Vector3>();
        for (int i = 0; i < GameObject.FindGameObjectsWithTag("Tree").Length; i++) {
            tree_scale.Add(GameObject.FindGameObjectsWithTag("Tree")[i].transform);
        }
        foreach (Transform t in tree_scale) {
            goal_tree_scale.Add(new Vector3(t.localScale.x, t.localScale.y, t.localScale.z));
            t.localScale = new Vector3(0, 0, 0);
        }

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

        if (tree_done_growing) {
            goal_timer -= Time.deltaTime;
        }
        else {
            goal_timer = 3.0f;
        }
        if(goal_timer <= 0) {
            WinText.SetActive(true);
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
                    else if (tag == "planet" || tag == "Sun") {
                        line_renderer.positionCount = i + 2;
                        line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                        removeAllElementsBehind(ref sun_rays, i);
                        sun_rays[i].hasBounced = false;
                    }
                    else if(tag == "target_planet") {
                        line_renderer.positionCount = i + 2;
                        line_renderer.SetPosition(line_renderer.positionCount - 1, hit.point);
                        removeAllElementsBehind(ref sun_rays, i);
                        sun_rays[i].hasBounced = false;
                        if (renderer == null) {
                            renderer = hit.collider.gameObject.GetComponent<Renderer>();
                        }
                        else {
                            if (ice_melting <= 1) {
                                ice_melting += grow_value;
                                renderer.material.SetFloat("Ice_Melt", ice_melting);
                            }
                            else if (grass_growing <= 1 && ice_melting >= 1) {
                                grass_growing += grow_value;
                                renderer.material.SetFloat("Grass_Grow", grass_growing);
                            }
                            else if(grass_growing >= 1) {
                                for (int j = 0; j < goal_tree_scale.Count; j++) {
                                    if(tree_scale[j].localScale.x < goal_tree_scale[j].x) {
                                        float value = tree_scale[j].localScale.x + grow_value * 4;
                                        tree_scale[j].localScale = new Vector3(value, value, value);
                                        tree_done_growing = false;
                                    }
                                    else {
                                        tree_done_growing = true;
                                    }
                                }
                            }
                        }
                    }

                }
                else {
                    line_renderer.SetPosition(line_renderer.positionCount - 1, sun_rays[sun_rays.Count - 1].ray.direction * rayLength);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void removeAllElementsBehind(ref List<sun_ray> list, int index) {
        for(int i = index + 1; i < list.Count; i++) {
            list.RemoveAt(i);
        }
    }
    
    Ray createNewRay(ref RaycastHit hit) {
        bouncyRayDir = Vector3.Reflect(sun_rays[sun_rays.Count-1].ray.direction, hit.normal);
        bouncyRayDir = bouncyRayDir.normalized;
        bouncyRayDir = new Vector3(bouncyRayDir.x, sun_pos.y / rayLength, bouncyRayDir.z);
        return new Ray(hit.point, bouncyRayDir);
    }
}
