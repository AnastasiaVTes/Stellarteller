using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cam_Switch : MonoBehaviour {
    public GameObject cam_pl;
    public GameObject cam_main;
    //public bool check_m;
    //public bool check_pl;
	// Use this for initialization
	void Start () {
        cam_main = GameObject.Find("Main Camera");
        cam_pl = GameObject.Find("Camera_Player");
        //check_m = cam_main.active;
        //check_pl = cam_pl.active;
        
	}

    public void OnClick() {
        if (cam_pl.activeInHierarchy == true)
        {
            cam_main.SetActive(true);
            cam_pl.SetActive(false);
            
        }
        else if (cam_main.activeInHierarchy == true)
        {
            cam_pl.SetActive(true);
            cam_main.SetActive(false);
        }
    
    }


	// Update is called once per frame
	void Update () {
		
	}
}
