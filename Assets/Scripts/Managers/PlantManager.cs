using System;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    [Header("Plants")]
    [SerializeField] private PlantDetail[] plants;

    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;

    public void PlantOnCrop(Cropfield crop)
    {
        PlantType RandomPlant = PickRandomPlant();

        GameObject seedPrefab = GetSeedForPlant(RandomPlant);

        if (seedPrefab == null)
        {
            Debug.LogWarning("Seed not found");
            return;
        }

        crop.PlantSeed(seedPrefab, scoreManager);
    }

    private GameObject GetSeedForPlant(PlantType type)
    {
        foreach (PlantDetail plant in plants)
        {
            if (plant.type == type)
                return plant.seedPrefab;
        }

        return null;
    }

    private PlantType PickRandomPlant()
    {
        return (PlantType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PlantType)).Length);
    }
}

