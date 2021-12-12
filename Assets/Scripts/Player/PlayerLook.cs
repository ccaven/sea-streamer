using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {
    public static PlayerLook instance;

    [Header("References")]
    public Transform cameraHolder;
    public Transform playerHead;
    public Transform orientation;

    [Header("Settings")]
    public float sensitivity;
    public float maxPitch;

    private float pitch;
    private float yaw;

    private Vector3 upVector;

    private Quaternion GetRotation() {
        Vector3 lookDirection = Quaternion.Euler(pitch, yaw, 0f) * Vector3.forward;
        return Quaternion.LookRotation(lookDirection, upVector);
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {

        if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButton(0)) {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Cursor.lockState == CursorLockMode.Locked && Input.GetKey(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
        }

        // Update pitch and yaw
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

        yaw += mouseX * Time.deltaTime;
        pitch -= mouseY * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        // Update camera's position and rotation
        cameraHolder.rotation = GetRotation();
        cameraHolder.position = playerHead.position;

        orientation.rotation = Quaternion.Euler(0, yaw, 0);
    }

    private void Awake() {
        instance = this;

        upVector = Vector3.up;
    }

}