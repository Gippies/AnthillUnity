using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    SEARCH,
    GET_THING,
    RETURN_TO_HILL,
    RETURN_TO_SURFACE,
    DROP_THING
}

public class AntBehavior : MonoBehaviour {
    private static readonly float MAX_SEARCH_RADIUS = 1.0f;
    private static readonly float MAX_SEARCH_SECONDS = 0.5f;

    private readonly float speed = 2.0f;

    private bool is_touching_carriable;
    private bool is_touching_hill;
    private float searchSeconds;
    private GameObject approaching;
    private GameObject carrying;
    private State currentState;
    private Vector3 velocity;
    private Vector3 carryingPosition;
    private Rigidbody myRigidbody;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        carryingPosition = new Vector3(0, transform.localScale.y, 0);
        searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        currentState = State.SEARCH;
        approaching = null;
        carrying = null;
        is_touching_carriable = false;
        is_touching_hill = false;
    }

    private void Search() {
        searchSeconds -= Time.deltaTime;
        if (searchSeconds <= 0.0f) {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            velocity = direction * speed;
            searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        }
        foreach (GameObject leafy in LeafyManager.leafies) {
            if (leafy.GetComponent<CarriableBehavior>().beingApproachedBy == null && leafy.GetComponent<CarriableBehavior>().beingCarriedBy == null &&
                leafy.GetComponent<CarriableBehavior>().is_stored == false &&
                transform.position.x - MAX_SEARCH_RADIUS <= leafy.transform.position.x && leafy.transform.position.x <= transform.position.x + MAX_SEARCH_RADIUS &&
                transform.position.y - MAX_SEARCH_RADIUS <= leafy.transform.position.y && leafy.transform.position.y <= transform.position.y + MAX_SEARCH_RADIUS &&
                transform.position.z - MAX_SEARCH_RADIUS <= leafy.transform.position.z && leafy.transform.position.z <= transform.position.z + MAX_SEARCH_RADIUS) {

                approaching = leafy;
                leafy.GetComponent<CarriableBehavior>().beingApproachedBy = this;
                currentState = State.GET_THING;
                break;
            }
        }
    }

    private void GetThing() {
        Vector3 direction = (approaching.transform.position - transform.position).normalized;
        velocity = direction * speed;
        if (is_touching_carriable) {
            carrying = approaching;
            carrying.GetComponent<CarriableBehavior>().beingCarriedBy = this;
            approaching = null;
            carrying.GetComponent<CarriableBehavior>().beingApproachedBy = null;

            carrying.transform.position = transform.position + carryingPosition;
            currentState = State.RETURN_TO_HILL;
        }
    }

    private void ReturnToHill() {
        Vector3 direction = (Vector3.zero - transform.position).normalized;
        velocity = direction * speed;
        carrying.transform.position = transform.position + carryingPosition;
        if (is_touching_hill) {
            carrying.transform.position = transform.position;

            carrying.GetComponent<CarriableBehavior>().is_stored = true;
            carrying.GetComponent<CarriableBehavior>().beingCarriedBy = null;
            carrying = null;

            currentState = State.SEARCH;
        }
    }

    // Update is called once per frame
    void Update() {
        if (currentState == State.SEARCH)
            Search();
        else if (currentState == State.GET_THING)
            GetThing();
        else if (currentState == State.RETURN_TO_HILL)
            ReturnToHill();
    }

    void FixedUpdate()
    {
        // Note that deltaTime here automatically recognizes it's inside of FixedUpdate
        myRigidbody.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Gatherer") {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Carriable") {
            is_touching_carriable = true;
        }
        else if (other.gameObject.tag == "Hill") {
            is_touching_hill = true;
        }
    }
}
