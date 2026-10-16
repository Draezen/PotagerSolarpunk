using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 1;

    [Header("Collectible ressource")]
    [SerializeField] private GameObject plantPrefab;

    private Transform target;
    private Vector3 targetOfffsetPosition;

    private void Awake()
    {
        Instantiate(plantPrefab, transform);
    }

    private void Update()
    {
        if (target)
        {
            Vector3 targetPosition = target.position + targetOfffsetPosition;
            Vector3 direction = targetPosition - transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
                Destroy(gameObject);
        }
    }

    public void SetTarget(Transform target, Vector3 offset = default)
    {
        this.target = target;
        targetOfffsetPosition = offset;
    }
}
