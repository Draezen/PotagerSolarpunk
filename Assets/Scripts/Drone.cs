using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DroneMovement : MonoBehaviour
{
    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private AudioSource moveSFX;
    private Vector2 moveInput;
    private bool canMove = true;
    private float moveSfxOriginalPitch;

    [Header("Water details")]
    [SerializeField] private float waterDuration = 1f;
    [SerializeField] private AudioSource waterSFX;
    [SerializeField] private ParticleSystem waterPS;

    [Header("Plant details")]
    [SerializeField] private AudioSource plantAS;

    private PlayerInputActions input;
    private Rigidbody rb;
    private PlantManager plantManager;
    private Cropfield cropUnderDrone;
    private Seed seedUnderdrone;
    private Coroutine immobilizeDroneCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        plantManager = FindFirstObjectByType<PlantManager>();
        moveSfxOriginalPitch = moveSFX.pitch;

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Drone.Move.performed += OnMovePerformed;
        input.Drone.Move.canceled += OnMoveCanceled;

        input.Drone.Plant.started += OnPlantActionStarted;
        input.Drone.Water.started += OnWaterActionStarted;

        input.Enable();
    }

    private void OnDisable()
    {
        input.Drone.Move.performed -= OnMovePerformed;
        input.Drone.Move.canceled -= OnMoveCanceled;

        input.Drone.Plant.started -= OnPlantActionStarted;
        input.Drone.Water.started -= OnWaterActionStarted;

        input.Disable();
    }

    private void Update()
    {
        HandleMovement();
        CheckIfCanHarvest();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.CompareTag("Crop"))
            cropUnderDrone = go.GetComponent<Cropfield>();

        if (go.CompareTag("Seed"))
        {
            seedUnderdrone = go.GetComponent<Seed>();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    private void OnMoveCanceled(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;

    private void OnWaterActionStarted(InputAction.CallbackContext context)
    {
        if (!seedUnderdrone)
            return;

        PlayWaterEffect();
        PlayWaterSFX();
        ImmobilizeDrone();
        seedUnderdrone.WaterSeed();
    }

    private void OnPlantActionStarted(InputAction.CallbackContext context)
    {
        if (!cropUnderDrone)
            return;

        if (!cropUnderDrone.CanPlant())
            return;

        plantManager.PlantOnCrop(cropUnderDrone);
        plantAS.Play();
    }

    private void CheckIfCanHarvest()
    {
        if (seedUnderdrone && seedUnderdrone.CanHarvest())
            seedUnderdrone.Harvest(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.CompareTag("Crop"))
            cropUnderDrone = null;

        if (go.CompareTag("Seed"))
            seedUnderdrone = null;
    }


    private void HandleMovement()
    {
        HandleMotorSound();

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float xSpeed = moveInput.x * moveSpeed;
        float zSpeed = moveInput.y * moveSpeed;

        rb.linearVelocity = new Vector3(xSpeed, 0, zSpeed);
    }

    private void HandleMotorSound()
    {
        float targetPitch = rb.linearVelocity.magnitude > 0.1f ? moveSfxOriginalPitch + .2f : moveSfxOriginalPitch;
        moveSFX.pitch = Mathf.Lerp(moveSFX.pitch, targetPitch, Time.deltaTime * 2f);
    }

    private void PlayWaterEffect() => waterPS.Play();

    private void PlayWaterSFX() => waterSFX.Play();

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
