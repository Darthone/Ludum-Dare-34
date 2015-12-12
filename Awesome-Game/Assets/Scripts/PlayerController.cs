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

    // Random intial Growth Direction
    Direction GrowthDirection;
	// Use this for initialization

	void Start () {
        // pick a random start direction
        System.Random rng = new System.Random();
    	GrowthDirection = (Direction)Enum.GetValues(typeof(Direction)).GetValue(rng.Next(2));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate() {

    }

    bool CheckPlant() {
        // check and update health and other stats
        // ends the game if needed
        return true;
    }
}
