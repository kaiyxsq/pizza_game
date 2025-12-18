using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPizzaSpawner : MonoBehaviour
{
    [Header("Čo a kde")]
    public GameObject salamiPrefab;
    public Transform pizzaTransform;
    public Camera mainCamera;

    [Header("Pozícia Spawnu (Čiara)")]
    public float maxRadius = 9f;
    public float minRadius = 4f;
    public Vector2 spawnDirection = new Vector2(1, 0); // Smer doprava

    [Header("Časovanie (Náhodné intervaly)")]
    public float minTimeBetweenSpawns = 0.5f; // Najrýchlejší možný spawn
    public float maxTimeBetweenSpawns = 2.0f; // Najpomalší možný spawn

    [Header("Limity")]
    public int maxSalamis = 15; // Maximálny počet objektov naraz

    // Interné premenné
    private Queue<GameObject> salamiQueue = new Queue<GameObject>();
    private float timer = 0f;
    private float currentSpawnDelay; // Sem si uložíme aktuálne vyžrebovaný čas

    void Start()
    {
        // Hneď na začiatku si určíme, kedy príde prvá saláma
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Kontrolujeme, či časovač prekročil NÁHODNE vygenerovanú hodnotu
        if (timer >= currentSpawnDelay)
        {
            TrySpawnOnLine();
            
            // Resetujeme časovač
            timer = 0f;
            // A hneď si vyžrebujeme nový náhodný čas pre BUDÚCI spawn
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        // Random.Range vráti náhodné číslo medzi min a max
        currentSpawnDelay = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
    }

    void TrySpawnOnLine()
    {
        // Skúsime nájsť pozíciu mimo obrazovky
        for (int i = 0; i < 10; i++)
        {
            float randomDist = Random.Range(minRadius, maxRadius);
            Vector3 offset = spawnDirection.normalized * randomDist;
            Vector3 worldPosition = pizzaTransform.position + offset;

            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(worldPosition);
            bool isOnScreen = (viewportPoint.x > 0 && viewportPoint.x < 1 && 
                               viewportPoint.y > 0 && viewportPoint.y < 1);

            if (!isOnScreen)
            {
                SpawnSalami(worldPosition);
                return;
            }
        }
    }

    void SpawnSalami(Vector3 position)
    {
        // 1. Vytvorenie
        GameObject newSalami = Instantiate(salamiPrefab, position, Quaternion.identity);

        // 2. Rotácia (špičkou do stredu)
        Vector3 directionToCenter = pizzaTransform.position - newSalami.transform.position;
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        newSalami.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. Priradenie k rodičovi (Pizzi)
        newSalami.transform.SetParent(pizzaTransform);

        // 4. Správa limitu (Fronta)
        salamiQueue.Enqueue(newSalami);

        if (salamiQueue.Count > maxSalamis)
        {
            GameObject oldestSalami = salamiQueue.Dequeue();
            if (oldestSalami != null)
            {
                Destroy(oldestSalami);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pizzaTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 startLine = pizzaTransform.position + (Vector3)spawnDirection.normalized * minRadius;
            Vector3 endLine = pizzaTransform.position + (Vector3)spawnDirection.normalized * maxRadius;
            Gizmos.DrawLine(startLine, endLine);
            Gizmos.DrawSphere(startLine, 0.2f);
            Gizmos.DrawSphere(endLine, 0.2f);
        }
    }
}