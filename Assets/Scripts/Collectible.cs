using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Transform target;
    private Vector3 targetOfffsetPosition;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 1;

    [Header("Collectible ressource")]
    [SerializeField] private GameObject plant;

    void Awake()
    {
        Instantiate(plant, transform);
    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position + targetOfffsetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position + targetOfffsetPosition) < 0.001f)
                Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target, Vector3 offset = default)
    {
        this.target = target;
        this.targetOfffsetPosition = offset;
    }
}
