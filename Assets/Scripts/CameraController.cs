using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed;
    public float mouseSensitivity;

    float rotateAngleX;
    float rotateAngleY;
    Camera thisCamera;

    // Start is called before the first frame update
    void Start() {
        rotateAngleX = 0.0f;
        rotateAngleY = 0.0f;

        thisCamera = GetComponent<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void MoveCamera() {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Jump"), Input.GetAxisRaw("Vertical"));
        Vector3 velocity = input.normalized * speed * Time.deltaTime;
        rotateAngleX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotateAngleY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotateAngleY = Mathf.Clamp(rotateAngleY, -90.0f, 90.0f);

        transform.Translate(velocity, Space.Self);
        transform.eulerAngles = new Vector3(rotateAngleY, rotateAngleX);
    }

    // Update is called once per frame
    void Update() {
        MoveCamera();
    }
}
