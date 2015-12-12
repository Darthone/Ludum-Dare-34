using UnityEngine;
using System.Collections;

public class RainCloud : MonoBehaviour {
    System.Random rng = new System.Random();
    public float RainDelay;
    public float SpawnTime;
    public float SpawnRange; // horizontal line
    public GameObject RainPrefab;
    public bool IsRaining = true;


    IEnumerator DelayRain(float delay) {
        yield return new WaitForSeconds(delay);
    }

    IEnumerator DelayDrops() {
        // TODO get random number
        yield return new WaitForSeconds(SpawnTime);
        if (IsRaining) {
            CreateDrop();
        }
        StartCoroutine(DelayDrops());
    }

    void CreateDrop() {

    }

	// Use this for initialization
	void Start () {
	    StartCoroutine(DelayRain(RainDelay))
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    
	}
}
