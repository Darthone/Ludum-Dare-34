using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {
	// Update is called once per frame
    float x, y;
    void Start() {
        Vector3 scale = this.transform.localScale;
        if (scale.x < 0)
            x = -0.001f;
        else
            x = 0.001f;
        if (scale.y < 0)
            y = -0.001f;
        else
            y = 0.001f;
        this.transform.localScale = new Vector3(x * 100, y * 100, 1);
    }

	void FixedUpdate () {
        this.transform.localScale += new Vector3(x, y, 0);	
	}

    
}
