using Riptide;
using Riptide.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ClientToServerId : ushort
{
    isReady = 1,
    playerData = 2,
    playerAction = 3,
    enemyHurt = 4,
    updateRestartCount = 5,
}

public enum ServerToClientId : ushort
{
    session = 1,
    playerJoined = 2,
    startGame = 3,
    playerData = 4,
    playerAction = 5,
    newEnemy = 6,
    updateEnemySpeed = 7,
    enemyDeath = 8,
    gameOver = 9,
    updateRestartCount = 10,
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

    private static ushort? session;
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
        Client.Disconnect();
        SceneManager.LoadScene("MainMenuScene");
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    [MessageHandler((ushort)(ServerToClientId.session))]
    private static void JoinSession(Message message)
    {
        session = message.GetUShort();
    }

    [MessageHandler((ushort)(ServerToClientId.playerJoined))]
    private static void PlayerJoined(Message message)
    {
        while (session == null) ;

        secPlayerId = message.GetUShort();
        players.Add(secPlayerId, null);

        Message playerReady = Message.Create(MessageSendMode.Reliable, (ushort)(ClientToServerId.isReady));
        playerReady.AddUShort(session.Value);
        Singleton.Client.Send(playerReady);
    }

    [MessageHandler((ushort)(ServerToClientId.startGame))]
    private static void StartGame(Message message)
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
            players[playerId].HandleAction(action);

            if (action == (ushort)PlayerActions.died)
            {
                players[playerId] = null;
            }
        }
    }

    [MessageHandler((ushort)ServerToClientId.gameOver)]
    private static void GameOver(Message message)
    {
        SceneManager.LoadScene("MultiGameOver");
    }
}
