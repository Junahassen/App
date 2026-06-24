using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int value = 1;
    public float rotationSpeed = 90f;

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    public void Collect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(value);
        }

        Destroy(gameObject);
    }
}
