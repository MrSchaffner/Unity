using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckingState : BaseState
{
    public float speed = 1.5f;
    //private Rigidbody rb;
    //float maxSpeed = 1.0f; // units/sec
    float brakeSpeed = .995f; // 1.0 = no braking
    float frictionFactor = .995f;

    public override void EnterState(PlayerController_FSM player)
    {
      //  throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(PlayerController_FSM player, Collision collision)
    {
        // if hit enemy. you DEAD
    }

    public override void FixedUpdate(PlayerController_FSM player)
    {

        //MOVING
        float moveH = Input.GetAxis("Horizontal"); //complex variable tied to keypresses
        float moveV = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        player.rb.AddForce(movement * speed);
        player.rb.velocity *= frictionFactor;
        Vector3 vel = player.rb.velocity;
        if (vel.magnitude < .4f) vel = new Vector3(0f, 0f, 0f);

    }

    // Update is called once per frame
    public override void Update(PlayerController_FSM player)
    {
        //BRAKING
        //if (Input.GetKey("b"))
        //{
        //    player.rb.velocity = vel * brakeSpeed;
        //}

        if (Input.GetKey("space")) // JUMPING
        {
            
                player.rb.AddForce(Vector3.up * player.jumpForce);
            //once in the air, change to jumping state
                player.TransitionToState(player.JumpingState);
            
        }

    }
}
