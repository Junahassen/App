using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    public float speed = 2f;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private bool movingToEnd;

    private void Awake()
    {
        startPoint = transform.position;
        endPoint = startPoint + offset;
    }

    private void Update()
    {
        Vector3 target = movingToEnd ? endPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            movingToEnd = !movingToEnd;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
