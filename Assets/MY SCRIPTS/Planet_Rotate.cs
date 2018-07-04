using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_Rotate : MonoBehaviour {

    //double rotate_time = 5.0;
 //time in hours for one full rotation
 // ex: saturn = 10.66 hours
    public float speed = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {

      //transform.Rotate(0, (360/(rotate_time*60*60))*Time.deltaTime, 0, Space.Self);
      transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
