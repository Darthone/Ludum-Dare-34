﻿using UnityEngine;
using System.Collections;

public class RainDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
//        this.transform.position -= new Vector3(0, 0.2f);
//        if (this.transform.position.x < -10f)
//            Destroy(this.gameObject);
	}

    void OnCollisionEnter2D(Collision2D collision) {
        Destroy(this.gameObject);
        if (collision.gameObject.CompareTag("Ground")) {
            GameController.control.pc.Water(2);
        } else if (collision.gameObject.CompareTag("Leaf")) {
            GameController.control.pc.Water(0);
        } else if (collision.gameObject.CompareTag("Plant")){
            GameController.control.pc.Water(1);
        }
        // else pass
    }
}
