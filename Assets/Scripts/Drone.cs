using System;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputActions input;
    private PlantManager plantManager;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 5;
    private Vector2 moveInput;

    [Header("Plant details")]
    [SerializeField] private PlantType selectedPlant;

    private Crop targetedCrop;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerInputActions();
        plantManager = FindFirstObjectByType<PlantManager>();
    }

    void OnEnable()
    {
        input.Enable();

        input.Drone.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Drone.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        HandleMovement();
        HandlePlantAction();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crop"))
        {
            targetedCrop = other.gameObject.GetComponent<Crop>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Crop"))
            targetedCrop = null;
    }

    private void HandlePlantAction()
    {
        if (input.Drone.Plant.WasPressedThisFrame())
        {
            if (!targetedCrop)
            {
                return;
            }

            if (!targetedCrop.CanPlant())
            {
                return;
            }

            plantManager.PlantOnCrop(selectedPlant, targetedCrop);
        }
    }

    private void HandleMovement()
    {
        float xSpeed = moveInput.x * moveSpeed;
        float zSpeed = moveInput.y * moveSpeed;

        rb.linearVelocity = new Vector3(xSpeed, 0, zSpeed);
    }
}
