using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball_move : MonoBehaviour
{

    public float speed;
    private Rigidbody rigidbody;
    //float maxSpeed = 1.0f; // units/sec
    float brakeSpeed = .995f; // 1.0 = no braking

    // Start is called before the first frame update
    void Start()
    {
        speed = 1.5f;
        rigidbody = this.GetComponent<Rigidbody>();    
    }

    private void FixedUpdate() //updates every Time.fixedDeltaTime * seconds - default is 50/s
    {
        float moveH = Input.GetAxis("Horizontal"); //complex variable tied to keypresses
        float moveV = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveH, 0.0f, moveV);
        rigidbody.AddForce(movement * speed);

    }

    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            Vector3 vel = rigidbody.velocity;
            if (vel.magnitude < .2f) vel = new Vector3(0f, 0f, 0f);

                rigidbody.velocity = vel * brakeSpeed;
        }


    }

    


}


//float maxSpeed = 1.0f; // units/sec
//void FixedUpdate()
//{
//    Rigidbody rb = GetComponent<Rigidbody>();
//    Vector3 vel = rb.velocity;
//    if (vel.magnitude > maxSpeed)
//    {
//        rb.velocity = vel.normalized * maxSpeed;
//    }
//}