using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private new Transform transform; //hides original transform data
    public PhysicMaterial newMaterial;

    public int health;
    public int speed = 15;

    public bool killedActionDone = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        transform = this.GetComponent<Transform>();
        health = 1;

        Vector3 movement = new Vector3(randVel(), 0.0f, randVel());
        // this.rb.AddForce(movement * speed);
        rb.AddForce(movement * speed);
        

    }

    public void Killed()
    {
        rb.velocity = new Vector3(0f,0f,0f);
        var scaleChange = new Vector3(0f, .9f, 0f);
        transform.localScale -= scaleChange;
        var collider = GetComponent<Collider>();
        collider.sharedMaterial = newMaterial;
        
    }

    private float randVel()
    {
        return Random.Range(-4f, 4f);
         
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("k"))
        {
            this.health = 0;
        }

        if (health <= 0 && !killedActionDone)
        {
            Killed();
            killedActionDone = true;
        }
    }
}
