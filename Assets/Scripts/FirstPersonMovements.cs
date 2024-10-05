using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstPersonMovements : MonoBehaviour
{
    public int keyCount = 0;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Jump")]
    public bool enableJump;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ground;
    bool grounded;
    public float groundDrag;

    [Header("Respawn Settings")]
    public Vector3 respawnPosition;
    public Quaternion respawnRotation;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    public Action<int> onKeyCollected;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        
        if (grounded) rb.drag = groundDrag;
        else rb.drag = 0;
        
        Inputs();
        speedControl();
        Respawn();

        
    }

    private void FixedUpdate()
    {
        Movements();
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (enableJump)
        {
            if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
            {
                readyToJump = false;

                Jump();

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }
    }

    private void Movements()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded) rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void speedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R) || gameObject.transform.position.y < 0)
        {
            transform.position = respawnPosition;
            transform.rotation = respawnRotation;

            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            other.gameObject.SetActive(false);
            StageComplete.Instance.SetUp();
        } 
        else if (other.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            ++keyCount;
            onKeyCollected(keyCount);
        }
    }
}
