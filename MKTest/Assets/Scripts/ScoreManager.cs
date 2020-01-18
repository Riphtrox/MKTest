using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI text;
    public int score;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ScoreUpdate(int coins)
    {

        //Calculate and display score
        score += coins;
        text.text = "Score: " + score.ToString();
    }

    //This is used to get the score by the GameManager
    public int GetScore()
    {
        return score;
    }

}
