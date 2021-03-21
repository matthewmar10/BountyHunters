using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionHandler : MonoBehaviour
{
    // set in Unity editor in EnemyPositionHandler object
    public GameObject enemy;
    Dictionary<string, GameObject> playerIDdict;

    public void CreateEnemies(string[] playerList)
    {
        

        playerIDdict = new Dictionary<string, GameObject>();

        //Instantiate(enemy, transform.position, Quaternion.identity);
        //Debug.Log("test1");

        for (int i = 0; i < playerList.Length; i++)
        {
            Debug.Log("Texas" + playerList[i]);
            GameObject new_enemy = Instantiate(enemy, transform.position, Quaternion.identity);
  
            //playerIDdict[playerList[i]] = new_enemy;

            Debug.Log("i " + i);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }

    public void UpdateVelocity(string posMessage)
    {
        Debug.Log(posMessage);
        // make sure we set the first position before initializion is complete
        //enemy.Move(posMessage);
    }
}