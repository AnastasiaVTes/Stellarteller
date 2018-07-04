using UnityEngine;
using System.Collections;

public class Orbit_Rotate : MonoBehaviour
{

    public Transform sun;
    public Transform planet;

    // public static 
    //public Rigidbody rb = UnityEngine.Component.GetComponent<Rigidbody>();
    void Awake()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        planet = this.transform;

        rb.AddForce(transform.forward * 100);
        rb.AddForce(transform.up * 100);
    }
    // Use this for initialization
    void Start()
    {
        GameObject sun_orb = GameObject.FindGameObjectWithTag("Sun");

        //sun = sun.transform; - srsly who names two different vars the same!!!!!
        sun = sun_orb.GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        Vector3 line = sun.position - planet.position;
        line.Normalize();
        float distance = Vector3.Distance(sun.position, planet.position);
        rb.AddForce(line * 10 / distance);
    }
}