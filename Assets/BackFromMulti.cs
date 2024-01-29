using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackFromMulti : MonoBehaviour
{
    public void BackMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
