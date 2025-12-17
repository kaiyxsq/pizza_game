using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;

    // Sem si uložíme, akú veľkú máš postavu (napr. 0.2 alebo 1)
    private float originalScaleX; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Zapamätáme si, akú veľkosť si nastavil v Inspectore (aby sme ju nepokazili)
        originalScaleX = transform.localScale.x;
    }

    void Update()
    {
        // 1. Získame vstup pre OBA smery
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D alebo Šípky do strán
        float moveY = Input.GetAxisRaw("Vertical");   // W/S alebo Šípky hore/dole

        // 2. Vytvoríme vektor pohybu
        // .normalized je dôležité, aby si nešiel šikmo rýchlejšie ako rovno
        Vector2 movement = new Vector2(moveX, moveY).normalized;

        // 3. Aplikujeme pohyb
        rb.velocity = movement * speed;

        // 4. LOGIKA ANIMÁCIE
        // Ak sa hýbeme (vektor nie je nula), spustíme beh
        bool isMoving = movement.magnitude > 0;
        anim.SetBool("isRunning", isMoving);

        // 5. OTÁČANIE (Zrkadlenie)
        // Otáčame len podľa X (doľava/doprava).
        // Ak ideme len hore/dole, necháme postavu otočenú tak, ako bola naposledy.
        if (moveX > 0)
        {
            // Doprava: Použijeme kladnú veľkosť
            transform.localScale = new Vector3(Mathf.Abs(originalScaleX), transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < 0)
        {
            // Doľava: Použijeme zápornú veľkosť
            transform.localScale = new Vector3(-Mathf.Abs(originalScaleX), transform.localScale.y, transform.localScale.z);
        }
    }
}