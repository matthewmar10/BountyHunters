using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour
{
    private SortedList<int, string> enemyPositionMessageQueue;
    private PlayerPositionMessage playerPositionDriftCheckMessage;
    private Rigidbody2D _enemy;
    private Animator anim;

    private const float DriftThreshold = 0.5f;

    [SerializeField]
    private float speed;

    private Vector3 moveDirection;

    private float maxSpeed = 10;

    public int enemyPositionSequence = 0;

    void FixedUpdate()
    {
        // NEED TO FIX THIS (NOT USING WebSocketService)
        
        // if (WebSocketService.Instance.matchInitialized && enemyPositionMessageQueue != null)
        /*
        if (enemyPositionMessageQueue != null)
        {
            // this FixedUpdate loop continuously applies whatever movement vectors are in the queue.
            // The list stores positions by sequence number, not index.
            PlayerPositionMessage enemyPositionToRender;
            //Vector3 movementPlane = new Vector3(_enemy.velocity.x, 0, _enemy.velocity.z);


            

            // Check if we have the next sequence to render
            if (enemyPositionMessageQueue.TryGetValue(enemyPositionSequence, out enemyPositionToRender))
            {
                // get the previous message's position for drift check
                PlayerPositionMessage previousEnemyPositionMessage;
                if (enemyPositionSequence > 1 && enemyPositionMessageQueue.TryGetValue(enemyPositionSequence - 1, out previousEnemyPositionMessage))
                {
                    // if our drift threshold is exceeded, perform correction
                    float drift = Vector3.Distance(_enemy.position, previousEnemyPositionMessage.currentPos);
                    if (drift >= DriftThreshold)
                    {
                        // Debug.Log("Drift detected ******************************");
                        StartCoroutine(CorrectDrift(_enemy.transform, _enemy.position, previousEnemyPositionMessage.currentPos, .2f));
                    }

                    // removes the previous message in queue now that we're done with the correction check
                    enemyPositionMessageQueue.Remove(enemyPositionToRender.seq - 1);
                }

                _enemy.AddForce(enemyPositionToRender.velocity, ForceMode.VelocityChange);

                // Debug.Log("Rendered queue sequence number: " + enemyPositionSequence);
                enemyPositionSequence++;
            }
        }
        */
    }



    public void Move(string velocity)
    {
        Debug.Log(velocity);
        string[] words = velocity.Split(' ');

        float flt1 = float.Parse(words[0]);
        float flt2 = float.Parse(words[1]);
        float flt3 = float.Parse(words[2]);

        moveDirection = new Vector3(flt1, flt2, flt3);

        _enemy.MovePosition(
            transform.position + moveDirection * speed * Time.deltaTime);


        anim.SetFloat("moveX", moveDirection.x);
        anim.SetFloat("moveY", moveDirection.y);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            anim.SetFloat("lastMoveX", moveDirection.x);
            anim.SetFloat("lastMoveY", moveDirection.y);
        }
    }




    void Update()
    {
        // Capping the speed/magnitude across network is critical to maintain smooth movement
        /*
        if (_enemy.velocity.magnitude > maxSpeed)
        {
            _enemy.velocity = Vector3.ClampMagnitude(_enemy.velocity, maxSpeed);
        }
        */
    }

    private IEnumerator CorrectDrift(Transform thisTransform, Vector3 startPos, Vector3 endPos, float correctionDuration)
    {
        float i = 0.0f;
        while (i < correctionDuration)
        {
            i += Time.deltaTime;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);

        }
        yield return null;
    }


    // NEED TO FIX THIS (not using WebSocketService)
    public void BufferState(string state)
    {
        /*
        // only add enemy position messages, for now
        if (state.opcode == WebSocketService.OpponentVelocity)
        {
            enemyPositionMessageQueue.Add(state.seq, state);
        }
        */
    }

    public void Reset(Vector3 enemyPosMessage)
    {
        _enemy.transform.position = enemyPosMessage;
        enemyPositionSequence = 0;
        enemyPositionMessageQueue = new SortedList<int, string>();
        // enemyPositionMessageQueue = new SortedList<int, PlayerPositionMessage>();
    }

    public void SetActive(bool activeFlag)
    {
        gameObject.SetActive(activeFlag);
    }

    void Awake()
    {
        Debug.Log("Enemy Awake");
        // _enemy = gameObject.GetComponent<Rigidbody2D>();
        _enemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetActive(true);
    }

    void Start()
    {
        Debug.Log("Enemy start");

    }
}