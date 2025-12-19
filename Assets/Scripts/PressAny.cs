using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    void Update()
    {
        // Čaká na stlačenie ľubovoľnej klávesy
        if (Input.anyKeyDown)
        {
            // Načíta scénu s menu (uisti sa, že sa volá presne takto)
            SceneManager.LoadScene("MainMenu");
        }
    }
}