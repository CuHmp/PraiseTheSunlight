using UnityEngine;

public class OrbitObject : MonoBehaviour {

    public float OrbitDegrees;
    public GameObject orbit_object;
    public Vector3 rotationAxis;

    private Vector3 orbitPosition;


    void Start() {
        orbitPosition = orbit_object.transform.position;
    }


    void Update() {
        transform.Rotate(rotationAxis);
        transform.parent.transform.position = RotatePointAroundPivot(transform.position, orbitPosition, Quaternion.Euler(0, OrbitDegrees * Time.deltaTime, 0));
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
        return angle * (point - pivot) + pivot;
    }
}
