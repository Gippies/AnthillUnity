using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableBehavior : MonoBehaviour
{

    public AntBehavior beingApproachedBy;
    public AntBehavior beingCarriedBy;
    public bool is_stored;

    // Start is called before the first frame update
    void Start()
    {
        beingApproachedBy = null;
        beingCarriedBy = null;
        is_stored = false;
    }
}
