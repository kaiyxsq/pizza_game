using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaToppingSpawner : MonoBehaviour
{
    [Header("Čo a kde")]
    public GameObject objectPrefab;     // Sem pretiahneš Hríb, Olivu, alebo čokoľvek iné
    public Transform parentTransform;   // Sem pretiahneš Pizzu (rodiča)
    public Camera gameCamera;           // Tvoja kamera

    [Header("Nastavenia Spawnu")]
    public Vector2 spawnDirection = new Vector2(1, 0); 
    public float minRadius = 4f;
    public float maxRadius = 9f;

    [Header("Časovanie a Limity")]
    public int maxObjects = 5;          // Maximálny počet objektov tohto typu
    public float minTime = 2.0f;        // Minimálny čas medzi spawnami
    public float maxTime = 4.0f;        // Maximálny čas medzi spawnami

    // Interný zoznam pre sledovanie vytvorených objektov
    private List<GameObject> activeObjects = new List<GameObject>();
    private float timer = 0f;
    private float currentDelay;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 1. Logika spawnovania
        if (timer >= currentDelay)
        {
            TrySpawnObject();
            timer = 0f;
            SetNextSpawnTime();
        }

        // 2. Logika mazania (Cleanup)
        CleanupOldObjects();
    }

    void SetNextSpawnTime()
    {
        currentDelay = Random.Range(minTime, maxTime);
    }

    void TrySpawnObject()
    {
        // Skúsime 10-krát nájsť pozíciu mimo obrazovky
        for (int i = 0; i < 10; i++)
        {
            float randomDist = Random.Range(minRadius, maxRadius);
            Vector3 offset = spawnDirection.normalized * randomDist;
            Vector3 worldPosition = parentTransform.position + offset;

            // Ak pozícia NIE JE na obrazovke, môžeme spawnovať
            if (!IsPositionOnScreen(worldPosition))
            {
                SpawnObject(worldPosition);
                return;
            }
        }
    }

    void SpawnObject(Vector3 position)
    {
        GameObject newObj = Instantiate(objectPrefab, position, Quaternion.identity);

        // Náhodná rotácia (univerzálna pre všetko)
        float randomRotation = Random.Range(0f, 360f);
        newObj.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        
        newObj.transform.SetParent(parentTransform);

        // Pridáme do zoznamu
        activeObjects.Add(newObj);
    }

    void CleanupOldObjects()
    {
        // Ak sme neprekročili limit, nič nerobíme
        if (activeObjects.Count <= maxObjects) return;

        // Prechádzame zoznam a hľadáme adepta na vymazanie
        for (int i = 0; i < activeObjects.Count; i++)
        {
            GameObject obj = activeObjects[i];

            if (obj == null)
            {
                activeObjects.RemoveAt(i);
                return;
            }

            // Kľúčová zmena: Vymažeme ho IBA ak je mimo obrazovky
            if (!IsPositionOnScreen(obj.transform.position))
            {
                Destroy(obj);
                activeObjects.RemoveAt(i);
                return; // Stačí vymazať jeden za frame
            }
        }
    }

    // Univerzálna funkcia na zistenie viditeľnosti
    bool IsPositionOnScreen(Vector3 pos)
    {
        Vector3 viewportPoint = gameCamera.WorldToViewportPoint(pos);
        return (viewportPoint.x > 0 && viewportPoint.x < 1 && 
                viewportPoint.y > 0 && viewportPoint.y < 1);
    }
}