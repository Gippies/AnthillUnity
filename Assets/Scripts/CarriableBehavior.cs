using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarriableBehavior : MonoBehaviour
{

    public AntBehavior beingApproachedBy;
    public AntBehavior beingCarriedBy;
    [FormerlySerializedAs("is_stored")] public bool isStored;
    public float searchRadius;

    // Start is called before the first frame update
    void Start()
    {
        beingApproachedBy = null;
        beingCarriedBy = null;
        isStored = false;
    }

    public bool InSearchArea(Transform antTransform) {
        Vector3 antPosition = antTransform.position;
        Vector3 thisPosition = transform.position;
        return antPosition.x - searchRadius <= thisPosition.x &&
               thisPosition.x <= antPosition.x + searchRadius &&
               antPosition.y - searchRadius <= thisPosition.y &&
               thisPosition.y <= antPosition.y + searchRadius &&
               antPosition.z - searchRadius <= thisPosition.z &&
               thisPosition.z <= antPosition.z + searchRadius;
    }
}
