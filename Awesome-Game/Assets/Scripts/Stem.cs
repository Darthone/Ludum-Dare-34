using UnityEngine;
using System.Collections;

public class Stem : MonoBehaviour {

    PlayerController pc;

    bool Growing = true;

	// Use this for initialization
	void Start () {
        this.pc = GameController.control.pc;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Growing) {
            this.transform.localScale += new Vector3(0, pc.GetGrowthSpeed() * 0.05f, 0);
        }
	}
}
