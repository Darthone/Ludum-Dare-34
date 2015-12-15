using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public PlayerController pc;
    Vector3 offset;

    void Start() {
        offset = this.transform.position;
    }

    void FixedUpdate (){
        if(!GameController.control.gameOver)
            this.transform.position = offset + pc.GetNextPosition(); 
    }
}