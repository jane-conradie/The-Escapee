using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ProcessCoinPickup();
        }
    }

    void ProcessCoinPickup()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

        Destroy(gameObject);
    }
}
