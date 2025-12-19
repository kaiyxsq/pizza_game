using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    public float rotationSpeed = 20f;

    [Header("Špeciálne nastavenie pre Level 4")]
    public bool isLevel4 = false; // Toto zaškrtni v inšpektorovi len v Leveli 4
    public float minTime = 20f; // Najkratší čas, kedy sa smer otočí
    public float maxTime = 30f; // Najdlhší čas, kedy sa smer otočí

    private float timer = 0f;
    private float timeToNextChange;

    void Start()
    {
        // Na začiatku si určíme, kedy prebehne prvá zmena
        SetNextChangeTime();
    }

    void Update()
    {
        // Základná rýchlosť je tá, ktorú máš nastavenú
        float currentSpeed = rotationSpeed;

        // Ak je to Level 4, počítame čas a meníme smer
        if (isLevel4)
        {
            timer += Time.deltaTime;

            // Ak sme v čase medzi 10 a 20 sekundami, otočíme rýchlosť naopak
            if (timer >= timeToNextChange)
        {
            ChangeDirection();
            timer = 0f;             // Vynulujeme stopky
            SetNextChangeTime();    // Nastavíme nový čas pre ďalšiu zmenu
        }
        }

        // Aplikujeme rotáciu
        transform.Rotate(0, 0, currentSpeed * Time.deltaTime);
    }
    void ChangeDirection()
    {
        // Vynásobenie -1 zmení kladné číslo na záporné a naopak (otočí smer)
        rotationSpeed *= -1;
    }
    void SetNextChangeTime()
    {
        // Vygeneruje náhodný čas medzi 20 a 30 sekundami
        timeToNextChange = Random.Range(minTime, maxTime);
    }
}