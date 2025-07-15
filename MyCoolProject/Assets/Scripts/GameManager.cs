using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Variables")]
    public PlayerController player;
    public float time;
    public bool timeActive;

    [Header("Game UI")]
    public TMP_Text gameUI_score;
    public TMP_Text gameUI_health;
    public TMP_Text gameUI_time;

    [Header("Countdown UI")]
    public TMP_Text countdownText;
    public int countdown;

    [Header("Screens")]
    public GameObject countdownUI;
    public GameObject gameUI;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

        // make sure the timer is set to 0
        time = 0;

        // disable player movement initially
        player.enabled = false;

        // set screen to the countdown
        SetScreen(countdownUI);

        // start coroutine
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        countdownText.gameObject.SetActive(true);
        countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        //enable player movement
        player.enabled = true;

        //start the game
        startGame();
    }

    void startGame()
    {
        // set the screen to see your stats
        SetScreen(gameUI);

        // start the timer
        timeActive = true;
    }

    public void endGame()
    {
        // end the timer
        timeActive = false;

        // disable player movement
        player.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // keep track of the time that goes by
        if(timeActive)
        {
            time = time + Time.deltaTime;
        }

        // set the UI to display stats
        gameUI_score.text = "Coins: " + player.coinCount;
        gameUI_health.text = "Health: " + player.health;
        gameUI_time.text = "Time: " + (time * 10).ToString("F2");
    }

    public void SetScreen(GameObject screen)
    {
        //disable all other screens
        gameUI.SetActive(false);
        countdownUI.SetActive(false);

        //activate the requested screen
        screen.SetActive(true);
    }
}
