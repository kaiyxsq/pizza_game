using UnityEngine;

public class ScreenBoundsCollider : MonoBehaviour
{
    void Start()
    {
        AddCollider();
    }

    void AddCollider()
    {
        // 1. Získame alebo pridáme EdgeCollider2D
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        if (edgeCollider == null)
        {
            edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        // 2. Získame hlavnú kameru
        Camera cam = Camera.main;
        
        // Ak kamera nie je ortografická (2D), toto nemusí fungovať správne
        if (!cam.orthographic) 
        {
            Debug.LogError("Kamera nie je nastavená na Orthographic!");
            return;
        }

       
        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topLeft = cam.ScreenToWorldPoint(new Vector3(0, Screen.height, cam.nearClipPlane));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.nearClipPlane));
        Vector2 bottomRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, cam.nearClipPlane));

        // 4. Nastavíme body collidera
        // Musíme pridať 5 bodov, aby sme uzavreli obdĺžnik (začneme a skončíme v rovnakom bode)
        edgeCollider.points = new Vector2[] { bottomLeft, topLeft, topRight, bottomRight, bottomLeft };
    }
}