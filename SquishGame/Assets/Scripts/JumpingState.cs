using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : BaseState
{
    public override void EnterState(PlayerController_FSM player)
    {
        //  throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter(PlayerController_FSM player, Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().health--;
        }
        else // ground or wall was hit.
        {
            player.TransitionToState(player.DuckingState);
        }
    }

    public override void FixedUpdate(PlayerController_FSM player) //updates every Time.fixedDeltaTime * seconds - default is 50/s
    {

    }

    public override void Update(PlayerController_FSM player)
    {
        //throw new System.NotImplementedException();
    }
}
