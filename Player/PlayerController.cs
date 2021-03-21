using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.CognitoIdentity;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Types;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Zenject;

using RTSGame;


public enum PlayerState
{
    walk,
    attack
}


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D player;
    private Animator anim;


    public PlayerState currentState;
    // public Transform playerCamera;

    [SerializeField]
    private float speed;

    private Vector3 moveDirection;
    // private Vector3 lastX, lastZ;
    // private float inputHorX, inputVertY;

    private bool firstZeroReceivedInARow = false;
    private bool playerIdle = true;
    //private float maxSpeed = 10;

    private GameSessionFirst gameSession;
    private RealTimeClient real_time_client;

    private const int OPPONENT_VELOCITY = 215;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(_enemy, new Vector3(0, 0, 0), Quaternion.identity);
        real_time_client = new RealTimeClient();
        gameSession = new GameSessionFirst(real_time_client);
        currentState = PlayerState.walk;
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
   
  
    /*
    [Inject]
    public void Construct(GameSessionFirst gameSession)
    {
        _gameSession = gameSession;
    }
    */


    // Need to alter this (aren't using WebSocketService)
    
    private void SendVectorAsMessage(Vector3 vector, int seq)
    {
        SerializableVector3 pos = vector;
        String posToSend = pos.ToString();
        //GameMessage posMessage = new PlayerPositionMessage("OnMessage", opCode, posToSend, new SerializableVector3(), 0, seq, "", localPlayerReference.position);
        //RTMessage posMessage = new RTMessage(opCode: OPPONENT_VELOCITY, targetPlayer: server_id, deliveryIntent: FAST, payload: posToSend);
        // posMessage.uuid = matchId;
        //SendWebSocketMessage(JsonUtility.ToJson(posMessage));

        string senderID = real_time_client.player_ID;
        posToSend = posToSend + senderID;

        real_time_client.SendMessage(DeliveryIntent.Fast, OPPONENT_VELOCITY, posToSend);
    }
   

    /*
    public async void SendWebSocketMessage(string message)
    {
        if (_websocket != null && _websocket.State == WebSocketState.Open)
        {
            // Sending plain text
            await _websocket.SendText(message);
        }
    }
    */


    // Sends velocity
    public void SendVelocity(Vector3 velocityIn)
    {
        int playerMovementMessageSequence = 0; // declared this here for the moment (remove later)
        SendVectorAsMessage(velocityIn, playerMovementMessageSequence++);
    }



    // A check to see if the user stopped moving
    private void PlayerIdleCheck(float x, float y)
    {
        if (x == 0 && y == 0)
        {
            if (firstZeroReceivedInARow)
            {
                // we have two zero messages, player not moving, stop sending messages
                playerIdle = true;
            }
            else
            {
                firstZeroReceivedInARow = true;
            }
        }
        else
        {
            // player moved, set both to false
            firstZeroReceivedInARow = false;
            playerIdle = false;
        }
    }



    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }  

    void FixedUpdate()
    {
        
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(moveX, moveY).normalized;
        
        
        if (Input.GetButtonDown("Attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState == PlayerState.walk)
        {
            if (Input.GetButtonDown("Switch1"))
            {
                anim.SetBool("sword", false);
            }
            else if (Input.GetButtonDown("Switch2"))
            {
                anim.SetBool("sword", true);
            }
            PlayerIdleCheck(moveX, moveY);

            if (!playerIdle) // skip sending extra zero vectors when player is not moving
            {
                Move();
            }
        }
    }

    public void Move()
    {
        player.MovePosition(
            transform.position + moveDirection * speed * Time.deltaTime);

        SendVelocity(moveDirection * speed);

        //player.velocity = new Vector2(moveDirection.x, moveDirection.y) * speed;

        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            anim.SetFloat("lastMoveX", moveDirection.x);
            anim.SetFloat("lastMoveY", moveDirection.y);
        }
    }


    // Need to alter this (we aren't using WebSocketService)
    /*
    private void PlayerMovement(float x, float y)
    {
        PlayerIdleCheck(x, y);

        if (!playerIdle) // skip sending extra zero vectors when player is not moving
        {
            Vector3 playerMovementRotation = new Vector3(x, 0f, y) * maxSpeed;

            Vector3 camRotation = playerCamera.transform.forward;
            camRotation.y = 0f; // zero out camera's vertical axis so it doesn't make them fly

            // need to clamp camera rotation to x/z only and not y vertical 
            Vector3 playerMovementWithCameraRotation = Quaternion.LookRotation(camRotation) * playerMovementRotation;

            // rounded to two decimal places
            Vector3 roundedVelocity
               = new Vector3(Mathf.Round(playerMovementWithCameraRotation.x * 100f) / 100f, 0f, Mathf.Round(playerMovementWithCameraRotation.z * 100f) / 100f);

            // Debug.Log("velocity to send: " + roundedVelocity.ToString("f6"));

            player.AddForce(roundedVelocity, ForceMode.VelocityChange);

            if (WebSocketService.Instance.matchInitialized)
            {
                WebSocketService.Instance.SendVelocity(roundedVelocity);
            }
        }
    }
    */


    private IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("attacking", false);

        //This creates a delay after using the weapon
        float delay = .67f;
        if (anim.GetBool("sword"))
            delay = .83f;
        else
            delay = .67f;

        yield return new WaitForSeconds(delay);
        currentState = PlayerState.walk;
    }

    public void TrySwapWeapon(WEAPON weapon)
    {
        Debug.Log(weapon);
    }
}
