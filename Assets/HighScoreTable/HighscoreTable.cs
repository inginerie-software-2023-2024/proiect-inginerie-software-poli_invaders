using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;
    //private List<HighscoreEntry> highscoreEntryList;
    
    private void Start()
    {
        entryContainer = transform.Find("highscoreEntryContainer");
        //Debug.Log("Container: " + entryContainer);
        entryTemplate = entryContainer.Find("highscoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        


        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores != null)
        {
            PlayerPrefs.DeleteKey("highscoreTable");
            PlayerPrefs.Save();
        }
        //Ma asigur ca am lista goala
        
        /*if (highscores == null)
        {
            Debug.Log("Initializing table with default values...");
            
            //Add default values
            AddHighscoreEntry(1);
            AddHighscoreEntry(2);

            
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }*/
        //i will comantate this haha
        
        //citesc scorurile din score.csv
        
        LoadScoresFromCSV("score.csv");
        
        
        
        
        

        //Sort entry list by Score
        for(int i=0;i<highscores.highscoreEntryList.Count;i++)
        {
            for(int j=i+1;j<highscores.highscoreEntryList.Count;j++)
            {
                if(highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    //Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }
        
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }

        /*Highscores highscores = new Highscores{highscoreEntryList = highscoreEntryList};
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));*/
        
        /*
        PlayerPrefs.DeleteKey("highscoreTable");
        PlayerPrefs.Save();
        */
        
        

    }
    


    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
            
        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
                
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
                
        }
            
        entryTransform.Find("posText").GetComponent<Text>().text = rankString;
            
        int score =highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        //entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        
        
        
        
        transformList.Add(entryTransform);
        
        //optional add alea pare impare
        
    }
    
    public void AddHighscoreEntry(int score)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score };

        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Sort the highscore list
        highscores.highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));

        // Limit the list to a specific number of entries (e.g., 10)
        const int maxEntries = 10;
        if (highscores.highscoreEntryList.Count > maxEntries)
        {
            highscores.highscoreEntryList = highscores.highscoreEntryList.GetRange(0, maxEntries);
        }

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void LoadScoresFromCSV(string filePath)
    {
        
        Debug.Log("Intra in csv");

        filePath = "D:\\PoliInva\\score.csv";
        //string m_Path = Application.dataPath ;
        //filePath = m_Path;
        //Debug.Log("CSV File Path: " + m_Path);
        
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Score file not found: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        List<int> scores = new List<int>();

        for (int i = 0; i < lines.Length; i++)
        {
            string[] fields = lines[i].Split(',');

            if (fields.Length > 0)
            {
                int score;
                if (int.TryParse(fields[0], out score))
                {
                    scores.Add(score);
                }
            }
        }

        // Add scores to highscore table
        foreach (int score in scores)
        {
            AddHighscoreEntry(score);
        }
    }

    
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
    
    //Clasa pentru a stoca un singur highscore
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        
    }
    
}
