using UnityEngine;

public class PlantManager : MonoBehaviour
{
    [Header("Plants")]
    [SerializeField] private PlantDetail[] plants;

    public void PlantOnCrop(PlantType type, Cropfield crop)
    {
        GameObject seed = GetSeedForPlant(type);

        if (seed == null)
        {
            Debug.Log("Seed not found");
            return;
        }

        crop.PlantSeed(seed);
    }

    private GameObject GetSeedForPlant(PlantType type)
    {
        foreach (PlantDetail plant in plants)
        {
            if (plant.type == type)
                return plant.seed;
        }

        return null;
    }
}

