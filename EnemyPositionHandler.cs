using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionHandler : MonoBehaviour
{
    // set in Unity editor in EnemyPositionHandler object
    public GameObject enemy;
    Dictionary<string, GameObject> playerIDdict;

    private bool enemyAllowed = false;
    private string[] playerIDList;

    public void CreateEnemies(string[] playerList)
    {
        playerIDList = playerList;

        enemyAllowed = true;    
    }

    void Update()
    {
        if (enemyAllowed)
        {
            playerIDdict = new Dictionary<string, GameObject>();
            for (int i = 0; i < playerIDList.Length; i++)
            {
                Debug.Log("Texas" + playerIDList[i]);

                GameObject new_enemy = (GameObject)Instantiate(enemy, transform.position, Quaternion.identity);
                playerIDdict[playerIDList[i]] = new_enemy;

                Debug.Log("i " + i);
            }
            enemyAllowed = false;
        }
    }

    public void UpdateVelocity(string posMessage)
    {
        Debug.Log(posMessage);
        // make sure we set the first position before initializion is complete
        //enemy.Move(posMessage);
    }
}