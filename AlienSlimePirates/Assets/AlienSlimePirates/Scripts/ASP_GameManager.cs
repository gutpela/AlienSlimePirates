﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* 
[System.Serializable]
public struct SceneLoadInfo
{

   public string SceneName;
   public string NextSceneName;
   public string TransitionSceneName;
   public bool UsesTransitionScene;
   public float transitionSceneDelay;
}
*/
//Enum holding all possible application statuses of the application
public enum GameStatus
{
    undetermined,
    menu,
    instructions,
    preLevel,
    LevelinProgress,
    postLevel,
    transitionToNewLevel,
    paused

}


public enum GameResult
{
    undetermined,
    PlayerWin,
    PlayerLossCore,
    PlayerLossDeath
}

//game manager singleton isnstance
public class ASP_GameManager : MonoBehaviour

{
    //Total enemies needing to be eliminated to finish level
    private int totalEnemiesInLevel = 0;
    //Total enemies eliminated towards satisfying level completion
    private int totalEnemiesEliminated = 0;
    //what is currently the statsu of the application
    [SerializeField]
    private GameStatus gameStatus = GameStatus.undetermined;
    [SerializeField]
    private GameResult gameResult = GameResult.undetermined;
    //score in current round/level
    [SerializeField]
    private int currentScore = 0;

    //cumulative score in all round/levels played in current session
    [SerializeField]
    private int cumulativeScore = 0;
    //high score out of  all round/levels played in current session
    [SerializeField]
    private int sessionHighScore = 0;
    //UI field allowing output of current score
    [SerializeField]
    private ASP_Game_HUD gameHUD;

	[SerializeField]
	private ASP_Player_HUD playerHUD;
    // private bool currentLevelOver = false;
    [SerializeField]
    private List<ASP_ProximityDamage> ProximityDamageObjects;
    [SerializeField]
    private List<ASP_Damageable> ProximityDamageableObjects;
    public static ASP_GameManager Instance { get; private set; }

    /* 
     //properties used for scene Loading
     public SceneLoadInfo[] sceneLoadInfo;
     public AsyncOperation status;
     private float startTime;
     public SceneLoadInfo currentSceneLoadInfo;
     public bool newSceneLoaded = false;
     public float tempID;
     */
    void Awake()
    {
        //tempID = Random.value;
        // First we check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        else
        {
            // Here we save our singleton instance
            Instance = this;
            // Furthermore we make sure thst the GameManager persists in new scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetLevel();
    }

    public void IncrementScore(int addToScore)
    {

        if (gameStatus == GameStatus.LevelinProgress)
        {
            currentScore += addToScore;
            if (gameHUD != null)
            {
                gameHUD.SetNewScore(currentScore.ToString());
            }
        }
    }

    public void GameOver(GameResult result)
    {

        if (gameStatus == GameStatus.LevelinProgress)
        {
            gameResult = result;
            gameStatus = GameStatus.postLevel;
            gameHUD.DisplayFinalResults(gameResult, currentScore.ToString());
           Time.timeScale = 0;
        }

    }


	public void CoreDamaged(float damagePercent)
	{
		//print ("PlayerDamaged " + damagePercent);
		playerHUD.CoreDamaged(damagePercent);
	}
	public void PlayerDamaged(float damagePercent)
	{
		//print ("PlayerDamaged " + damagePercent);
		playerHUD.PlayerDamaged(damagePercent);
	}
    public void RegisterLevelEnemies(int newEnemies)
    {
        totalEnemiesInLevel += newEnemies;
    }
    public void RegisterEnemyDeath()
    {
        totalEnemiesEliminated++;
        if (totalEnemiesEliminated >= totalEnemiesInLevel)
        {
            GameOver(GameResult.PlayerWin);
        }
    }
    public void GameBegin()
    {
        gameStatus = GameStatus.LevelinProgress;
    }



    public void RegisterNewProximityDamageObject(ASP_ProximityDamage newObject)
    {
        ProximityDamageObjects.Add(newObject);
    }


    public void RegisterNewProximityDamagableObject(ASP_Damageable newObject)
    {
        ProximityDamageableObjects.Add(newObject);
    }



    public void UnRegisterNewProximityDamageObject(ASP_ProximityDamage removeObject)
    {

        ProximityDamageObjects.Remove(removeObject);

    }


    public void UnRegisterNewProximityDamagableObject(ASP_Damageable removeObject)
    {

        ProximityDamageableObjects.Remove(removeObject);

    }

    public ASP_ProximityDamage[] GetProximityDamageObjects()
    {
        return ProximityDamageObjects.ToArray();
    }

    private void ResetLevel()
    {
        ProximityDamageObjects = new List<ASP_ProximityDamage>();
        ProximityDamageableObjects = new List<ASP_Damageable>();
    }






























    /* 
     * Scene Loading logic disabled
    void OnEnable()
    {

         SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        newSceneLoaded = true;
        startTime = Time.time;

    }




    void StartNewLoad()
    {
        status = SceneManager.LoadSceneAsync(GetNextSceneName());
        status.allowSceneActivation = false;
        newSceneLoaded = false;
    }

    public void AllowSceneActivation()
    {
        status.allowSceneActivation = true;
    }

    void SetSceneLoadInfo()
    {
        //must find something for parse to succeed
        //  SceneLoadInfo myScene = sceneLoadInfo[0];
        //if current scene is also active loaded scene
        for (int i = 0; i < sceneLoadInfo.Length; i++)
        {
            if (sceneLoadInfo[i].SceneName == SceneManager.GetActiveScene().name)
            {
                currentSceneLoadInfo = sceneLoadInfo[i];
            }
        }

    }

    string GetNextSceneName()
    {
        if (SceneManager.GetActiveScene().name == currentSceneLoadInfo.SceneName)
        {

            //In a primary scene, load transition or next primary
            if (currentSceneLoadInfo.UsesTransitionScene)
            {
                //load transition scene
                return currentSceneLoadInfo.TransitionSceneName;
            }
            else
            {
                //load next scene
                return currentSceneLoadInfo.NextSceneName;
            }
        }
        else
        {
            //In a transition scene
            return currentSceneLoadInfo.NextSceneName;
        }
    }



    void Update()
    {

        if (newSceneLoaded)
        {

            StartNewLoad();
        }


        //  print("status.progress " + status.progress);

        if (currentSceneLoadInfo.SceneName != SceneManager.GetActiveScene().name && status.progress >= 0.9f)
        {
            //in a transition scene and ready to go

            //print("checking time");
            if (startTime + currentSceneLoadInfo.transitionSceneDelay < Time.time)
            {
                status.allowSceneActivation = true;
            }
        }

    }
    */

}
