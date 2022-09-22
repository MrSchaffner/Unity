using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_FSM : MonoBehaviour
{
    #region Player Variables

    public SphereCollider collider;

    private BaseState currentState;
    public BaseState CurrentState
    {
        get { return currentState; }
    }

    public float jumpForce;
    public Transform head;
    public Transform weapon01;
    public Transform weapon02;
    
    private Rigidbody rbody;

    public Rigidbody rb
    {
        get { return rbody; }
    }

    #endregion

    #region Player States
    public readonly JumpingState JumpingState = new JumpingState();
    public readonly DuckingState DuckingState = new DuckingState();

    #endregion

    public void Start()
    {
        TransitionToState(DuckingState);
        Debug.Log("in start()");
        collider = this.GetComponent<SphereCollider>(); //gets component attached to OBJECT this script is attached to.
       jumpForce = 300;
        
    }

    private void Awake()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
