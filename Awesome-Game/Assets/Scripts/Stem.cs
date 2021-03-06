﻿using UnityEngine;
using System.Collections;

public class Stem : MonoBehaviour {

    PlayerController pc;
    bool Growing = true;
    
	// Use this for initialization
	void Start () {
        this.pc = GameController.control.pc;
	}
	
    public void StopGrowing(){
        Growing = false;
    }

	// Update is called once per frame
	void FixedUpdate () {
        if (Growing && !GameController.control.gameOver) {
            this.transform.localScale += new Vector3(0, pc.GetGrowthSpeed(), 0);
        } else {
            this.transform.localScale += new Vector3(0.001f, 0f, 0);
        }
	}
}
