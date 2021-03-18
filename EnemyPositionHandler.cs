using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositionHandler : MonoBehaviour
{
    // set in Unity editor in EnemyPositionHandler object
    public Enemy enemy;

    public void init(Vector3 enemyPosMessage)
    {
        if (!enemy.isActiveAndEnabled)
        {
            // spawn enemy 
            enemy.SetActive(true);
        }
        enemy.Reset(enemyPosMessage);
    }

    public void UpdateVelocity(string posMessage)
    {
        // make sure we set the first position before initializion is complete
        enemy.Move(posMessage);
    }
}