﻿using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
    System.Random rng = new System.Random();

    public GameObject LeafPrefab;
    public GameObject StemPrefab;

    enum Direction{Left, Right};

    float GrowthSpeed = 0.05f;
    int StemCount = 0;
    int EnergyLevel = 100; // used for turning, forking, or  creating leaves
    int SunLevel = 100;
    int WaterLevel = 100;
    float StructuralIntegrity = 100f;
    float Health = 100f;
    float StemTime = 6f;
    float GrowingAngle = 0f;

    int MaxGrowthAngle = 20;
    int EnergySplit = 13;
    int EnergyLeaf = 20;

    bool CanTurn = false;
    bool CanLeaf = false;

    float TurnDelay = 2f;
    float LeafDelay = 4f;

    Vector3 NextPosition;
    GameObject CurrentStem;

    // Random intial Growth Direction
    Direction GrowthDirection;


    IEnumerator Cycle(float delay) {
        // perform calculations to reduce water and light
        yield return new WaitForSeconds(delay);
        WaterLevel -= 3 * StemCount;

        StartCoroutine(Cycle(delay));
    }

    void Split() {  // create a new stem
        this.CurrentStem.GetComponent<Stem>().StopGrowing(); // stop growing the old one
        GrowingAngle = getNewAngle(GrowingAngle); // get a  new angle based on the directions
        // create new stem
        this.CurrentStem = (GameObject)Instantiate(StemPrefab, NextPosition + Vector3.back, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        this.StemCount++;
    }

    IEnumerator Spliter(float delay) {
        yield return new WaitForSeconds(delay);
        Split();
        //multiplyer += 0.5f + 0.05f * delay;
        //minSpawnTime = Mathf.Clamp(minSpawnTime - 0.1f, 0.1f, 1f);
        //maxSpawnTime = Mathf.Clamp(maxSpawnTime - 0.2f, 1.5f, 3f);
        //powerupChance = Mathf.Clamp(powerupChance + 0.01f, 0.20f, 0.33f);
        //StartCoroutine(IncreaseMultiplyer(0.04f * delay * delay + 1f + delay));
        StartCoroutine(Spliter(StemTime));
    }

    IEnumerator Reset_Leaf(float delay){
        yield return new WaitForSeconds(delay);
        CanLeaf = true;
    }

    IEnumerator Reset_Turn(float delay){
        yield return new WaitForSeconds(delay);
        CanTurn = true;
    }

    float getNewAngle() {
        if (GrowthDirection == Direction.Left) {
            return (float)(-5 - rng.Next(MaxGrowthAngle));
        } else {
            return (float)(5 + rng.Next(MaxGrowthAngle));
        }
    }

    float getNewAngle(float start) {
        if (GrowthDirection == Direction.Left) {
            return (float)(start - rng.Next(MaxGrowthAngle));
        } else {
            return (float)(start + rng.Next(MaxGrowthAngle));
        }
    }

    void SwitchGrowthDirection() {
        if (GrowthDirection == Direction.Left) {
            GrowthDirection = Direction.Right;
        } else {
            GrowthDirection = Direction.Left;
        }
    }

	void Start () {
        NextPosition = this.transform.position;
        // pick a random start direction
    	GrowthDirection = (Direction)Enum.GetValues(typeof(Direction)).GetValue(rng.Next(2));
        GrowingAngle = getNewAngle();
        this.CurrentStem = (GameObject)Instantiate(StemPrefab, this.transform.position + Vector3.back, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        StartCoroutine(Spliter(StemTime));
        StartCoroutine(Reset_Leaf(LeafDelay));
        StartCoroutine(Reset_Turn(TurnDelay));
        StartCoroutine(Cycle(8f));
	}

    void CreateLeaf() {
        //TODO
        GameObject temp;
        if (GrowthDirection == Direction.Left) {
            temp = (GameObject)Instantiate(LeafPrefab, NextPosition + Vector3.forward, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
            temp.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            temp = (GameObject)Instantiate(LeafPrefab, NextPosition + Vector3.forward, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        }
    }

    public void Water(int where) {
        switch (where) {
            case 0:
                // leaf
                this.WaterLevel += 2;
                break;
            case 1:
                // plant
                this.WaterLevel += 3;
                break;
            case 2:
                //ground
                this.WaterLevel += 1;
                break;
        }
        this.WaterLevel = Mathf.Clamp(this.WaterLevel, 0, this.StemCount * 15);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Button_0")) {
            if (CanTurn && EnergyLevel >= EnergySplit) {
                EnergyLevel -= EnergySplit;
                CanTurn = false;
                SwitchGrowthDirection(); 
                Split();
                StartCoroutine(Reset_Turn(TurnDelay));
            } else {
                Debug.Log("ADD a Sound to not being able to turn");
                //TODO Play sound
            }
        }
        if (Input.GetButtonDown("Button_1")) {
            if (CanLeaf && EnergyLevel >= EnergyLeaf) {
                EnergyLevel -= EnergyLeaf;
                CanLeaf = false;
                CreateLeaf();
                StartCoroutine(Reset_Leaf(LeafDelay));
            } else {
                Debug.Log("ADD a Sound to not being able to Leaf");
                //TODO Play sound
            }
        }

 
	}

    void FixedUpdate() {
        NextPosition += this.GetGrowthAngle() * Vector3.up * this.GetGrowthSpeed();
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

    public int GetStemCount(){
        return this.StemCount;
    }   

    bool CheckPlant() {
        // check and update health and other stats
        // ends the game if needed
        return true;
    }
}
