using System;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputActions inputAction;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 5;
    private Vector2 moveInput;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputAction = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputAction.Enable();

        inputAction.Drone.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputAction.Drone.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputAction.Disable();
    }


    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float xSpeed = moveInput.x * moveSpeed;
        float zSpeed = moveInput.y * moveSpeed;

        rb.linearVelocity = new Vector3(xSpeed, 0, zSpeed);
    }
}
