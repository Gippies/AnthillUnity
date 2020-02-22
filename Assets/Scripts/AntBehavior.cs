using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : MonoBehaviour
{
    private readonly float speed = 6f;

    private Vector3 velocity;
    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 direction = input.normalized;
        velocity = direction * speed;
    }

    void FixedUpdate()
    {
        // Note that deltaTime here automatically recognizes it's inside of FixedUpdate
        myRigidbody.position += velocity * Time.deltaTime;
    }
}
