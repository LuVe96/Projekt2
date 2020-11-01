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
        Vector2 keyboardMovement = getMovementInput();
    
        if(joystick.Horizontal <= -0.1 ||  keyboardMovement.x <= -0.1)
        {
            transform.position += new Vector3(-playerSpeedWD, 0, 0);
            playerIsMoving = true;
        }
        else if (joystick.Horizontal >= 0.1 || keyboardMovement.x >= 0.1)
        {
            transform.position += new Vector3(playerSpeedWD, 0, 0);
            playerIsMoving = true;
        }

        if (joystick.Vertical <= -0.1 || keyboardMovement.y <= -0.1)
        {
            transform.position += new Vector3(0, 0, -playerSpeedWD);
            playerIsMoving = true;
        }
        else if (joystick.Vertical >= 0.1 || keyboardMovement.y >= 0.1)
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
        else if (keyboardMovement.x != 0|| keyboardMovement.y != 0)
        {
            var rotation = Quaternion.LookRotation(new Vector3(keyboardMovement.x, 0, keyboardMovement.y));
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



    private string horizontalKeyDown = "";
    private string verticalKeyDown = "";

    private Vector2 getMovementInput()
    {
        Vector2 h = new Vector2(0, 0);



        if ((Input.GetKeyDown(KeyCode.A) || horizontalKeyDown == "A") && horizontalKeyDown != "D")
        {
            horizontalKeyDown = "A";
            h = new Vector2(-1, h.y);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || horizontalKeyDown == "D") && horizontalKeyDown != "A")
        {
            horizontalKeyDown = "D";
            h = new Vector2(1, h.y);
        }

        if ((Input.GetKeyDown(KeyCode.W) || verticalKeyDown == "W") && verticalKeyDown != "S")
        {
            verticalKeyDown = "W";
            h = new Vector2(h.x, 1);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || verticalKeyDown == "S")  && verticalKeyDown != "W")
        {
            verticalKeyDown = "S";
            h = new Vector2(h.x, -1);
        }


        if ((Input.GetKeyUp(KeyCode.A) && horizontalKeyDown == "A") || (Input.GetKeyUp(KeyCode.D) && horizontalKeyDown == "D"))
        {
            horizontalKeyDown = "";
            h = new Vector2(0, h.y);
        }

        if ((Input.GetKeyUp(KeyCode.W) && verticalKeyDown == "W") || (Input.GetKeyUp(KeyCode.S) && verticalKeyDown == "S"))
        {
            verticalKeyDown = "";
            h = new Vector2(h.x, 0);
        }

        return h;
    }
       
}
