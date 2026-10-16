using System.Collections;
using TMPro;
using UnityEngine;

public class Seed : MonoBehaviour
{
    [Header("Life step")]
    [SerializeField] private GameObject seedStepPrefab;
    [SerializeField] private GameObject growStepPrefab;
    [SerializeField] private GameObject ripStepPrefab;
    [SerializeField] private Material rottenMat;
    [SerializeField] private GameObject collectiblePrefab;

    [Header("Score details")]
    [SerializeField] private int pointsValue;
    [SerializeField] private GameObject floatingTextPrefab;

    [Header("Growing details")]
    [SerializeField] private float seedTime = 5;
    [SerializeField] private float growTime = 5;
    [SerializeField] private float ripTime = 5;
    [SerializeField] private float rottenTime = 5;

    [Header("Water details")]
    [SerializeField] private float waterGrowthMultiplier = 1.5f;
    [SerializeField] private float pointsMultiplier = 1.5f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem rottenPS;
    [SerializeField] private ParticleSystem harvestPS;
    [SerializeField] private AudioSource harvestAudioSource;

    private ScoreManager scoreManager;
    private float stateTimer;
    private PlantState state;
    private GameObject currentPlant;
    private bool hasBeenWatered = false;
    private bool hasBeenHarvested = false;

    private void Start()
    {
        ChangeState(PlantState.Seed);
    }

    private void Update()
    {
        UpdateState();
    }

    public void Initialize(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    public void WaterSeed() => hasBeenWatered = true;

    public bool CanHarvest() => state == PlantState.Rip && !hasBeenHarvested;

    public void Harvest(Transform drone)
    {
        hasBeenHarvested = true;

        HandleScore();

        CreateCollectible(drone);
        PlayEffects();

        DestroyCurrentPlant();
        Destroy(gameObject, .5f);
    }

    private void PlayEffects()
    {
        harvestAudioSource.Play();
        harvestPS.Play();
    }

    private void HandleScore()
    {
        float finalPoints = hasBeenWatered ? pointsValue * pointsMultiplier : pointsValue;
        int roundedPoints = (int)Mathf.Floor(finalPoints);

        scoreManager.IncreaseScore(roundedPoints);

        Vector3 spawnPoint = transform.position + Vector3.up;

        GameObject floatingText = Instantiate(floatingTextPrefab, spawnPoint, Quaternion.identity);
        floatingText.GetComponentInChildren<TextMeshPro>().text = roundedPoints.ToString();
    }

    private void CreateCollectible(Transform drone)
    {
        GameObject instance = Instantiate(collectiblePrefab, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.identity);
        instance.GetComponent<Collectible>().SetTarget(drone, new Vector3(0, 1, 0));
    }

    private void UpdateState()
    {
        if (hasBeenHarvested)
            return;

        float growthMultiplier = hasBeenWatered ? waterGrowthMultiplier : 1;
        stateTimer += Time.deltaTime * growthMultiplier;

        switch (state)
        {
            case PlantState.Seed:
                {
                    if (stateTimer >= seedTime)
                        ChangeState(PlantState.Grow);

                    break;
                }

            case PlantState.Grow:
                {
                    if (stateTimer >= growTime)
                        ChangeState(PlantState.Rip);

                    break;
                }

            case PlantState.Rip:
                {
                    if (stateTimer >= ripTime)
                        ChangeState(PlantState.Rotten);

                    break;
                }

            case PlantState.Rotten:
                {
                    if (stateTimer >= rottenTime)
                        Destroy(gameObject);

                    break;
                }
        }
    }

    private void ChangeState(PlantState newState)
    {
        state = newState;
        stateTimer = 0;

        switch (newState)
        {
            case PlantState.Seed:
                {
                    CreateNewPlant(seedStepPrefab);
                    break;
                }
            case PlantState.Grow:
                {
                    DestroyCurrentPlant();
                    CreateNewPlant(growStepPrefab);
                    break;
                }
            case PlantState.Rip:
                {
                    DestroyCurrentPlant();
                    CreateNewPlant(ripStepPrefab);
                    break;
                }
            case PlantState.Rotten:
                {
                    rottenPS.Play();
                    currentPlant.GetComponent<MeshRenderer>().material = rottenMat;
                    break;
                }
        }
    }

    private void DestroyCurrentPlant() => Destroy(currentPlant);

    private void CreateNewPlant(GameObject step) => currentPlant = Instantiate(step, transform);
}
