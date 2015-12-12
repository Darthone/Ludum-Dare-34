using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    // Singleton Game Manager class
    public static GameController control = null;

    public GameObject player;
    public PlayerController pc;
    public GameObject floatingText;

    GUIText myGUIText;
    GUIStyle myGUIStyle;

    bool paused = false;
    public bool canPause = true;

    float fadeSpeed = 3f;          // Speed that the screen fades to and from black.
    private bool sceneStarting = true;      // Whether or not the scene is still fading in.
    public bool sceneEnding = false;
    public bool gameOver = false;

    public AudioClip newWorldAvailableSound;
    public AudioClip gameoverSound;

    public long score = 0;
    public float multiplyer = 1.0f;

    public float minSpawnTime = 1.5f;
    public float maxSpawnTime = 4f;
    public float powerupChance = .20f;

    IEnumerator IncreaseMultiplyer(float delay) {
        yield return new WaitForSeconds(delay);
        multiplyer += 0.5f + 0.05f * delay;
        minSpawnTime = Mathf.Clamp(minSpawnTime - 0.1f, 0.1f, 1f);
        maxSpawnTime = Mathf.Clamp(maxSpawnTime - 0.2f, 1.5f, 3f);
        //powerupChance = Mathf.Clamp(powerupChance + 0.01f, 0.20f, 0.33f);
        StartCoroutine(IncreaseMultiplyer(0.04f * delay * delay + 1f + delay));
    }

    public IEnumerator Shake(float duration, float magnitude) {
        // shakes the camera
        float elapsed = 0.0f;
        Vector3 originalCamPos = Camera.main.transform.position;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);
            yield return null;
        }

        Camera.main.transform.position = originalCamPos;
    }

    // Use this for initialization
    void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
            return;
        }

        //fill screen with gui texture
        GetComponent<GUITexture>().pixelInset = new Rect(0, 0, Screen.width, Screen.height);
        myGUIText = this.GetComponent<GUIText>();
        myGUIText.pixelOffset = new Vector2(Screen.width - 30f, Screen.height - 15f);
        myGUIStyle = new GUIStyle();
        myGUIStyle.fontStyle = myGUIText.fontStyle;
        myGUIStyle.fontSize = myGUIText.fontSize;
        myGUIStyle.font = myGUIText.font;
        myGUIStyle.normal.textColor = Color.white;
        pc = player.GetComponent<PlayerController>();
        StartCoroutine(IncreaseMultiplyer(8f));
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Menu")) {
            if (canPause) {
                paused = togglePause();
                canPause = false;
                Invoke("resetPause", 0.05f);
            }
        }

        if (Input.GetButtonUp("Menu")) {
            canPause = true;
        }

        if (Input.GetButtonDown("Mute")) {
            AudioListener.pause = !AudioListener.pause;
        }
    }

    void FixedUpdate() {

        if (sceneStarting)
            StartScene();
        if (sceneEnding) {
            EndScene();
        }
    }

    void OnGUI() {
        if (paused) {
            GUILayout.Label("Game is paused!");
            if (GUILayout.Button("Unpause"))
                paused = togglePause();
        }
        else if (!gameOver) {
            //in game ui displays
        }
        else {
            // game over display
        }
    }

    bool togglePause() {// lets the player pause the game
        if (Time.timeScale == 0f) {
            Time.timeScale = 1f;
            return (false);
        }
        else {
            Time.timeScale = 0f;
            return (true);
        }
    }

    //Fading Functions
    void OnLevelWasLoaded() {
        sceneStarting = true;
        StartScene();
    }

    void FadeToClear() {
        // Lerp the colour of the texture between itself and transparent.
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack() {
        // Lerp the colour of the texture between itself and black.
        GetComponent<GUITexture>().color = Color.Lerp(GetComponent<GUITexture>().color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene() {
        // Fade the texture to clear.
        FadeToClear();
        // If the texture is almost clear...
        if (GetComponent<GUITexture>().color.a <= 0.02f) {
            // ... set the colour to clear and disable the GUITexture.
            GetComponent<GUITexture>().color = Color.clear;
            GetComponent<GUITexture>().enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene() {
        // Make sure the texture is enabled.
        GetComponent<GUITexture>().enabled = true;

        // Start fading towards black.
        FadeToBlack();
        if (GetComponent<GUITexture>().color.a >= 0.95f) {
            // ... set the colour to clear and disable the GUITexture.
            GetComponent<GUITexture>().color = Color.black;
            sceneEnding = false;
        }
    }

    public void GameOver() {
        if (!gameOver) {
            gameOver = true;
            AudioSource.PlayClipAtPoint(gameoverSound, this.transform.position);
            sceneEnding = true;
            FadeToBlack();
            myGUIText.anchor = TextAnchor.MiddleCenter;
            myGUIText.fontSize = 60;
            myGUIText.pixelOffset = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Invoke("RestartGame", 5f);
        }
    }

    void RestartGame() {
        SceneManager.LoadScene("MainMenu");
        Destroy(this.gameObject);
    }
}