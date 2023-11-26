using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class Score : MonoBehaviour
{
    private static Score instance;
    private int score = 0;
    public Text scoreText;

    public static Score Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Score>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<Score>();
                    singletonObject.name = "Score (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    public int CurrentScore
    {
        get { return score; }
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "Score:\n" + score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "Score:\n" + score.ToString();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        scoreText.text = "Score:\n" + score.ToString();
    }
}
