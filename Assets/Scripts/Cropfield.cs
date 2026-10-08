using UnityEngine;

public class Cropfield : MonoBehaviour
{
    [Header("Light effect")]
    [SerializeField] private MeshRenderer lightEffect;
    [SerializeField] private Color cropIsEmptyColor;
    [SerializeField] private Color cropHasPlantColor;
    private Color originalColor;

    private GameObject seedInCrop;

    void Awake()
    {
        originalColor = lightEffect.material.color;
    }

    public bool CanPlant() => seedInCrop == null;

    public void PlantSeed(GameObject seed)
    {
        Vector3 pos = new(transform.position.x, 0.1f, transform.position.z);

        seedInCrop = Instantiate(seed, pos, Quaternion.identity);
        ActivateLightEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ActivateLightEffect();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ResetLightEffect();
    }

    private void ActivateLightEffect() => lightEffect.material.color = seedInCrop ? cropHasPlantColor : cropIsEmptyColor;

    private void ResetLightEffect() => lightEffect.material.color = originalColor;
}
