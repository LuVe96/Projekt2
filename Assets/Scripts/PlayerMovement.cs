using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Joystick joystick;
    public float playerSpeed = 3f;
    public Transform playerModel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playerSpeedWD = playerSpeed * Time.deltaTime;
    
        if(joystick.Horizontal <= -0.1)
        {
            transform.position += new Vector3(-playerSpeedWD, 0, 0);
        }
        else if (joystick.Horizontal >= 0.1)
        {
            transform.position += new Vector3(playerSpeedWD, 0, 0);
        }

        if (joystick.Vertical <= -0.1)
        {
            transform.position += new Vector3(0, 0, -playerSpeedWD);
        }
        else if (joystick.Vertical >= 0.1)
        {
            transform.position += new Vector3(0, 0, playerSpeedWD);
        }

        if(joystick.Horizontal != 0 && joystick.Vertical != 0)
        {
            var rotation = Quaternion.LookRotation(new Vector3(joystick.Horizontal, 0, joystick.Vertical));
            playerModel.transform.rotation = Quaternion.RotateTowards(playerModel.transform.rotation, rotation, 30f);
        }
        
    }
}
