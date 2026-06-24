using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { SpeedBoost, Shield, DoubleJump }
    public PowerUpType powerUpType;
    public float duration = 5f;
    public float value = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                player.EnableSpeedBoost(value, duration);
                break;
            case PowerUpType.Shield:
                player.EnableShield(duration);
                break;
            case PowerUpType.DoubleJump:
                player.EnableDoubleJump(duration);
                break;
        }

        Destroy(gameObject);
    }
}
