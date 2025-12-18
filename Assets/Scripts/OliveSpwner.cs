using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliveSpawner : MonoBehaviour
{
    [Header("Čo a kde")]
    public GameObject olivePrefab;      // Sem daj Prefab Olivy
    public Transform pizzaTransform;    // Sem pretiahni Pizzu
    public Camera mainCamera;

    [Header("Nastavenia Spawnu")]
    public Vector2 spawnDirection = new Vector2(1, 0); 
    public float minRadius = 4f;
    public float maxRadius = 9f;

    [Header("Časovanie a Limity")]
    public int maxOlives = 20;          // Koľko olív max
    public float minTime = 0.5f;        // Rýchlosť
    public float maxTime = 1.5f;

    // Interné premenné len pre olivy
    private Queue<GameObject> oliveQueue = new Queue<GameObject>();
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
            TrySpawnOlive();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        currentDelay = Random.Range(minTime, maxTime);
    }

    void TrySpawnOlive()
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
        GameObject newOlive = Instantiate(olivePrefab, position, Quaternion.identity);

        // Rotácia k stredu
        Vector3 directionToCenter = pizzaTransform.position - newOlive.transform.position;
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        newOlive.transform.rotation = Quaternion.Euler(0, 0, angle);

        newOlive.transform.SetParent(pizzaTransform);

        // Pridanie do fronty a kontrola limitu
        oliveQueue.Enqueue(newOlive);

        if (oliveQueue.Count > maxOlives)
        {
            GameObject oldOlive = oliveQueue.Dequeue();
            if (oldOlive != null) Destroy(oldOlive);
        }
    }
}