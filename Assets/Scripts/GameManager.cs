using UnityEngine;
using TMPro; // Dôležité pre prácu s novým textom

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI - Sem pretiahni Text")]
    public TextMeshProUGUI scoreText; // Odkaz na textové pole

    [Header("Nastavenia Hry")]
    public int winningScore = 5000;
    public float basePointsPerSecond = 1f;

    [Header("Stav Hry")]
    public float currentScore = 0;
    public float scoreMultiplier = 1f;
    public float timeSinceLastBadHit = 0f;
    public bool isGameActive = true;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (!isGameActive) return;

        // 1. Logika času a násobiča
        timeSinceLastBadHit += Time.deltaTime;

        if (timeSinceLastBadHit < 20f)
        {
            scoreMultiplier = 1f;
        }
        else if (timeSinceLastBadHit >= 20f && timeSinceLastBadHit < 40f)
        {
            scoreMultiplier = 10f;
        }
        else if (timeSinceLastBadHit >= 40f)
        {
            scoreMultiplier = 100f;
        }

        // 2. Pridávanie bodov za čas
        AddScore(basePointsPerSecond * scoreMultiplier * Time.deltaTime);

        // 3. Aktualizácia Textu na obrazovke
        UpdateScoreUI();
    }

    public void AddScore(float amount)
    {
        currentScore += amount;

        // Ak by sme náhodou išli do mínusu (voliteľné)
        if (currentScore < 0) currentScore = 0;

        if (currentScore >= winningScore)
        {
            WinGame();
        }
    }

    public void ResetMultiplier()
    {
        timeSinceLastBadHit = 0f;
        scoreMultiplier = 1f;
    }

    // Funkcia na prekreslenie textu
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // (int) oreže desatinné miesta, aby skóre nebolo 12.4589
            scoreText.text = "Body: " + (int)currentScore + "\nNásobič: x" + scoreMultiplier;
        }
    }

    public void WinGame()
    {
        isGameActive = false;
        if (scoreText != null) scoreText.text = "VYHRAL SI!\nSkóre: " + (int)currentScore;
        Time.timeScale = 0; 
    }

    public void GameOver()
    {
        isGameActive = false;
        if (scoreText != null) scoreText.text = "PREHRAL SI!\nSkús znova.";
        Time.timeScale = 0;
    }
}