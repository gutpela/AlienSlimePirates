﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ASP_Game_HUD : MonoBehaviour
{

    [SerializeField]
    private Text currentScoreLabel;
    [SerializeField]
    private Text currentScoreOutput;
    [SerializeField]
    private Text gameOver;
    [SerializeField]
    private Text victory;

    [SerializeField]
    private Text defeat;
    [SerializeField]
    private Text gameResult;
    [SerializeField]
    private Text FinalScoreLabel;
    [SerializeField]
    private Text FinalScoreOutput;
    // Use this for initialization
    void Start()
    {
        currentScoreLabel.enabled = true;
        currentScoreOutput.enabled = true;
        gameOver.enabled = false;
        victory.enabled = false;
        defeat.enabled = false;
        gameResult.enabled = false;
        FinalScoreLabel.enabled = false;
        FinalScoreOutput.enabled = false;
    }


    public void SetNewScore(string newScore)
    {
        currentScoreOutput.text = newScore;
    }

    public void DisplayFinalResults(GameResult result, string finalScore)
    {

		print ("*******************************FOO *************************** " + finalScore);
        currentScoreLabel.enabled = false;
        currentScoreOutput.enabled = false;

        gameOver.enabled = true;

        if (result == GameResult.PlayerWin)
        {
            victory.enabled = true;
            gameResult.text = "You defeated the\nAlien Slime Pirates!";
        }
        else if (result == GameResult.PlayerLossCore)
        {
            defeat.enabled = true;
            gameResult.text = "The Alien Slime Pirates\ndestroyed the core!";

        }
        else if (result == GameResult.PlayerLossDeath)
        {
            defeat.enabled = true;
            gameResult.text = "The Alien Slime Pirates\nhave slain you!";
        }

        gameResult.enabled = true;
        FinalScoreLabel.enabled = true;
        FinalScoreOutput.enabled = true;
        FinalScoreOutput.text = finalScore;
    }



}
