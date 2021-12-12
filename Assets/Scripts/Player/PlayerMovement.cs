using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement instance;

    static KeyCode jumpKey = KeyCode.Space;

    [Header("References")]
    public Rigidbody rb;
    public Transform orientation;

    [Header("Basic Settings")]
    public float jumpHeight;
    public float groundSpeed, groundAcceleration;
    public float airSpeed, airAcceleration;    

    [Header("Wallrun Settings")]
    public float minSpeed;
    public float boostSpeed;
    public float minHeight;

    private bool isGrounded;
    private bool isWallRunning;
    private bool justJumped;
    private bool canDoubleJump;
    
    private Vector3 inputMoveDirection;
    private Vector3 jumpForce;

    private void GetInput() {
        inputMoveDirection = Vector3.Normalize(
            orientation.forward * Input.GetAxisRaw("Vertical") + 
            orientation.right * Input.GetAxisRaw("Horizontal"));
    }

    private void Jump() => rb.AddForce(jumpForce, ForceMode.Impulse);

    private void MoveOnGround() {

        // Jump
        if (Input.GetKey(jumpKey) && !justJumped) {
            Jump(); 
            
            justJumped = true;
            isGrounded = false;
            canDoubleJump = true;

            return;
        }

        rb.AddForce(inputMoveDirection * groundAcceleration, ForceMode.Force);

        // Drag
        rb.AddForce(-rb.velocity * groundAcceleration / groundSpeed, ForceMode.Force);

    }

    private void MoveInAir() {

        // Double jump
        if (canDoubleJump && Input.GetKeyDown(jumpKey)) {
            Jump();
            canDoubleJump = false;
        }

        // Air strafe

    }

    private void StartWallRun() { }

    private void ContinueWallRun() { }

    private Vector3 GetBestNormal(Collision collision) {
        Vector3 best = Vector3.down;

        foreach (ContactPoint contact in collision.contacts) {
            if (contact.normal.y > best.y) best = contact.normal;
        }

        return best;
    }

    private bool IsNormalGround(Vector3 normal) => normal.y > 0.99f;
    private bool IsNormalWall(Vector3 normal) => Mathf.Abs(normal.y) < 0.01f;

    private bool CanWallRun() => !Physics.Raycast(transform.position, Vector3.down, minHeight);

    private void OnCollisionEnter(Collision collision) {

        Vector3 normal = GetBestNormal(collision);

        if (IsNormalGround(normal)) { 
            isGrounded = true;
            isWallRunning = false;
        }
    }

    private void OnCollisionStay(Collision collision) {
        Vector3 normal = GetBestNormal(collision);

        if (IsNormalGround(normal)) {
            isGrounded = true;
            isWallRunning = false;
        }
    }

    private void OnCollisionExit(Collision collision) {
        isGrounded = false;
        justJumped = false;
    }

    private void Update() {
        GetInput();

        if (isWallRunning) ContinueWallRun();
        else if (!isGrounded) MoveInAir();
        else MoveOnGround();
        
    }


    private void Awake() {
        instance = this;

        jumpForce = Vector3.up * Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
    }
}
