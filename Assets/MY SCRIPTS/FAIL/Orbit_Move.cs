using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class Orbit_Move : MonoBehaviour {
     
     GameObject cube;
     public Transform center;
     public Vector3 axis = Vector3.up;
     public Vector3 desiredPosition;
     public Vector3 current_position;
     public float radius = 2.0f;
     public float radiusSpeed = 0.5f;
     public float rotationSpeed = 80.0f;
 
     void Start () {
         //get the transform of the object using it
         Vector3 vec_ob = transform.position;
         current_position = vec_ob;
         //move object
         cube = GameObject.FindWithTag("Sun");
         center = cube.transform;
         transform.position = (transform.position - center.position).normalized * radius + center.position;
         radius = 2.0f;

     }
     
     void Update () {
         transform.RotateAround (center.position, axis, rotationSpeed * Time.deltaTime);
         desiredPosition = (transform.position - center.position).normalized * radius + center.position;
         transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
     }
 }
