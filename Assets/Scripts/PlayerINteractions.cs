using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Toto sa zavolá, keď vojdeme do Triggeru (napr. Oliva)
    void OnTriggerEnter2D(Collider2D other)
    {
        HandleObjectInteraction(other.gameObject, other.tag);
    }

    // Toto sa zavolá, keď narazíme do Pevnej prekážky (napr. Stena, ak nie je Trigger)
    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleObjectInteraction(collision.gameObject, collision.gameObject.tag);
    }

    void HandleObjectInteraction(GameObject obj, string tag)
    {
        if (GameManager.instance == null) return; // Poistka

        switch (tag)
        {
            case "BlackOlive":
                GameManager.instance.AddScore(200);
                Destroy(obj); // Zje olivu
                Debug.Log("+200 (Hnedá)");
                break;

            case "GreenOlive":
                GameManager.instance.AddScore(100);
                Destroy(obj); // Zje olivu
                Debug.Log("+100 (Zelená)");
                break;

            case "Salami":
                GameManager.instance.AddScore(-50);
                GameManager.instance.ResetMultiplier();
                Debug.Log("-50 (Saláma)");
                break;

            /*case "Wall":
                GameManager.instance.AddScore(-50);
                GameManager.instance.ResetMultiplier();
                Debug.Log("-50 (Stena)");
                break;

            case "Mushroom":
                GameManager.instance.GameOver();
                Debug.Log("GAME OVER (Hríb)");
                break;*/
        }
    }
}