using UnityEngine;

public class Seed : MonoBehaviour
{
    private ParticleSystem ps;

    [Header("Life step")]
    [SerializeField] private GameObject seedStep;
    [SerializeField] private GameObject growStep;
    [SerializeField] private GameObject ripStep;
    [SerializeField] private Material rottenMat;
    [SerializeField] private GameObject collectible;

    [Header("Growing details")]
    [SerializeField] private float seedTime = 5;
    [SerializeField] private float growTime = 5;
    [SerializeField] private float ripTime = 5;
    [SerializeField] private float rottenTime = 5;

    [Header("Water details")]
    [SerializeField] private float waterGrowthMultiplier = 1.5f;

    private float stateTimer;
    private PlantState state;
    private GameObject model;
    private bool wasWatered = false;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ChangeState(PlantState.Seed);
    }

    private void Update()
    {
        UpdateState();
    }

    public void WaterSeed() => wasWatered = true;

    public bool CanGather() => state == PlantState.Rip;

    public void Gather(Transform drone)
    {
        // play sound
        // play particles
        // gain points

        GameObject instance = Instantiate(collectible, new Vector3(transform.position.x, .5f, transform.position.z), Quaternion.identity);
        instance.GetComponent<Collectible>().SetTarget(drone, new Vector3(0, 1, 0));

        Destroy(gameObject);
    }

    private void UpdateState()
    {
        float growthMultiplier = wasWatered ? waterGrowthMultiplier : 1;
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
                    model = Instantiate(seedStep, transform);
                    break;
                }
            case PlantState.Grow:
                {
                    Destroy(model);
                    model = Instantiate(growStep, transform);
                    break;
                }
            case PlantState.Rip:
                {
                    Destroy(model);
                    model = Instantiate(ripStep, transform);
                    break;
                }
            case PlantState.Rotten:
                {
                    ps.Play();
                    model.GetComponent<MeshRenderer>().material = rottenMat;
                    break;
                }
        }
    }
}
