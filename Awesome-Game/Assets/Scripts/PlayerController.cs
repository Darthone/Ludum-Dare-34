using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    System.Random rng = new System.Random();

    public GameObject LeafPrefab;
    public GameObject StemPrefab;

    public Slider ScoreSlider;
    public Slider BalanceSlider;
    public Slider WaterSlider;
    public Slider EnergySlider;

    public Text txtScore;

    enum Direction{Left, Right};

    float GrowthSpeed = 0.05f;
    int StemCount = 0;
    int EnergyLevel = 100; // used for turning, forking, or  creating leaves
    int MaxEnergyLevel = 100;
    int SunLevel = 100;
    int MaxSunLevel = 100;
    int WaterLevel = 100;
    int MaxWaterLevel = 100;
    float StructuralIntegrity = 100f;
    float Score = 0;
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
    Vector3 StartPosition;
    GameObject CurrentStem;

    // Random intial Growth Direction
    Direction GrowthDirection;


    IEnumerator Cycle(float delay) {
        // perform calculations to reduce water and light
        yield return new WaitForSeconds(delay);
        WaterLevel -= 3 * StemCount;
        SunLevel -= 1 * StemCount;

        float temp = WaterLevel / (float)MaxWaterLevel;
        temp = WaterLevel / (float)MaxWaterLevel;
        EnergyLevel += (int)(temp * 0.25f * 100); 
        temp = SunLevel / (float)MaxSunLevel;
        EnergyLevel += (int)(temp * 0.25f * 100);

        EnergySlider.maxValue = MaxEnergyLevel;
        WaterSlider.maxValue = MaxWaterLevel;
        EnergySlider.value = EnergyLevel;
        WaterSlider.value = WaterLevel;
        //HealthSlider.maxValue = MaxEnergyLevel;

        StartCoroutine(Cycle(delay));
    }

    void Split() {  // create a new stem
        this.CurrentStem.GetComponent<Stem>().StopGrowing(); // stop growing the old one
        GrowingAngle = getNewAngle(GrowingAngle); // get a  new angle based on the directions
        // create new stem
        this.CurrentStem = (GameObject)Instantiate(StemPrefab, NextPosition + Vector3.back, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        this.StemCount++;
        this.StructuralIntegrity -= Mathf.Abs(this.NextPosition.x - StartPosition.x) * .25f;
        BalanceSlider.value = StructuralIntegrity;
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
        StartPosition = this.transform.position;
        BalanceSlider.maxValue = 100;
        // pick a random start direction
    	GrowthDirection = (Direction)Enum.GetValues(typeof(Direction)).GetValue(rng.Next(2));
        GrowingAngle = getNewAngle();
        this.CurrentStem = (GameObject)Instantiate(StemPrefab, this.transform.position + Vector3.back, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        StartCoroutine(Spliter(StemTime));
        StartCoroutine(Reset_Leaf(LeafDelay));
        StartCoroutine(Reset_Turn(TurnDelay));
        StartCoroutine(Cycle(8f));
        EnergySlider.maxValue = MaxEnergyLevel;
        WaterSlider.maxValue = MaxWaterLevel;
        EnergySlider.value = EnergyLevel;
        WaterSlider.value = WaterLevel;}

    void CreateLeaf() {
        //TODO
        GameObject temp;
        if (GrowthDirection == Direction.Left) {
            temp = (GameObject)Instantiate(LeafPrefab, NextPosition + Vector3.forward, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
            temp.transform.localScale = new Vector3(-1, 1, 1);
        } else {
            temp = (GameObject)Instantiate(LeafPrefab, NextPosition + Vector3.forward, Quaternion.AngleAxis(GrowingAngle, Vector3.forward));
        }
        //add to score for making a leaf
        Score += 25;
        txtScore.text = Score.ToString();
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
        this.WaterLevel = Mathf.Clamp(this.WaterLevel, 0, MaxWaterLevel);
        //Update slider
        this.WaterSlider.value = WaterLevel;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Button_0")) {
            if (CanTurn && EnergyLevel >= EnergySplit) {
                EnergyLevel -= EnergySplit;
                //Update slider
                this.EnergySlider.value = EnergyLevel;
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
                //Update slider
                this.EnergySlider.value = EnergyLevel;
                CanLeaf = false;
                CreateLeaf();
                StartCoroutine(Reset_Leaf(LeafDelay));
            } else {
                Debug.Log("ADD a Sound to not being able to Leaf");
                //TODO Play sound
            }
        }
        txtScore.text = this.NextPosition.y.ToString();


    }

    void FixedUpdate() {
        NextPosition += this.GetGrowthAngle() * Vector3.up * this.GetGrowthSpeed();
        MaxWaterLevel = 100 + this.StemCount * 15;
        MaxEnergyLevel = 100 + this.StemCount * 10;
        MaxSunLevel = 100 + this.StemCount * 10;
        SunLevel = Mathf.Clamp(SunLevel, 0, MaxSunLevel);
        WaterLevel = Mathf.Clamp(WaterLevel, 0, MaxSunLevel);
        EnergyLevel = Mathf.Clamp(EnergyLevel, 0, MaxSunLevel);
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
