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
        IsRaining = !IsRaining;
        StartCoroutine(DelayRain(delay));
    }

    IEnumerator DelayDrops() {
        // TODO get random number
        yield return new WaitForSeconds(SpawnTime);
        if (IsRaining) {
            CreateDrop();
        }
        StartCoroutine(DelayDrops());
    }

    void OnDrawGizmos() {
        //if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        //Debug.Log(this.transform.position + Vector3.left * SpawnRange);
        //Debug.Log(this.transform.position + Vector3.right * SpawnRange);
        Gizmos.DrawLine(this.transform.position + Vector3.left * SpawnRange, this.transform.position + Vector3.right * SpawnRange);
    }

    void CreateDrop() {
        Vector3 spawn = this.transform.position;
        float x = ((float)rng.NextDouble() * 2 * SpawnRange) - SpawnRange;
        spawn += new Vector3(x, 0f);
        GameObject temp = (GameObject)Instantiate(RainPrefab, spawn, this.transform.rotation);
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(DelayRain(RainDelay));
        StartCoroutine(DelayDrops());
	}
	
}
