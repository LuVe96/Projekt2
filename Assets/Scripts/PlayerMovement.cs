using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Joystick joystick;
    public float playerSpeed = 3f;
    public Transform playerModel;
    static public bool playerIsMoving { get;  private set; } = false;
    private GameObject goalObject = null;

    private IAttackEnemyInterface attackEnemyInterface;

    // Update is called once per frame
    void Update()
    {
        float playerSpeedWD = playerSpeed * Time.deltaTime;
        playerIsMoving = false;
    
        if(joystick.Horizontal <= -0.1)
        {
            transform.position += new Vector3(-playerSpeedWD, 0, 0);
            playerIsMoving = true;
        }
        else if (joystick.Horizontal >= 0.1)
        {
            transform.position += new Vector3(playerSpeedWD, 0, 0);
            playerIsMoving = true;
        }

        if (joystick.Vertical <= -0.1)
        {
            transform.position += new Vector3(0, 0, -playerSpeedWD);
            playerIsMoving = true;
        }
        else if (joystick.Vertical >= 0.1)
        {
            transform.position += new Vector3(0, 0, playerSpeedWD);
            playerIsMoving = true;
        }

        // Player Looks at joystick direction
        if (joystick.Horizontal > 0.1 || joystick.Horizontal < -0.1 
            || joystick.Vertical < -0.1 && joystick.Vertical > 0.1)
        {
            var rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);
        }
        // Player Looks at position of next enemy
        else if (goalObject != null)
        {
            var rotation = Quaternion.LookRotation(goalObject.transform.position - playerModel.transform.position );
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);

            // when rotation ended
            if (rotation == playerModel.transform.rotation)
            {
                attackEnemyInterface.PlayerHasTurnedToEnemy(goalObject);
                goalObject = null;
            }
        }

    }



    public void LookAt(GameObject enemy, IAttackEnemyInterface attackEnemyInterface)
    {
        this.attackEnemyInterface = attackEnemyInterface;
        this.goalObject = enemy;
    }
}
