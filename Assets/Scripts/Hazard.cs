using UnityEngine;

public class Hazard : MonoBehaviour
{
    public int damage = 1;
    public float knockbackStrength = 4f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage, knockbackStrength, transform.position);
        }
        else if (GameManager.Instance != null)
        {
            GameManager.Instance.Damage(damage);
        }
    }
}
