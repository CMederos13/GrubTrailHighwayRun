using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Runs the game: singleton, win/lose checks, HUD, sounds and screens
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Road distance and finish line for the highway run
    [Header("Highway")]
    public RoadScroller road;
    public float finishDistance = 500f;
    public bool startOnPlay = true;

    [Header("Gameplay")]
    public PlayerController playerController;
    public List<Enemy> enemyList;

    public int score;

    public TMP_Text text;

    [Header("HUD")]
    public TMP_Text distanceText;

    [Header("Audio Clips")]
    public AudioClip fireGunSound;

    public AudioClip gameOverSound;

    public AudioClip gameplayMusic;

    public AudioClip takingDamageSound;

    public AudioClip zombieHordeSound;

    public AudioClip explosionSound; //Explosion when the truck or an enemy is destroyed

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
    public GameObject gameplay; // parent of truck, controller, road, and zombies
    public GameObject gameplayUI; // Score and Health Bar


    // checks if win/lose while playing
    private bool isGameRunning = false;


    // Makes this the only GameManager and keeps it between scenes
    public void Awake()
    {
        enemyList = new List<Enemy>();

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

    // Starts the music, then starts the game or shows the main menu
    void Start()
    {
        if (musicPlayer != null)
        {
            musicPlayer.clip = gameplayMusic;
            musicPlayer.Play();
        }

        if (startOnPlay)
        {
            DoStartGame();
        }
        else
        {
            DoShowMainMenu();
        }
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
            // Reaching the finish distance wins the game
            else if (road != null && road.distanceTraveled >= finishDistance)
            {
                DoShowVictory();
            }
        }

        if (text != null)
        {
            text.text = "" + score;
        }
        // Shows distance traveled on the HUD
        if (distanceText != null && road != null)
        {
            distanceText.text = "Distance: " + Mathf.FloorToInt(road.distanceTraveled) + " / " + finishDistance;
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

    // Plays a sound effect through the SFX player
    public void PlaySound(AudioClip clip)
    {
        if (sfxPlayer != null && clip != null)
        {
            sfxPlayer.PlayOneShot(clip);
        }
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

    //Turns screen and gameplay off, this on what it needs when the bottons clicked
    private void HideAllScreens()
    {
        SetScreen(mainMenuScreen, false);
        SetScreen(creditsScreen, false);
        SetScreen(settingsScreen, false);
        SetScreen(victoryScreen, false);
        SetScreen(gameOverScreen, false);
        SetScreen(gameplay, false);
        SetScreen(gameplayUI, false);
    }

    public void DoShowMainMenu()
    {
        //Restarts Music if game over stopped it
        if (musicPlayer != null && !musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }

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

        if (musicPlayer != null && !musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }

        score = 0;
        Time.timeScale = 1;
        isGameRunning = true;
    }

    // Player reached the finish: show victory and freeze the game
    public void DoShowVictory()
    {
        ClearEnemies();
        isGameRunning = false;
        Debug.Log("You made it to the finish!");
        HideAllScreens();
        SetScreen(victoryScreen, true);
        Time.timeScale = 0;
    }

    // Truck was destroyed: show game over and freeze the game
    public void DoShowGameOver()
    {
        ClearEnemies();
        isGameRunning = false;
        Debug.Log("Game Over");
        HideAllScreens();
        SetScreen(gameOverScreen, true);
        if (cameraListener != null)
        {
            cameraListener.enabled = true;
        }
        Time.timeScale = 0;
    }
}