using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [Header("Move details")]
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float loopTime = 10;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        if (timer > loopTime)
            ResetLoop();
    }

    private void ResetLoop()
    {
        timer = 0;
        transform.position = new Vector3(-20, transform.position.y, transform.position.z);
    }
}
