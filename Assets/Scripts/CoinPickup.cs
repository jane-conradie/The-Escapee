using System;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForGoldCoinPickup = 100;
    [SerializeField] int pointsForBronzeCoinPickup = 50;

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

        if (gameObject.tag == "Coin - Gold")
        {
            // 100 points
            scoreToAdd += pointsForGoldCoinPickup;
        }
        else if (gameObject.tag == "Coin - Bronze")
        {
            // 50 points
            scoreToAdd += pointsForBronzeCoinPickup;
        }

        // send to UI
        FindObjectOfType<GameSession>().ProcessScoreIncrease(scoreToAdd);

        wasCollected = true;
    }
}
