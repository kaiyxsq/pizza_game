using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Nastavenia Poškodenia")]
    public float wallDamagePerSecond = 50f;   // Koľko bodov berie stena za sekundu
    public float salamiDamagePerSecond = 30f; // Koľko bodov berie saláma za sekundu

    // ---------------------------------------------------------
    // 1. ZBIERANIE (Veci čo zmiznú)
    // Funguje len ak majú objekty "Is Trigger" = ZAŠKRTNUTÉ (✔️)
    // ---------------------------------------------------------
    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.instance == null) return;

        switch (other.tag)
        {
            case "BlackOlive":
                GameManager.instance.AddScore(200);
                Destroy(other.gameObject); // Zje sa a zmizne
                Debug.Log("Zjedol si Čiernu Olivu (+200)");
                break;

            case "GreenOlive":
                GameManager.instance.AddScore(100);
                Destroy(other.gameObject); // Zje sa a zmizne
                Debug.Log("Zjedol si Zelenú Olivu (+100)");
                break;
            
            case "Corn":
                GameManager.instance.AddScore(500);
                Destroy(other.gameObject); // Zje sa a zmizne
                Debug.Log("Zjedol si Kukuricu (+500)");
                break;
        }
    }

    // ---------------------------------------------------------
    // 2. NÁRAZ (Pevné veci)
    // Funguje len ak objekty "Is Trigger" = ODŠKRTNUTÉ (❌ - prázdne)
    // ---------------------------------------------------------
   void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Zistíme MENO objektu (nie len Tag)
        string menoObjektu = collision.gameObject.name;
        string tagObjektu = collision.gameObject.tag;

        // Ak narazíme do niečoho bez Tagu (napr. podlaha), ignorujeme to

        // 2. Ak to má Tag, riešime čo s tým
        if (tagObjektu == "Salami")
        {
            GameManager.instance.ResetMultiplier();
            GameManager.instance.AddScore(-salamiDamagePerSecond);
        }
        else if (tagObjektu == "Wall")
        {
             GameManager.instance.ResetMultiplier();
             GameManager.instance.AddScore(-wallDamagePerSecond);
        }
        else if (tagObjektu == "Mushroom")
        {
            GameManager.instance.GameOver();
        }
    }

    // ---------------------------------------------------------
    // 3. DRŽANIE (Pevné veci)
    // Pokiaľ sa o ne šúchaš, body klesajú
    // ---------------------------------------------------------
    void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.instance == null) return;

        string tag = collision.gameObject.tag;

        if (tag == "Salami")
        {
            // Odpočítame body podľa času
            float damage = salamiDamagePerSecond * Time.deltaTime;
            GameManager.instance.AddScore(-damage);
            
            // Udržiavame multiplier na 0
            GameManager.instance.ResetMultiplier();
        }
        /*else if (tag == "Wall")
        {
            float damage = wallDamagePerSecond * Time.deltaTime;
            GameManager.instance.AddScore(-damage);
            GameManager.instance.ResetMultiplier();
        }*/
    }
}