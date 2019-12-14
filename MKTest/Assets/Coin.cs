using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinWorth = 1;       //How much the coin is worth
    private bool counted = false;   //Ensuring the score count is a one-shot

    private void OnTriggerEnter2D(Collider2D other)
    {

        //Check if it is the player and the coin has not been counted
        if (other.gameObject.CompareTag("Player") && !counted)
        {
            ScoreManager.instance.ScoreUpdate(coinWorth);
            counted = true;     //Score has been counted, don't count it again
        }
    }
}
