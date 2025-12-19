using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Tento skript patrí len do scény "MainMenu".
    // Priraď tieto funkcie na tlačidlá (Buttons) v menu.

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Level4");
    }

    // Funkcia na vypnutie hry (dobré mať v hlavnom menu)
    public void QuitGame()
    {
        Debug.Log("Hra sa vypína...");
        Application.Quit();
    }
}