using System;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            ProcessCoinPickup();
        }
    }

    void ProcessCoinPickup()
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);

        // add score to ui
        CalculateAndAddScore();

        Destroy(gameObject);
    }

    void CalculateAndAddScore()
    {
        int scoreToAdd = 0;
        scoreToAdd += pointsForCoinPickup;

        // send to UI
        FindObjectOfType<GameSession>().ProcessScoreIncrease(scoreToAdd);

        wasCollected = true;
    }
}
