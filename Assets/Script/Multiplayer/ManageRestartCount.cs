using Riptide;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageRestartCount : MonoBehaviour
{
    private static TextMeshProUGUI text;
    private bool wantsRestart;

    public void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        wantsRestart = false;
    }

    public void Update()
    {
        
    }

    public void LeaveSession()
    {
        NetworkManager.Singleton.Client.Disconnect();
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SwitchRestart()
    {
        wantsRestart ^= true;

        Message switchRestart = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.updateRestartCount);
        switchRestart.AddBool(wantsRestart);
        NetworkManager.Singleton.Client.Send(switchRestart);
    }

    [MessageHandler((ushort)ServerToClientId.updateRestartCount)]
    private static void UpdateRestartCount(Message message)
    {
        ushort total = message.GetUShort();
        text.text = $"{total}/2";
        
        if (total == 2)
        {
            SceneManager.LoadScene("MultiInvaders");
        }
    }
}
