using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject scoreOverlay; //The overlay showing the score during gameplay
    public GameObject gameOverMenu; //The menu at game over
    public TextMeshProUGUI text;    //The text to display score
    public AudioSource deathSound;  //Audio that plays when the player dies
    bool gameIsOver = false;        //Check if GameOver has already been called
    int score;                      //Saving the score


    private void Awake()
    {
        //Ensuring the correct overlays are displaying
        scoreOverlay.SetActive(true);
        gameOverMenu.SetActive(false);
    }

    public void GameOver()
    {
        if (!gameIsOver)
        {
            gameIsOver = true;

            deathSound.Play();

            //Pause the game
            Time.timeScale = 0;

            //Get and display the score value
            score = FindObjectOfType<ScoreManager>().GetScore();
            text.text = "Your Score: " + score.ToString();

            //Swap to the game over menu
            scoreOverlay.SetActive(false);
            gameOverMenu.SetActive(true);
        }
    }
}
