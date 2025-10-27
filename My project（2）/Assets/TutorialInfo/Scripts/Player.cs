using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpHeight = 1.5f;

    [Header("Double Jump Settings")]
    public int maxJumpCount = 2;
    public float doubleJumpHeight = 1.2f;

    private CharacterController controller;
    private Camera cam;
    private float xRotation = 0f;

    private Vector3 velocity;
    private bool isGrounded;
    private float gravity = -9.81f;

    private int jumpCount = 0;
    private bool wasGrounded = true;

    [Header("Crosshair Settings")]
    public Color crosshairColor = Color.white;
    public int crosshairSize = 20;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CheckGroundStatus();
        isGrounded = controller.isGrounded;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        HandleJump();
    }

    void CheckGroundStatus()
    {
        if (!wasGrounded && controller.isGrounded)
        {
            jumpCount = 0;
        }
        wasGrounded = controller.isGrounded;
    }

    void HandleJump()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded && jumpCount == 0)
            {
                PerformJump(jumpHeight);
                jumpCount = 1;
            }
            else if (!isGrounded && jumpCount == 1)
            {
                PerformJump(doubleJumpHeight);
                jumpCount = 2;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void PerformJump(float jumpForce)
    {
        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }


    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = crosshairSize;
        style.normal.textColor = crosshairColor;
        style.fontStyle = FontStyle.Bold;


        GUI.Label(new Rect(Screen.width / 2 - crosshairSize / 2, Screen.height / 2 - crosshairSize / 2, crosshairSize, crosshairSize), "+", style);
    }
}