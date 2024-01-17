using Riptide;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SessionType : ushort
{
    pub = 1,
    priv = 2,
}

public class MultiJoinLobby : MonoBehaviour
{
    private static MultiJoinLobby _singleton;

    public static MultiJoinLobby Singleton
    {
        get => _singleton;
        private set {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else
            {
                Debug.Log($"{nameof(MultiJoinLobby)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public bool startPrivate;

    [SerializeField] private GameObject menu;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text wrongCode;

    [SerializeField] private Button codeButton;
    [SerializeField] private TMP_Text privateCode;

    public TMP_Text GetPrivateCodeText()
    {
        return privateCode;
    }

    public void CopyToClipboard()
    {
        GUIUtility.systemCopyBuffer = privateCode.text;
    }

    public void JoinPublicSession()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.joinSession);
        message.AddUShort((ushort)SessionType.pub);

        NetworkManager.Singleton.Client.Send(message);
        Singleton.menu.SetActive(false);
    }

    public void StartPrivateSession()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.joinSession);
        message.AddUShort((ushort)SessionType.priv);

        startPrivate = true;
        NetworkManager.Singleton.Client.Send(message);
        Singleton.codeButton.gameObject.SetActive(true);
        Singleton.menu.SetActive(false);
    }

    public void JoinPrivateSession()
    {
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerId.joinSession);
        message.AddUShort((ushort)SessionType.priv);
        message.AddString(inputField.text);

        NetworkManager.Singleton.Client.Send(message);
        Singleton.menu.SetActive(false);
    }

    [MessageHandler((ushort)ServerToClientId.wrongCode)]
    private static void WrongCode(Message _)
    {
        Singleton.menu.SetActive(true);
        Singleton.wrongCode.gameObject.SetActive(true);
    }

    private void Awake()
    {
        Singleton = this;

        Singleton.wrongCode.gameObject.SetActive(false);
        Singleton.codeButton.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
