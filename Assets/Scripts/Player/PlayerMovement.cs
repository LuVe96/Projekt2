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
    private new Rigidbody rigidbody;
    private Animator animator;
    private float playerSpeedMultiplier = 1;

    private IAttackEnemyInterface attackEnemyInterface;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        playerIsMoving = false;
        Vector2 keyboardMovement = getMovementInput();

        Vector3 newRigidbodyVelocity = new Vector3(0, rigidbody.velocity.y, 0);

        //if (joystick.Horizontal <= -0.1 ||  keyboardMovement.x <= -0.1)
        //{
        //    newRigidbodyVelocity = new Vector3(-playerSpeed, rigidbody.velocity.y, newRigidbodyVelocity.z);
        //    playerIsMoving = true;
        //}
        //else if (joystick.Horizontal >= 0.1 || keyboardMovement.x >= 0.1)
        //{
        //    newRigidbodyVelocity = new Vector3(playerSpeed, rigidbody.velocity.y, newRigidbodyVelocity.z);
        //    playerIsMoving = true;
        //}

        //if (joystick.Vertical <= -0.1 || keyboardMovement.y <= -0.1)
        //{
        //    newRigidbodyVelocity = new Vector3(newRigidbodyVelocity.x, rigidbody.velocity.y, -playerSpeed);
        //    playerIsMoving = true;
        //}
        //else if (joystick.Vertical >= 0.1 || keyboardMovement.y >= 0.1)
        //{

        //    newRigidbodyVelocity = new Vector3(newRigidbodyVelocity.x, rigidbody.velocity.y, playerSpeed);
        //    playerIsMoving = true;
        //}
        if (joystick.Horizontal > 0.1 || joystick.Horizontal < -0.1
           || joystick.Vertical < -0.1 || joystick.Vertical > 0.1)
        {
            newRigidbodyVelocity = (new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized * playerSpeed * playerSpeedMultiplier) 
                + new Vector3(0,rigidbody.velocity.y,0);
                playerIsMoving = true;
        } else if (keyboardMovement.x != 0 || keyboardMovement.y != 0)
        {
            newRigidbodyVelocity = (new Vector3(keyboardMovement.x, 0, keyboardMovement.y).normalized * playerSpeed * playerSpeedMultiplier)
               + new Vector3(0, rigidbody.velocity.y, 0);
            playerIsMoving = true;
        }

            /// Player Looks at joystick direction
        if (joystick.Horizontal > 0.1 || joystick.Horizontal < -0.1 
        || joystick.Vertical < -0.1 || joystick.Vertical > 0.1)
        {
            var rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);

            animator.SetBool("isWalking", true);
        }
        else if (keyboardMovement.x != 0|| keyboardMovement.y != 0)
        {
            var rotation = Quaternion.LookRotation(new Vector3(keyboardMovement.x, 0, keyboardMovement.y));
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);

            animator.SetBool("isWalking", true);
        }

        // Player Looks at position of next enemy
        else if (goalObject != null)
        {
            // damit spieler sich nicht neigt
            Vector3 goalPos = new Vector3(goalObject.transform.position.x, playerModel.transform.position.y, goalObject.transform.position.z);
            var rotation = Quaternion.LookRotation(goalPos - playerModel.transform.position );
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);

            // when rotation ended
            if (rotation == playerModel.transform.rotation)
            {
                attackEnemyInterface.PlayerHasTurnedToEnemy(goalObject);
                goalObject = null;
            }

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


        rigidbody.velocity = newRigidbodyVelocity;

    }


    public void LookAt(GameObject enemy, IAttackEnemyInterface attackEnemyInterface)
    {
        this.attackEnemyInterface = attackEnemyInterface;
        this.goalObject = enemy;
    }


    public void setMovementSpeed(float multiplier)
    {
        playerSpeedMultiplier = multiplier;
        animator.SetFloat("walkspeedMultiplier", multiplier);
    }



    // Tastatur input

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
