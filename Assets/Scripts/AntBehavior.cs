using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AntType {
    GATHERER,
    DIGGER
}

public enum AntState {
    SEARCH,
    GET_THING,
    RETURN_TO_HILL,
    RETURN_TO_SURFACE,
    DROP_THING
}

public class AntBehavior : MonoBehaviour {
    private static readonly float MAX_SEARCH_RADIUS = 1.0f;
    private static readonly float DROP_ZONE_RADIUS = 1.0f;
    private static readonly float MAX_SEARCH_SECONDS = 1.0f;

    public RootManager rootManager;
    public AntType antType;

    private readonly float speed = 2.0f;

    private float searchSeconds;
    private GameObject approaching;
    private GameObject carrying;
    private GameObject touchingGameObject;
    private AntState currentAntState;
    private Vector3 velocity;
    private Vector3 carryingPosition;
    private Vector3 dropPosition;
    private Rigidbody myRigidbody;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        carryingPosition = new Vector3(0, transform.localScale.y, 0);
        dropPosition = Vector3.zero;
        searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        currentAntState = AntState.SEARCH;
        velocity = Vector3.zero;
        approaching = null;
        carrying = null;
        touchingGameObject = null;
    }

    private void Search(List<GameObject> carriableList) {
        searchSeconds -= Time.deltaTime;
        if (searchSeconds <= 0.0f) {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            velocity = direction * speed;
            searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        }
        foreach (GameObject carriable in carriableList) {
            CarriableBehavior carriableBehavior = carriable.GetComponent<CarriableBehavior>();
            if (!carriableBehavior.beingApproachedBy && !carriableBehavior.beingCarriedBy && !carriableBehavior.isStored &&
                carriableBehavior.InSearchArea(transform, MAX_SEARCH_RADIUS)) {

                approaching = carriable;
                carriableBehavior.beingApproachedBy = this;
                currentAntState = AntState.GET_THING;
                break;
            }
        }
    }

    private void GetThing() {
        Vector3 direction = (approaching.transform.position - transform.position).normalized;
        velocity = direction * speed;
        if (touchingGameObject && touchingGameObject == approaching) {
            carrying = approaching;
            carrying.GetComponent<CarriableBehavior>().beingCarriedBy = this;
            approaching = null;
            carrying.GetComponent<CarriableBehavior>().beingApproachedBy = null;

            carrying.transform.position = transform.position + carryingPosition;
            currentAntState = AntState.RETURN_TO_HILL;
        }
    }

    private void ReturnToHill() {
        Vector3 currentPosition = transform.position;
        Vector3 direction = (dropPosition - currentPosition).normalized;
        velocity = direction * speed;
        carrying.transform.position = currentPosition + carryingPosition;
        if (currentPosition.x > dropPosition.x - DROP_ZONE_RADIUS && currentPosition.x < dropPosition.x + DROP_ZONE_RADIUS &&
            currentPosition.y > dropPosition.y - DROP_ZONE_RADIUS && currentPosition.y < dropPosition.y + DROP_ZONE_RADIUS &&
            currentPosition.z > dropPosition.z - DROP_ZONE_RADIUS && currentPosition.z < dropPosition.z + DROP_ZONE_RADIUS) {
            carrying.transform.position = transform.position;

            CarriableBehavior carriableBehavior = carrying.GetComponent<CarriableBehavior>();
            carriableBehavior.isStored = true;
            carriableBehavior.beingCarriedBy = null;
            carrying = null;

            currentAntState = AntState.SEARCH;
        }
    }

    private void Climb() {
        RaycastHit hit;
        if (velocity.y <= 0.1f &&
            Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f) &&
            hit.collider.gameObject.CompareTag("Climbable") &&
            hit.distance < 0.5f) {
            
            velocity += Vector3.up * speed;
        }
    }

    // Update is called once per frame
    void Update() {
        if (currentAntState == AntState.SEARCH) {
            if (antType == AntType.GATHERER)
                Search(rootManager.GetLeafies());
            else if (antType == AntType.DIGGER)
                Search(rootManager.GetDirts());
        }
        else if (currentAntState == AntState.GET_THING)
            GetThing();
        else if (currentAntState == AntState.RETURN_TO_HILL)
            ReturnToHill();
        Climb();
    }

    void FixedUpdate() {
        // Note that deltaTime here automatically recognizes it's inside of FixedUpdate
        myRigidbody.position += velocity * Time.deltaTime;
        Vector3 lookVector = new Vector3(velocity.x, 0.0f, velocity.z);
        if (lookVector != Vector3.zero)
            myRigidbody.rotation = Quaternion.LookRotation(lookVector);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Gatherer")) {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.CompareTag("Carriable")) {
            touchingGameObject = collision.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Carriable")) {
            touchingGameObject = other.gameObject;
        }
    }
}
