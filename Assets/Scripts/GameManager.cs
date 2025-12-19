using UnityEngine;
using TMPro;                
using UnityEngine.SceneManagement; 
using UnityEngine.Audio;    
using UnityEngine.UI;       

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // --- PÔVODNÉ PREMENNÉ PRE HRU ---
    [Header("HUD - In-Game UI")]
    public TextMeshProUGUI scoreText; 

    [Header("Win & Lose UI Panely")]
    public GameObject winPanel;       
    public TextMeshProUGUI winScoreText; 
    public GameObject losePanel;      
    public TextMeshProUGUI loseScoreText; 

    [Header("Nastavenia Hry")]
    public int winningScore = 5000;
    public float basePointsPerSecond = 1f;

    [Header("Stav Hry")]
    public float currentScore = 0;
    public float scoreMultiplier = 1f;
    public float timeSinceLastBadHit = 0f;
    public bool isGameActive = true;

    // NOVÁ PREMENNÁ - aby sa win panel neukazoval stále dokola po pokračovaní
    private bool hasWon = false; 

    // --- PREMENNÉ PRE PAUZU A AUDIO ---
    [Header("Pause Menu Systém")]
    public GameObject pauseMenuUI; 
    [Header("Audio")]
    public AudioMixer audioMixer;
    
  
    public AudioSource backgroundMusic; 

    private bool isPaused = false;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        // Uistíme sa, že HUD text je viditeľný
        if (scoreText != null) scoreText.gameObject.SetActive(true);

        Time.timeScale = 1f;
        isGameActive = true;
        hasWon = false; // Resetujeme stav výhry
    }

    void Update()
    {
        // 1. KONTROLA PAUZY (ESC)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameActive)
            {
                if (isPaused) ResumeGame();
                else PauseGame();
            }
        }

        // Ak hra nebeží (GameOver) alebo je pauza, končíme Update
        if (!isGameActive || isPaused) return;


        // 2. LOGIKA ČASU A NÁSOBIČA
        timeSinceLastBadHit += Time.deltaTime;

        if (timeSinceLastBadHit < 20f) scoreMultiplier = 1f;
        else if (timeSinceLastBadHit >= 20f && timeSinceLastBadHit < 40f) scoreMultiplier = 10f;
        else if (timeSinceLastBadHit >= 40f) scoreMultiplier = 100f;

        // 3. PRIDÁVANIE BODOV
        AddScore(basePointsPerSecond * scoreMultiplier * Time.deltaTime);

        // 4. AKTUALIZÁCIA HUD TEXTU
        UpdateScoreUI();
    }

    public void AddScore(float amount)
    {
        if (!isGameActive) return;

        currentScore += amount;
        if (currentScore < 0) currentScore = 0;

        // KONTROLA VÝHRY
        // Pridal som podmienku && !hasWon, aby sa Win nespustil, ak si už vyhral a pokračuješ
        if (currentScore >= winningScore && !hasWon)
        {
            WinGame();
        }
    }

    public void ResetMultiplier()
    {
        timeSinceLastBadHit = 0f;
        scoreMultiplier = 1f;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Odstránil som SetActive(true), lebo ho zapíname v Resume/Continue funkciách
            scoreText.text = "Body: " + (int)currentScore + "\nNásobič: x" + scoreMultiplier;
        }
    }

    // --- WIN / LOSE ---

    public void WinGame()
    {
        if (!isGameActive) return; 
        
        isGameActive = false; // Pozastaví počítanie bodov kým je panel otvorený
        hasWon = true;        // Zapamätáme si, že hráč už dosiahol cieľ
        
        if (winPanel != null) 
        {
            winPanel.SetActive(true);
            if (winScoreText != null) 
                winScoreText.text +=(int)currentScore;
        }

        if (scoreText != null) scoreText.gameObject.SetActive(false); // Skryje HUD
        if (backgroundMusic != null) backgroundMusic.Stop();
        Time.timeScale = 0; // Zastaví hru
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- NOVÁ FUNKCIA: POKRAČOVAŤ V HRE PO VÝHRE ---
    // Toto priraď na tlačidlo "Pokračovať" v tvojom Win Paneli
    public void ContinuePlaying()
    {
        // Skryjeme Win Panel
        if (winPanel != null) winPanel.SetActive(false);

        // Znova zobrazíme HUD skóre
        if (scoreText != null) scoreText.gameObject.SetActive(true);
        if (backgroundMusic != null) backgroundMusic.Play();
        // Pustíme čas a logiku
        Time.timeScale = 1f;
        isGameActive = true; 
        
        // Poznámka: Premenná 'hasWon' je teraz true, takže WinGame() sa už znova nezavolá
    }

    public void GameOver()
    {
        if (!isGameActive) return;
        isGameActive = false;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
            if (loseScoreText != null)
                loseScoreText.text +=(int)currentScore;
        }

        if (scoreText != null) scoreText.gameObject.SetActive(false);
        if (backgroundMusic != null) backgroundMusic.Stop();
        Time.timeScale = 0; 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // --- BUTTONS ---

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (backgroundMusic != null) backgroundMusic.Play();
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Koniec hry, žiadny ďalší level. Idem do menu.");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // --- PAUZA ---

    public void PauseGame()
    {
        isPaused = true;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (scoreText != null) scoreText.gameObject.SetActive(false);
        Time.timeScale = 0f; 
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (scoreText != null) scoreText.gameObject.SetActive(true);
        if (isGameActive)
        {
            Time.timeScale = 1f;
        }
    }

    // --- AUDIO ---
    public void SetMusicVolume(float volume)
    {
        if (volume <= 0.0001f) volume = 0.0001f;
        if (audioMixer != null) audioMixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        if (volume <= 0.0001f) volume = 0.0001f;
        if (audioMixer != null) audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
}