using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class RetryGame : MonoBehaviour
{
    [SerializeField] Player jucator; //il am memorat pentru a mentine calea din fisier
    // Start is called before the first frame update
    void Start()
    {
        string assetsPath = Application.dataPath;
        Player.filePath = Path.Combine(Path.GetDirectoryName(assetsPath), "score.csv");
        Debug.Log(Player.filePath);   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        Cursor.visible = false;
        //Score.Instance.ResetScore();
        SceneManager.LoadScene("SelectLevel");
    }

    public void LoadLobby()
    {
        //Cursor.visible = false;
        SceneManager.LoadScene("Lobby");
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
