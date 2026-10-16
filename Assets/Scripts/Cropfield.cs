using UnityEngine;

public class Cropfield : MonoBehaviour
{
    [Header("Light effect")]
    [SerializeField] private MeshRenderer lightEffect;
    [SerializeField] private Color cropIsEmptyColor;
    [SerializeField] private Color cropHasPlantColor;
    private Color originalColor;

    private GameObject seedInCrop;
    private bool droneIsPresent;

    private void Awake()
    {
        originalColor = lightEffect.material.color;
    }

    private void Update()
    {
        CheckIfDronePresent();
    }

    public bool CanPlant() => seedInCrop == null;

    public void PlantSeed(GameObject seed, ScoreManager scoreManager)
    {
        Vector3 pos = new(transform.position.x, 0.1f, transform.position.z);

        seedInCrop = Instantiate(seed, pos, Quaternion.identity);
        seedInCrop.GetComponent<Seed>().Initialize(scoreManager);

        ActivateLightEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            droneIsPresent = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            droneIsPresent = false;
            ResetLightEffect();
        }
    }

    private void CheckIfDronePresent()
    {
        if (droneIsPresent)
            ActivateLightEffect();
    }

    private void ActivateLightEffect() => lightEffect.material.color = seedInCrop ? cropHasPlantColor : cropIsEmptyColor;

    private void ResetLightEffect() => lightEffect.material.color = originalColor;
}
