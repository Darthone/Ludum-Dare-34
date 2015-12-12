using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

    public GameObject LeafPrefab;
    public GameObject StemPrefab;

    enum Direction{Left, Right};

    float GrowthSpeed = 1f;
    float EnergyLevel = 50f; // used for turning, forking, or  creating leaves
    float SunLevel = 100f;
    float WaterLevel = 100f;
    float StructuralIntegrity = 100f;
    float Health = 100f;
    float StemTime = 6f;
    float GrowingAngle = 0f;

    Vector3 NextPosition;
    GameObject CurrentStem;

    // Random intial Growth Direction
    Direction GrowthDirection;
	// Use this for initialization

    IEnumerator Spliter(float delay) {
        yield return new WaitForSeconds(delay);
        //multiplyer += 0.5f + 0.05f * delay;
        //minSpawnTime = Mathf.Clamp(minSpawnTime - 0.1f, 0.1f, 1f);
        //maxSpawnTime = Mathf.Clamp(maxSpawnTime - 0.2f, 1.5f, 3f);
        //powerupChance = Mathf.Clamp(powerupChance + 0.01f, 0.20f, 0.33f);
        //StartCoroutine(IncreaseMultiplyer(0.04f * delay * delay + 1f + delay));
    }

	void Start () {
        NextPosition = this.transform.position;
        // pick a random start direction
        System.Random rng = new System.Random();
    	GrowthDirection = (Direction)Enum.GetValues(typeof(Direction)).GetValue(rng.Next(2));
        if (GrowthDirection == Direction.Left) {
            GrowingAngle = (float)(-5 - rng.Next(15));
        } else {
            GrowingAngle = (float)(5 + rng.Next(15));
        }
        this.CurrentStem = (GameObject)Instantiate(StemPrefab, this.transform.position + new Vector3(0, 0, this.transform.position.z - 1), Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
	}

    void OnDrawGizmos() {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.CurrentStem.transform.position, this.CurrentStem.transform.position + Quaternion.AngleAxis(GrowingAngle, Vector3.forward) *  Vector3.up) ;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {
        NextPosition += this.GetGrowthAngle() * Vector3.up * this.GetGrowthSpeed() * 0.05f;
    }

    public float GetGrowthSpeed() {
        return this.GrowthSpeed;
    }

    public Quaternion GetGrowthAngle() {
        return Quaternion.AngleAxis(GrowingAngle, Vector3.forward);
    }

    public Vector3 GetNextPosition(){
        return this.NextPosition;
    }

    bool CheckPlant() {
        // check and update health and other stats
        // ends the game if needed
        return true;
    }
}
