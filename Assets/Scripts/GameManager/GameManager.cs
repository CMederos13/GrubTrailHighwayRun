using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Runs the game: singleton, win/lose checks, HUD, sounds and screens
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private static bool playOnLoad = false;
    private static bool startScreenShown = false;

    // Road distance and finish line for the highway run
    [Header("Highway")]
    public RoadScroller road;
    public float finishDistance = 500f;
    public bool startOnPlay = false;

    [Header("Gameplay")]
    public PlayerController playerController;
    public List<Enemy> enemyList;
    public int score;

    [Header("Lives")]
    public int startingLives = 3; // Designer can set the amount of lives the player gets
    public Transform livesContainer;
    public GameObject lifeIconPrefab;
    private int currentLives;

    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text distanceText;
    public TMP_Text ammoText;

    [Header("End Screen Text")]
    public TMP_Text gameOverScoreText;
    public TMP_Text victoryScoreText;

    [Header("Audio Clips")]
    public AudioClip fireGunSound;
    public AudioClip gameOverSound;
    public AudioClip gameplayMusic;
    public AudioClip menuMusic;
    public AudioClip takingDamageSound;
    public AudioClip zombieHordeSound;
    public AudioClip explosionSound; //Explosion when the truck or an enemy is destroyed

    [Header("Audio Sources")]
    public AudioSource sfxPlayer;
    public AudioSource musicPlayer;
    public AudioListener cameraListener; // Used while in Menus

    [Header("Game Screens")]
    public GameObject startScreen;
    public GameObject mainMenuScreen;
    public GameObject creditsScreen;
    public GameObject settingsScreen;
    public GameObject victoryScreen;
    public GameObject gameOverScreen;

    [Header("Gameplay Screens")]
    public GameObject gameplay; // parent of truck, controller, road, and zombies
    public GameObject gameplayUI; // Score and Health Bar

    // checks if win/lose while playing
    private bool isGameRunning = false;

    // Makes this the only GameManager
    public void Awake()
    {
        enemyList = new List<Enemy>();

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (startOnPlay || playOnLoad)
        {
            playOnLoad = false;
            DoStartGame();
        }
        else if (startScreenShown)
        {
            DoShowMainMenu();
        }
        else
        {
            DoShowStartScreen();
        }
    }

    void Update()
    {
        if (startScreen != null && startScreen.activeSelf && Input.anyKeyDown)
        {
            DoShowMainMenu();
        }

        //checking for win/lose while game is running
        if (isGameRunning)
        {
            if (playerController != null && playerController.pawn == null)
            {
                PlayerDied();
            }
            // Reaching the finish distance wins the game
            else if (road != null && road.distanceTraveled >= finishDistance)
            {
                DoShowVictory();
            }
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        // Shows distance traveled on the HUD
        if (distanceText != null && road != null)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(road.distanceTraveled) + " / " + finishDistance;
        }

        if (ammoText != null && playerController != null && playerController.pawn != null)
        {
            ShooterBullet shooter = playerController.pawn.GetComponent<ShooterBullet>();
            if (shooter != null)
            {
                ammoText.text = "Ammo: " + shooter.currentAmmo;
            }
        }
    }

    private void PlayerDied()
    {
        //Stops music so Game Over sound is Heard
        if (musicPlayer != null)
        {
            musicPlayer.Stop();
        }
        PlaySound(gameOverSound);
        DoShowGameOver();
    }

    public bool LoseLife()
    {
        currentLives = currentLives - 1;
        UpdateLivesUI();
        ClearEnemies();
        return currentLives > 0;
    }

    private void UpdateLivesUI()
    {
        if (livesContainer == null || lifeIconPrefab == null)
        {
            return;
        }

        foreach (Transform icon in livesContainer)
        {
            Destroy(icon.gameObject);
        }

        for (int i = 0; i < currentLives; i++)
        {
            Instantiate(lifeIconPrefab, livesContainer);
        }
    }

    public float GetDifficulty()
    {
        if (road == null)
        {
            return 1f;
        }
        return 1f + road.distanceTraveled / finishDistance;
    }

    // Plays a sound effect through the SFX player
    public void PlaySound(AudioClip clip)
    {
        if (sfxPlayer != null && clip != null)
        {
            sfxPlayer.PlayOneShot(clip);
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicPlayer == null || clip == null)
        {
            return;
        }
        if (musicPlayer.clip == clip && musicPlayer.isPlaying)
        {
            return;
        }
        musicPlayer.clip = clip;
        musicPlayer.Play();
    }

    // Turns a screen on or off, skipping it if it isn't set up yet
    private void SetScreen(GameObject screen, bool isOn)
    {
        if (screen != null)
        {
            screen.SetActive(isOn);
        }
    }

    private void ClearEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }

    private int SaveHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        return highScore;
    }

    //Turns screen and gameplay off, this on what it needs when the bottons clicked
    private void HideAllScreens()
    {
        SetScreen(startScreen, false);
        SetScreen(mainMenuScreen, false);
        SetScreen(creditsScreen, false);
        SetScreen(settingsScreen, false);
        SetScreen(victoryScreen, false);
        SetScreen(gameOverScreen, false);
        SetScreen(gameplay, false);
        SetScreen(gameplayUI, false);
    }

    public void DoShowStartScreen()
    {
        HideAllScreens();
        SetScreen(startScreen, true);
        PlayMusic(menuMusic);
        isGameRunning = false;
        Time.timeScale = 1;
    }

    public void DoShowMainMenu()
    {
        startScreenShown = true;
        PlayMusic(menuMusic);
        HideAllScreens();
        SetScreen(mainMenuScreen, true);
        if (cameraListener != null)
        {
            cameraListener.enabled = true;
        }
        isGameRunning = false;
        Time.timeScale = 1;
    }

    public void DoShowCredits()
    {
        HideAllScreens();
        SetScreen(creditsScreen, true);
    }

    public void DoShowSettings()
    {
        HideAllScreens();
        SetScreen(settingsScreen, true);
    }

    // Shows gameplay, resets the score and starts the run
    public void DoStartGame()
    {
        HideAllScreens();
        SetScreen(gameplay, true);
        SetScreen(gameplayUI, true);
        PlayMusic(gameplayMusic);

        score = 0;
        currentLives = startingLives;
        UpdateLivesUI();
        Time.timeScale = 1;
        isGameRunning = true;
    }

    // Player reached the finish: show victory and freeze the game
    public void DoShowVictory()
    {
        ClearEnemies();
        isGameRunning = false;
        HideAllScreens();
        SetScreen(victoryScreen, true);
        int highScore = SaveHighScore();
        if (victoryScoreText != null)
        {
            victoryScoreText.text = "Score: " + score + "\nHigh Score: " + highScore;
        }
        Time.timeScale = 0;
    }

    // Truck was destroyed: show game over and freeze the game
    public void DoShowGameOver()
    {
        ClearEnemies();
        isGameRunning = false;
        HideAllScreens();
        SetScreen(gameOverScreen, true);
        int highScore = SaveHighScore();
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + score + "\nHigh Score: " + highScore;
        }
        if (cameraListener != null)
        {
            cameraListener.enabled = true;
        }
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        playOnLoad = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        playOnLoad = false;
        startScreenShown = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}