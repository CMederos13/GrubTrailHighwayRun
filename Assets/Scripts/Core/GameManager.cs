using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    

    [Header("Player Lives")]
    public GameObject playerShipPreFab; //StarShipPawn prefab, spawn at the world origin
    public int startingLives = 3; // Designers can set the amount of lives the player gets
    private int currentLives;
    public TMP_Text livesText; // Shows lives left in the UI 

    [Header("Player Health UI")]
    public Image playerHealthBar; // Health Bar fill image the ship updates
    public Transform playerDamageNumberParent; // Where the ghost damage numbers pop up

    [Header("Enemy Spawning")]
    public GameObject asteroidPreFab; // Asteroid spawn at the start
    public GameObject ufoPreFab; // UFO enemy 
    public int enemyCount = 5; // Designer set how many enemies spawn
    public float ufoChancePercent = 20; // Chances of (0-100) that each enemy is a UFO Obstacle 
    public float safeSpawnDistance = 3; // keeps enemies from spawning on top of player

    [Header("Screen Bounds")]
    public float minX = -9;
    public float maxX = 9;
    public float minY = -5;
    public float maxY = 5;
    public float warpMargin = 1f; // how far past the bounds objects travel before warping



    [Header("Gameplay")]
    public PlayerController playerController;
    public List<Obstacle> obstacleList;

    public int score;

    public TMP_Text text;

    [Header("Audio Clips")]
    public AudioClip fireGunSound;

    public AudioClip teleportSound;

    public AudioClip gameOverSound;

    public AudioClip backgroundMusic;

    public AudioClip takingDamageSound;

    public AudioClip ufoHumSound;

    public AudioClip deathSound; //Explosion when the ship or an enemy is destroyed

    [Header("Audio Sources")]
    public AudioSource sfxPlayer;

    public AudioSource musicPlayer;

    public AudioListener cameraListener; // Used while in Menus

    [Header("Game Screens")]
    public GameObject mainMenuScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;
    public GameObject victoryScreen;
    public GameObject gameOverScreen;


    [Header("Gameplay Screens")]
    public GameObject gameplay; // parent of ship,controller, and asteroids
    public GameObject gameplayUI; // Score and Health Bar


    // checks if win/lose while playing
    private bool isGameRunning = false;


    public void Awake()
    {
        obstacleList = new List<Obstacle>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void Start()
    {
        musicPlayer.clip = backgroundMusic;
        musicPlayer.Play();

        DoShowMainMenu();
    }
    void Update()
    {

        //checking for win/lose while game is running
        if (isGameRunning)
        {
            if (playerController != null && playerController.pawn == null)
            {
                PlayerDied();
            }
            else if (obstacleList.Count <= 0)
            {
                DoShowVictory();
            }

        }

        if (text != null)
        {
            text.text = "" + score;
        }
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    private void PlayerDied()
    {
        currentLives = currentLives - 1;

        if (currentLives > 0)
        {
            SpawnPlayer();
        }
        else
        {
            //Last Life Lost: Stops music so Game Over sound is Heard
            musicPlayer.Stop();
            sfxPlayer.PlayOneShot(gameOverSound);
            DoShowGameOver();
        }
    }
    //Creates a new ship at the world origin and connects it to the Controller and Health UI
    private void SpawnPlayer()
    {
        // Removes the old ship if one is still in the scene 
        if (playerController.pawn != null)
        {
            Destroy(playerController.pawn.gameObject);
        }
        GameObject newShip = Instantiate(playerShipPreFab, Vector3.zero, Quaternion.identity, gameplay.transform);

        // Hands over the new ship to the player controller
        playerController.pawn = newShip.GetComponent<Pawn>();

        //Connect the new ship's health to the health bar UI
        Health shipHealth = newShip.GetComponent<Health>();
        shipHealth.healthBar = playerHealthBar;
        shipHealth.damageNumberParent = playerDamageNumberParent;


        // The new ship's audio listener takes over from the camera 
        cameraListener.enabled = false;

    }
    private void SpawnEnemies()
    {
        // Clears old enemies and spawns new ones at random positions
        for (int i = obstacleList.Count - 1; i >= 0; i--)
        {
            Destroy(obstacleList[i].gameObject);
        }
        obstacleList.Clear();

        for (int i = 0; i < enemyCount; i++)
        {
            // Picks a random spot and chooses again if obstacle is to close
          Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
          while (Vector3.Distance(spawnPosition, Vector3.zero) < safeSpawnDistance)
            {
                spawnPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
            }
            // Roll the UFO chance; Spawns an Asteroid if there no UFO or the roll fails 
            GameObject prefabToSpawn = asteroidPreFab;

            if (ufoPreFab != null && Random.Range(0f, 100f) < ufoChancePercent)
            {
                prefabToSpawn = ufoPreFab;
            }

            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity, gameplay.transform);
        }
    }
                

    //Turns screen and gameplay off, this on what it needs when the bottons clicked
    
    private void HideAllScreens()
    {
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(false);
        settingsScreen.SetActive(false);
        victoryScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameplay.SetActive(false);
        gameplayUI.SetActive(false);

    }
    public void DoShowMainMenu()
    {
        //Restarts Music if game over stopped it
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }

        HideAllScreens();
        mainMenuScreen.SetActive(true);
        cameraListener.enabled = true;
        isGameRunning = false;
        Time.timeScale = 1;
    }

    public void DoShowCredits()
    {
        HideAllScreens();
        creditsScreen.SetActive(true);
    }
    public void DoShowSettings()
    {
        HideAllScreens();
        settingsScreen.SetActive(true);
    }

    public void DoStartGame()
    {
        HideAllScreens();
        gameplay.SetActive(true);
        SpawnEnemies();
        gameplayUI.SetActive(true);
        
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }
        //Reset Run: Score 0, full lives, new ship at the origin
       
        score = 0;
        currentLives = startingLives;
        SpawnPlayer();
        Time.timeScale = 1;
        isGameRunning = true;
    }
    public void DoShowVictory()
    {
        isGameRunning = false;
        HideAllScreens();
        victoryScreen.SetActive(true);
        cameraListener.enabled = true;
    }

    public void DoShowGameOver()
    {
        isGameRunning = false;
        HideAllScreens();
        gameOverScreen.SetActive(true);
        cameraListener.enabled = true;
    }
}

