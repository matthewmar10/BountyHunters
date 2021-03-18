// Original source: https://docs.aws.amazon.com/gamelift/latest/developerguide/realtime-client.html

using System;
using System.Text;
using System.Collections;
using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Types;
using UnityEngine;
using Zenject;

/**
 * An example client that wraps the GameLift Realtime client SDK
 * 
 * You can redirect logging from the SDK by setting up the LogHandler as such:
 * ClientLogger.LogHandler = (x) => Console.WriteLine(x);
 *
 */
public class RealTimeClient
{
    public Aws.GameLift.Realtime.Client Client { get; private set; }

    //private EnemyPositionHandler _enemyPositionHandler = null;
    public GameObject _enemy;

    //private SceneHandlerService _sceneHandlerService;

    // An opcode defined by client and your server script that represents a custom message type
    private const int MY_TEST_OP_CODE = 10;
    private const int OPPONENT_VELOCITY = 215;


    private GameSessionFirst gameSession;

    /*
    [Inject]
    public RealTimeClient(SceneHandlerService sceneHandlerService)
    {
        _sceneHandlerService = sceneHandlerService;
    }
    */

    /// Initialize a client for GameLift Realtime and connect to a player session.
    /// <param name="endpoint">The DNS name that is assigned to Realtime server</param>
    /// <param name="remoteTcpPort">A TCP port for the Realtime server</param>
    /// <param name="listeningUdpPort">A local port for listening to UDP traffic</param>
    /// <param name="connectionType">Type of connection to establish between client and the Realtime server</param>
    /// <param name="playerSessionId">The player session ID that is assigned to the game client for a game session </param>
    /// <param name="connectionPayload">Developer-defined data to be used during client connection, such as for player authentication</param>
    public void init(string endpoint, int remoteTcpPort, int listeningUdpPort, ConnectionType connectionType,
                 string playerSessionId, byte[] connectionPayload, GameObject enemy)
    {
        Debug.Log("Entered RealTimeClient");

        // Create a client configuration to specify a secure or unsecure connection type
        // Best practice is to set up a secure connection using the connection type RT_OVER_WSS_DTLS_TLS12.
        ClientConfiguration clientConfiguration = new ClientConfiguration()
        {
            // C# notation to set the field ConnectionType in the new instance of ClientConfiguration
            ConnectionType = connectionType
        };

        // Create a Realtime client with the client configuration            
        Client = new Client(clientConfiguration);

        // Initialize event handlers for the Realtime client
        Client.ConnectionOpen += OnOpenEvent;
        Client.ConnectionClose += OnCloseEvent;
        Client.GroupMembershipUpdated += OnGroupMembershipUpdate;
        Client.DataReceived += OnDataReceived;

        // Create a connection token to authenticate the client with the Realtime server
        // Player session IDs can be retrieved using AWS SDK for GameLift
        ConnectionToken connectionToken = new ConnectionToken(playerSessionId, connectionPayload);

        Debug.Log("Before connect");
        // Initiate a connection with the Realtime server with the given connection information
        Client.Connect(endpoint, remoteTcpPort, listeningUdpPort, connectionToken);

        //_enemyPositionHandler = FindObjectOfType<EnemyPositionHandler>();

        //Vector3 enemyStartPos = new Vector3(0, 0, 0);
        //_enemyPositionHandler.init(enemyStartPos);

        //Instantiate(_enemy, new Vector3(0, 0, 0), Quaternion.identity);

        _enemy = enemy;

      
    }


  

    public void Disconnect()
    {
        Debug.Log("Disconnect");
        if (Client.Connected)
        {
            Client.Disconnect();
        }
    }

    public bool IsConnected()
    {
        Debug.Log("IsConnected");
        // _enemyPositionHandler.init(posMessage.enemyVelocity);
        return Client.Connected;
    }


    /// <summary>
    /// Example of sending a custom message to the server.
    /// 
    /// Server could be replaced by known peer Id etc.
    /// </summary>
    /// <param name="intent">Choice of delivery intent ie Reliable, Fast etc. </param>
    /// <param name="payload">Custom payload to send with message</param>
    public void SendMessage(DeliveryIntent intent, string payload)
    {
        Debug.Log("SendMessage");
        Client.SendMessage(Client.NewMessage(MY_TEST_OP_CODE)
            .WithDeliveryIntent(intent)
            .WithTargetPlayer(Constants.PLAYER_ID_SERVER)
            .WithPayload(StringToBytes(payload)));
    }

    public void SendMessage(DeliveryIntent intent, int opcode, string payload)
    {
        Debug.Log("SendMessage with opcode");
        Debug.Log(opcode);
        Debug.Log("Payload: " + payload);
        //Debug.Log("Intent: " + intent);
        //Debug.Log("Player ID Server: " + Constants.PLAYER_ID_SERVER);


        //gameSession = GetComponent<GameSessionFirst>();
        //string gameID = gameSession.gameSessionId;

        //PlayerSessionObject playerSession = PlayerController.GetComponent<PlayerSessionObject>();
        //string gameID = playerSession.GameSessionId;
        string gameID = GameSessionFirst.gameSessionId;

        Debug.Log("Game session ID: " + gameID);
        
        Client.SendMessage(Client.NewMessage(opcode)
            .WithDeliveryIntent(intent)
            .WithTargetPlayer(-1)
            .WithPayload(StringToBytes(payload)));

        // .WithTargetPlayer(Constants.PLAYER_ID_SERVER)
    }

    /**
     * Handle connection open events
     */
    public void OnOpenEvent(object sender, EventArgs e)
    {
        Debug.Log("OnOpenEvent");
    }

    /**
     * Handle connection close events
     */
    public void OnCloseEvent(object sender, EventArgs e)
    {
        Debug.Log("OnCloseEvent");
    }

    /**
     * Handle Group membership update events 
     */
    public void OnGroupMembershipUpdate(object sender, GroupMembershipEventArgs e)
    {
    }

    /**
     *  Handle data received from the Realtime server 
     */
    public virtual void OnDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.Log("OnDataReceived");

        string data = System.Text.Encoding.Default.GetString(e.Data);
        Debug.Log($"[server-sent] OnDataReceived - Sender: {e.Sender} OpCode: {e.OpCode} data: {data}");

        
        switch (e.OpCode)
        {
            // handle message based on OpCode
            //case ThirdPersonCharacterController.GAMEOVER_OP_CODE:
                //_sceneHandlerService.Outcome = data;

                //Disconnect();
                //break;

            case RealTimeClient.OPPONENT_VELOCITY:
                //PlayerPositionMessage posMessage = JsonUtility.FromJson<PlayerPositionMessage>(message);
                //_enemy.Move(data);
                _enemy.GetComponent<Enemy>().Move(data);
                //Debug.Log("Opponent velocity: " + data);

                break;

            default:
                break;


        }
        
    }

    
    // Helper method to simplify task of sending/receiving payloads
    public static byte[] StringToBytes(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    
    // Helper method to simplify task of sending/receiving payloads. 
    public static string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}