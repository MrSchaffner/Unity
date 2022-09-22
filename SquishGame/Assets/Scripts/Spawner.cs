using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Rigidbody enemyPrefab;
    public Transform spawner;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    public Enemy Spawn()
    {
        Rigidbody enemyInstance = Instantiate(enemyPrefab, spawner.position, spawner.rotation) as Rigidbody;


        return enemyInstance.GetComponent<Enemy>(); //goes to parent object

    }

    // Update is called once per frame. Checks if all enemies are dead
    void Update()
    {
        

        //made it out
 
    }
}
