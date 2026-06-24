using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3.5f;
    public float chaseRange = 6f;
    public int damage = 1;

    private Transform player;
    private bool movingToB;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (pointA == null)
            pointA = transform;

        if (pointB == null)
            pointB = transform;
    }

    private void Update()
    {
        Vector3 target = movingToB ? pointB.position : pointA.position;

        if (player != null && Vector3.Distance(transform.position, player.position) < chaseRange)
        {
            target = player.position;
            transform.position = Vector3.MoveTowards(transform.position, target, chaseSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                movingToB = !movingToB;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(damage, 5f, transform.position);
        }
    }
}
