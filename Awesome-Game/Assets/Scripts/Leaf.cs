using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.localScale += new Vector3(0.001f, 0.001f, 0);	
	}
}
