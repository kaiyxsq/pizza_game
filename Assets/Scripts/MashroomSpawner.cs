using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    [Header("Čo a kde")]
    public GameObject mushroomPrefab;   // Sem daj Prefab Hríbu
    public Transform pizzaTransform;    // Sem pretiahni Pizzu
    public Camera mainCamera;

    [Header("Nastavenia Spawnu")]
    public Vector2 spawnDirection = new Vector2(1, 0); 
    public float minRadius = 4f;
    public float maxRadius = 9f;

    [Header("Časovanie a Limity")]
    public int maxMushrooms = 5;        // Koľko hríbov max
    public float minTime = 2.0f;        // Hríby môžu byť pomalšie
    public float maxTime = 4.0f;

    // Interné premenné len pre hríby
    private Queue<GameObject> mushroomQueue = new Queue<GameObject>();
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
            TrySpawnMushroom();
            timer = 0f;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        currentDelay = Random.Range(minTime, maxTime);
    }

    void TrySpawnMushroom()
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
        GameObject newMushroom = Instantiate(mushroomPrefab, position, Quaternion.identity);

        // Rotácia k stredu
        Vector3 directionToCenter = pizzaTransform.position - newMushroom.transform.position;
        float angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;
        newMushroom.transform.rotation = Quaternion.Euler(0, 0, angle);

        newMushroom.transform.SetParent(pizzaTransform);

        // Pridanie do fronty a kontrola limitu
        mushroomQueue.Enqueue(newMushroom);

        if (mushroomQueue.Count > maxMushrooms)
        {
            GameObject oldMushroom = mushroomQueue.Dequeue();
            if (oldMushroom != null) Destroy(oldMushroom);
        }
    }
}