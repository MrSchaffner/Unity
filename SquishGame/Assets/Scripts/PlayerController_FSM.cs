using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BAD ONE
public class PlayerController_FSM : MonoBehaviour
{
    #region Player Variables

    //public SphereCollider sCollider;
    public float jumpForce;
    private BaseState currentState;
    public BaseState CurrentState
    {
        get { return currentState; }
    }

    public Rigidbody rb;
 
    #endregion

    #region Player States
    public readonly JumpingState JumpingState = new JumpingState();
    public readonly DuckingState DuckingState = new DuckingState();

    #endregion

    public void Start()
    {
        Debug.Log("in playerCOntroller.Start();");
        TransitionToState(DuckingState);
        //sCollider = this.GetComponent<SphereCollider>(); //gets component attached to OBJECT this script is attached to.
        //rb = GetComponent<Rigidbody>();
        jumpForce = 300;
    }

    private void Awake()
    {
        Debug.Log("awake!");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("in playerCOntroller.Update();");
        currentState.Update(this); //passes to state
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        currentState.FixedUpdate(this); //passes to state
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public void TransitionToState(BaseState newState)
    {
        currentState = newState;
        newState.EnterState(this); // current player is this
    }
}
