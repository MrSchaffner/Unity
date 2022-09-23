using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public Transform target;
    public float rotateSpeed;
    private float simulationRate = .8f;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = this.gameObject.transform;
            Debug.Log("target not specified. setting to default.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(point: target.transform.position, axis: target.transform.up, angle: rotateSpeed * Time.deltaTime * simulationRate);


    }
}
