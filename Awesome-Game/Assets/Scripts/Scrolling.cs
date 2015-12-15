using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour {
    Renderer R;
    Vector3 delta;
    void Start(){
        this.R = this.GetComponent<Renderer>();
        delta = Camera.main.transform.position;

    }

	// Update is called once per frame
	void Update () {
        delta = delta - Camera.main.transform.position;
        R.material.mainTextureOffset -= (Vector2)delta * 0.02f;
        delta = Camera.main.transform.position; 
	}
}
