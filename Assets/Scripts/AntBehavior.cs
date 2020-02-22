using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : MonoBehaviour
{
    private readonly float speed = 2.0f;
    private static readonly float MAX_SEARCH_SECONDS = 0.5f;

    private float searchSeconds;
    private Vector3 velocity;
    private Rigidbody myRigidbody;

    void Start() {
        myRigidbody = GetComponent<Rigidbody>();
        searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
    }

    // Update is called once per frame
    void Update() {
        searchSeconds -= Time.deltaTime;

        if (searchSeconds <= 0.0f) {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)).normalized;
            velocity = direction * speed;
            searchSeconds = Random.Range(0.0f, MAX_SEARCH_SECONDS);
        }
    }

    void FixedUpdate() {
        // Note that deltaTime here automatically recognizes it's inside of FixedUpdate
        myRigidbody.position += velocity * Time.deltaTime;
    }
}
