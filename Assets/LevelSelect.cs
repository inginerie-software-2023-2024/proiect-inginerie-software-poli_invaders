using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }
    
    public void PlayPoli()
    {
        SceneManager.LoadScene("PoliInvaders");
    }
    
    public void PlayLuca()
    {
        SceneManager.LoadScene("LucaInvaders 1");
    }

    public void PlayCoffee()
    {
        SceneManager.LoadScene("CoffeeInvaders");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
