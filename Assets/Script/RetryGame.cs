using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        Cursor.visible = false;
        //Score.Instance.ResetScore();
        SceneManager.LoadScene("PoliInvaders");
    }

    public void LoadMenu()
    {
        Cursor.visible = true;
        //Score.Instance.ResetScore();
        SceneManager.LoadScene("MainMenuScene");
    }

        public void LoadLeaderBoard()
    {
        Cursor.visible = true;
        //Score.Instance.ResetScore();
        SceneManager.LoadScene("Leaderboard");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
