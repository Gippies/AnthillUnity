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

    private float searchSeconds;
    private GameObject approaching;
    private State currentState;
    private Vector3 velocity;
    private Rigidbody myRigidbody;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        currentState = State.SEARCH;
        approaching = null;
    }

    private void Search() {
        searchSeconds -= Time.deltaTime;
        if (searchSeconds <= 0.0f) {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            velocity = direction * speed;
            searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        }
        foreach (GameObject leafy in LeafyManager.leafies) {
            if (transform.position.x - MAX_SEARCH_RADIUS <= leafy.transform.position.x && leafy.transform.position.x <= transform.position.x + MAX_SEARCH_RADIUS &&
                transform.position.y - MAX_SEARCH_RADIUS <= leafy.transform.position.y && leafy.transform.position.y <= transform.position.y + MAX_SEARCH_RADIUS &&
                transform.position.z - MAX_SEARCH_RADIUS <= leafy.transform.position.z && leafy.transform.position.z <= transform.position.z + MAX_SEARCH_RADIUS) {
                approaching = leafy;
                currentState = State.GET_THING;
            }
        }
    }

    private void GetThing() {
        Vector3 direction = (approaching.transform.position - transform.position).normalized;
        velocity = direction * speed;
    }

    // Update is called once per frame
    void Update() {
        if (currentState == State.SEARCH)
            Search();
        else if (currentState == State.GET_THING)
            GetThing();
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
}