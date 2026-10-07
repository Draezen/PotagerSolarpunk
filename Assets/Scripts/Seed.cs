using UnityEngine;

public class Seed : MonoBehaviour
{
    private ParticleSystem ps;

    [Header("Life step")]
    [SerializeField] private GameObject seedStep;
    [SerializeField] private GameObject growStep;
    [SerializeField] private GameObject ripStep;

    [Header("Growing details")]
    [SerializeField] private float seedTime = 5;
    [SerializeField] private float growTime = 5;
    [SerializeField] private float ripTime = 5;
    [SerializeField] private float rottenTime = 5;
    [SerializeField] private Material rottenMat;

    private float stateTime;
    private PlantState state;
    private GameObject model;

    void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ChangeState(PlantState.Seed);
    }

    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case PlantState.Seed:
                {
                    if (Time.time > stateTime + seedTime)
                        ChangeState(PlantState.Grow);

                    break;
                }

            case PlantState.Grow:
                {
                    if (Time.time > stateTime + growTime)
                        ChangeState(PlantState.Rip);

                    break;
                }

            case PlantState.Rip:
                {
                    if (Time.time > stateTime + ripTime)
                        ChangeState(PlantState.Rotten);

                    break;
                }

            case PlantState.Rotten:
                {
                    if (Time.time > stateTime + rottenTime)
                        Destroy(gameObject);

                    break;
                }
        }
    }

    private void ChangeState(PlantState newState)
    {
        state = newState;
        stateTime = Time.time;

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
