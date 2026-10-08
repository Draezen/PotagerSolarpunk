using System;
using System.Collections;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInputActions input;
    private PlantManager plantManager;
    private ParticleSystem ps;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 5;
    private Vector2 moveInput;
    private bool canMove = true;

    [Header("Plant details")]
    [SerializeField] private PlantType selectedPlant;

    [Header("Water details")]
    [SerializeField] private float waterDuration = 1f;

    private Cropfield cropDetected;
    private Seed seedDetected;
    private Coroutine immobilizeDroneCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerInputActions();
        plantManager = FindFirstObjectByType<PlantManager>();
        ps = GetComponentInChildren<ParticleSystem>();
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
        HandleWatering();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.CompareTag("Crop"))
            cropDetected = go.GetComponent<Cropfield>();

        if (go.CompareTag("Seed"))
        {
            seedDetected = go.GetComponent<Seed>();
            if (seedDetected.CanGather())
                seedDetected.Gather(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.CompareTag("Crop"))
            cropDetected = null;

        if (go.CompareTag("Seed"))
            seedDetected = null;
    }

    private void HandleMovement()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float xSpeed = moveInput.x * moveSpeed;
        float zSpeed = moveInput.y * moveSpeed;

        rb.linearVelocity = new Vector3(xSpeed, 0, zSpeed);
    }

    private void HandlePlantAction()
    {
        if (input.Drone.Plant.WasPressedThisFrame())
        {
            if (!cropDetected)
                return;

            if (!cropDetected.CanPlant())
                return;

            plantManager.PlantOnCrop(selectedPlant, cropDetected);
        }
    }

    private void HandleWatering()
    {
        if (input.Drone.Water.WasPressedThisFrame())
        {
            if (!seedDetected)
                return;

            PlayWaterEffect();
            ImmobilizeDrone();
            seedDetected.WaterSeed();
        }
    }

    private void PlayWaterEffect() => ps.Play();

    private void ImmobilizeDrone()
    {
        if (immobilizeDroneCoroutine != null)
            StopCoroutine(immobilizeDroneCoroutine);

        immobilizeDroneCoroutine = StartCoroutine(ImmobilizeDroneCo());
    }

    private IEnumerator ImmobilizeDroneCo()
    {
        canMove = false;

        yield return new WaitForSeconds(waterDuration);

        canMove = true;
    }
}
