using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ClientToServerId : ushort
{
    joinSession = 1,
    isReady = 2,
    playerData = 3,
    playerAction = 4,
    enemyHurt = 5,
    updateRestartCount = 6,
}

public enum ServerToClientId : ushort
{
    session = 1,
    wrongCode = 2,
    playerJoined = 3,
    startGame = 4,
    playerData = 5,
    playerAction = 6,
    newEnemy = 7,
    updateEnemySpeed = 8,
    enemyDeath = 9,
    gameOver = 10,
    updateRestartCount = 11,
    endSession = 12,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client { get; private set; }

    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private static Guid? session;
    private static Dictionary<ushort, Multi_Player> players;

    private static ushort mainPlayerId;
    private static ushort secPlayerId;

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.ClientDisconnected += PlayerLeft;
        Client.Disconnected += DidDisconnect;

        players = new();
        session = null;

        Connect();
    }

    private void FixedUpdate()
    {
        Client.Update();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        mainPlayerId = Client.Id;
        players.Add(Client.Id, null);
    }

    private void FailedToConnect(object sender, EventArgs e)
    {

    }

    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {

    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    [MessageHandler((ushort)(ServerToClientId.session))]
    private static void JoinSession(Message message)
    {
        session = new(message.GetString());

        if (MultiJoinLobby.Singleton && MultiJoinLobby.Singleton.startPrivate)
        {
            MultiJoinLobby.Singleton.startPrivate = false;

            MultiJoinLobby.Singleton.GetPrivateCodeText().text = session.ToString();
        }
    }

    [MessageHandler((ushort)(ServerToClientId.playerJoined))]
    private static void PlayerJoined(Message message)
    {
        while (session == null) ;

        secPlayerId = message.GetUShort();
        players.Add(secPlayerId, null);

        Message playerReady = Message.Create(MessageSendMode.Reliable, (ushort)(ClientToServerId.isReady));
        playerReady.AddString(session.Value.ToString());
        Singleton.Client.Send(playerReady);
    }

    [MessageHandler((ushort)(ServerToClientId.startGame))]
    private static void StartGame(Message _)
    {
        while (session == null) ;

        SceneManager.sceneLoaded += GetPlayers;
        SceneManager.LoadScene("Scenes/MultiInvaders");
    }

    private static void GetPlayers(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name != "MultiInvaders")
        {
            return;
        }

        List<GameObject> gameObjects = new();
        arg0.GetRootGameObjects(gameObjects);

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.TryGetComponent<Multi_Player>(out var player))
            {
                if (player.IsMain)
                {
                    players[mainPlayerId] = player;
                }
                else
                {
                    players[secPlayerId] = player;
                }
            }
        }
    }

    [MessageHandler((ushort)(ServerToClientId.playerData))]
    private static void UpdateData(Message message)
    {
        ushort playerId = message.GetUShort();
        Vector3 position = message.GetVector3();

        if (players[playerId] != null)
        {
            players[playerId].PlayerMovement.UpdatePosition(position);
        }
    }

    [MessageHandler((ushort)ServerToClientId.playerAction)]
    private static void PlayerAction(Message message)
    {
        ushort playerId = message.GetUShort();
        ushort action = message.GetUShort();

        if (players[playerId] != null)
        {
            players[playerId].HandleAction(action, message);

            if (action == (ushort)PlayerActions.died)
            {
                players[playerId] = null;
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.gameOver)]
    private static void GameOver(Message _)
    {
        SceneManager.LoadScene("MultiGameOver");
    }

    [MessageHandler((ushort)ServerToClientId.endSession)]
    private static void EndSession(Message _)
    {
        Singleton.Client.Disconnect();
        SceneManager.LoadScene("MainMenuScene");
    }

    public bool isDisconnected()
    {
        return Client.IsConnected;
    }

    public bool isInSession()
    {
        return session.HasValue;
    }
}
