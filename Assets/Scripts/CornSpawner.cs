using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornSpawner : MonoBehaviour
{
    [Header("Čo a kde")]
    public GameObject cornPrefab;       // Sem daj Prefab Kukurice
    public Transform pizzaTransform;    // Sem pretiahni Pizzu
    public Camera mainCamera;

    [Header("Nastavenia Spawnu")]
    // Môžeš zmeniť smer, aby kukurica chodila napr. zhora (0, 1) alebo zľava (-1, 0)
    public Vector2 spawnDirection = new Vector2(0, 1); 
    public float minRadius = 4f;
    public float maxRadius = 9f;

    [Header("Časovanie a Limity")]
    public int maxCorn = 10;            // Koľko kukurice max (zvyčajne menej ako olív)
    public float minTime = 1.0f;        // Kukurica môže chodiť pomalšie
    public float maxTime = 3.0f;

    // Interné premenné len pre kukuricu
    private Queue<GameObject> cornQueue = new Queue<GameObject>();
    private float timer = 0f;
    private float currentDelay;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentDelay)
        {
            TrySpawnCorn();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        currentDelay = Random.Range(minTime, maxTime);
    }

    void TrySpawnCorn()
    {
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
                SpawnObject(worldPosition);
                return;
            }
        }
    }

    void SpawnObject(Vector3 position)
    {
        GameObject newCorn = Instantiate(cornPrefab, position, Quaternion.identity);


        // Rotácia k stredu
        float randomZ = Random.Range(0f, 360f);
        newCorn.transform.rotation = Quaternion.Euler(0, 0, randomZ);

        newCorn.transform.SetParent(pizzaTransform);

        // Pridanie do fronty a kontrola limitu
        cornQueue.Enqueue(newCorn);

        if (cornQueue.Count > maxCorn)
        {
            GameObject oldCorn = cornQueue.Dequeue();
            if (oldCorn != null) Destroy(oldCorn);
        }
    }
}