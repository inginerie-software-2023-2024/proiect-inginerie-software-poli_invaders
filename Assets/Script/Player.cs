using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;
    public float health;
    public Rigidbody2D rb;

    public SpriteRenderer playerSr;
    public Player_Movement playerMov;

    private Projectile projectileScript;

    public Sprite defaultSprite;
    public Sprite hurtSprite;
    public float hurtDuration = 0.25f;
    public float hurtTimer = 0.0f;

    public Sprite pinkSprite;
    public Sprite purpleSprite;
    public Sprite orangeSprite;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        health = 5f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo) {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null) {
            health -= 1;
            if (health <= 0) {
                
                //MOARE PLAYERU :((
                //Destroy(gameObject);
                //adaug aici scorul
                
                Debug.Log("Ba hai ca scrie in csv" + Score.Instance.CurrentScore);
                WriteScoreToCSV(Score.Instance.CurrentScore);
                
                
                
                if (projectileScript != null) {
                    projectileScript.enabled = false;
                }

                playerSr.enabled = false;
                playerMov.enabled = false;

                Cursor.visible = true;
                SceneManager.LoadScene("GameOverScene");
            }
            else
            {
                playerSr.sprite = hurtSprite;
                hurtTimer = hurtDuration;
            }


        }
    }

    // Added small easter egg for fun :)
    private void Update()
    {
        if (Input.GetKey(KeyCode.H))
        {
            playerSr.sprite = pinkSprite;
        }
        
        if (Input.GetKey(KeyCode.M))
        {
            playerSr.sprite = orangeSprite;
        }
        
        if (Input.GetKey(KeyCode.B))
        {
            playerSr.sprite = purpleSprite;
        }

        if (hurtTimer > 0) {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer <= 0) {
                playerSr.sprite = defaultSprite;
            }
        }

    }
    
    private void WriteScoreToCSV(int score)
    {
        string filePath = "D:\\PoliInva\\score.csv";
        
        Debug.Log("Intra in csv PALYERUYY cu scoru " + score + "!!");
        
        //filePath = "D:\\PoliInvaders-main\\scores.csv";
        //string m_Path = Application.dataPath + "/HighscoreTable/score.csv";
        //filePath = m_Path;
        //Debug.Log("CSV File Path: " + m_Path);

        // Check if the CSV file exists
        bool fileExists = File.Exists(filePath);

        // Create or append to the CSV file
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            // If the file doesn't exist, write the header row
            if (!fileExists)
            {
                sw.WriteLine("Score");
            }

            // Write the score to a new row
            sw.WriteLine(score.ToString());
        }
    }


}