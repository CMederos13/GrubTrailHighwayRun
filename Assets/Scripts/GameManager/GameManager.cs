using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Runs the game: singleton, win/lose checks, HUD, sounds and screens
public class GameManager : MonoBehaviour
{
    // Singleton: the one shared GameManager any script can reach with GameManager.instance
    public static GameManager instance;

    // Static so they keep their value when the scene reloads (Play Again / Main Menu)
    private static bool playOnLoad = false;
    private static bool startScreenShown = false;

    // Road distance and finish line for the highway run
    [Header("Highway")]
    public RoadScroller road;
    public float finishDistance = 500f;
    public bool startOnPlay = false;

    // Who is playing, the zombies alive right now (List) and the score
    [Header("Gameplay")]
    public PlayerController playerController;
    public List<Enemy> enemyList;
    public int score;

    [Header("Lives")]
    public int startingLives = 3; // Designer can set the amount of lives the player gets
    public Transform livesContainer;
    public GameObject lifeIconPrefab;
    private int currentLives;

    // Texts and the progress bar on the gameplay HUD
    [Header("HUD")]
    public TMP_Text scoreText;
    public TMP_Text highScoreText;
    public TMP_Text distanceText;
    public TMP_Text ammoText;
    public Image progressFill;

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

    // Decides what to show first: jump straight into a run, the main menu, or the start screen
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

    // Every frame: start screen key press, win/lose checks and updating the HUD
    void Update()
    {
        // Pressing any key on the start screen opens the main menu
        if (startScreen != null && startScreen.activeSelf && Input.anyKeyDown)
        {
            // Plays the same click sound as the menu buttons
            AudioManager audioManager = GetComponent<AudioManager>();
            if (audioManager != null)
            {
                audioManager.Play("UIClick");
            }
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

        // Shows the current score
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        // Shows the best score ever: the saved high score, or this run's score if it is higher
        if (highScoreText != null)
        {
            int bestScore = Mathf.Max(score, PlayerPrefs.GetInt("HighScore", 0));
            highScoreText.text = "Best: " + bestScore;
        }

        // Shows distance traveled on the HUD
        if (distanceText != null && road != null)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(road.distanceTraveled) + " / " + finishDistance;
        }

        // Fills the progress bar: distance / finish distance gives 0 (start) to 1 (finish)
        if (progressFill != null && road != null)
        {
            progressFill.fillAmount = road.distanceTraveled / finishDistance;
        }

        // Shows how much ammo the truck's gun has left
        if (ammoText != null && playerController != null && playerController.pawn != null)
        {
            ShooterBullet shooter = playerController.pawn.GetComponent<ShooterBullet>();
            if (shooter != null)
            {
                ammoText.text = "Ammo: " + shooter.currentAmmo;
            }
        }
    }

    // Truck is gone with no lives left: stop the music and show Game Over
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

    // Takes away one life, updates the medkit icons, clears the zombies
    // Returns true if the player still has lives left
    public bool LoseLife()
    {
        currentLives = currentLives - 1;
        UpdateLivesUI();
        ClearEnemies();
        return currentLives > 0;
    }

    // Redraws the medkit icons so there is one for each life left
    private void UpdateLivesUI()
    {
        if (livesContainer == null || lifeIconPrefab == null)
        {
            return;
        }

        // foreach: removes every old icon
        foreach (Transform icon in livesContainer)
        {
            Destroy(icon.gameObject);
        }

        // for loop: makes one new icon per life
        for (int i = 0; i < currentLives; i++)
        {
            Instantiate(lifeIconPrefab, livesContainer);
        }
    }

    // Difficulty goes from 1 at the start to 2 at the finish
    // Spawners divide their wait time by this, so things spawn faster near the end
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

    // Plays a music track on loop, unless that track is already playing
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

    // foreach: destroys every zombie in the enemy list, then empties the list
    private void ClearEnemies()
    {
        foreach (Enemy enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        enemyList.Clear();
    }

    // PlayerPrefs: saves the score if it beats the high score, so it is remembered after the game closes
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

    // Screen methods below are called by Start/Update and by the menu buttons' On Click
    public void DoShowStartScreen()
    {
        HideAllScreens();
        SetScreen(startScreen, true);
        PlayMusic(menuMusic);
        isGameRunning = false;
        Time.timeScale = 1;
    }

    // Shows the main menu with the menu music
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

    // Shows the credits
    public void DoShowCredits()
    {
        HideAllScreens();
        SetScreen(creditsScreen, true);
    }

    // Shows the settings (volume sliders)
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

    // Play Again button: reloads the scene and starts a new run right away
    public void PlayAgain()
    {
        playOnLoad = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Main Menu button: reloads the scene and opens the main menu (skips the start screen)
    public void BackToMainMenu()
    {
        playOnLoad = false;
        startScreenShown = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}