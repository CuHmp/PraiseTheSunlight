using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMirror : MonoBehaviour {

    private Vector3 sun_position;
    private GameObject mouse_position;
    public float speed = 20.0f;

    public List<Transform> mirror_transform;

    // Start is called before the first frame update
    void Start() {
        sun_position = GameObject.FindGameObjectWithTag("Sun").transform.position;
        mouse_position = GameObject.FindGameObjectWithTag("mouse");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        mouse_position.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update() {
        float x = Input.GetAxis("Mouse X") * speed * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
        mouse_position.transform.Translate(x, y, 0);
        if (Input.GetMouseButtonDown(0)) {
            Vector3 newPos = mouse_position.transform.position;
            newPos.y = sun_position.y;

            GameObject startPoint = new GameObject();
            startPoint.name = "startMirror" + (mirror_transform.Count / 2);
            startPoint.tag = "mirror";
            startPoint.transform.position = newPos;
            mirror_transform.Add(startPoint.transform);
        }
        if (Input.GetMouseButtonUp(0)) {
            Vector3 newPos = mouse_position.transform.position;
            newPos.y = sun_position.y;

            GameObject endPoint = new GameObject();
            endPoint.name = "endMirror" + (mirror_transform.Count / 2);
            endPoint.tag = "mirror";
            endPoint.transform.position = newPos;

            mirror_transform.Add(endPoint.transform);

            createLineRenderer(mirror_transform[mirror_transform.Count - 2].transform, endPoint.transform);
        }
    }

    void createLineRenderer(Transform startPos, Transform endPos) {
        LineRenderer lineRenderer = endPos.gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, endPos.position);
        lineRenderer.SetPosition(1, startPos.position);
        lineRenderer.widthCurve = AnimationCurve.Linear(0, 0.1f, 1, 0.1f);
    }
}
