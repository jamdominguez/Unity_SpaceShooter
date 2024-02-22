using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public float hazardSpawn;
    public int hazardCount;
    public float startWait;
    public float waveSpawn;
    public Vector3 spawnValues;
    public Text scoreText;
    public Text gameOverText;
    public Text restartText;
    public AudioClip gameOverClip;
    

    private float waveNumber;
    private int score;
    private int level;
    private bool playerIsAlive;
    private float initialAsteroidsSpeed;
    private bool isGameOver = false;

    //Constants
    private int POINTS_BY_DESTROY = 10;
    private float INCRASE_HAZARD_SPEED = 1.5f;
    private int INCRASE_HAZARD_COUNT = 5;
    private float DECRASE_WAVE_SPAWN = -0.5f;
    private float DECRASE_HAZARD_SPAWN = -0.1f;

    //public GameObject Hazard { get => hazards; set => hazards = value; }


    // Use this for initialization
    void Start () {
        updateTexts();
        this.score = 0;
        this.playerIsAlive = true;
        //this.initialAsteroidsSpeed = this.Hazard.GetComponent<Mover>().speed;
        InitHazard();
        UpdateScoreText();
        NotificationCenter.DefaultCenter().AddObserver(this, "AsteroidDeleted");
        NotificationCenter.DefaultCenter().AddObserver(this, "PlayerDeleted");
        
        StartCoroutine(SpawnWave());        
	}

    private void Update() {
        if (isGameOver && Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void updateTexts() {
        gameOverText.gameObject.SetActive(isGameOver);
        restartText.gameObject.SetActive(isGameOver);
    }

    IEnumerator SpawnWave() {
        yield return new WaitForSeconds(startWait);

        while (this.playerIsAlive) {            
            for (int i = 0; i < hazardCount; i++) {
                float xSpawn =  Random.Range(-spawnValues.x, spawnValues.x);            
                Vector3 spawnPosition = new Vector3(xSpawn, spawnValues.y, spawnValues.z);
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Instantiate(hazard, spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(hazardSpawn);
            }
            this.waveNumber++;
            UpdateLevel();
            UpdateScoreText();
            yield return new WaitForSeconds(waveSpawn);
        }
    }

    void PlayerDeleted() {
        this.playerIsAlive = false;
        //this.Hazard.GetComponent<Mover>().speed = initialAsteroidsSpeed;
        isGameOver = true;
        updateTexts();
        AudioSource audio = GetComponent<AudioSource>();
        audio.Stop();
        audio.clip = this.gameOverClip;        
        audio.Play();
    }

    void AsteroidDeleted() {
        this.score += POINTS_BY_DESTROY;
        UpdateScoreText();
    }

    //Inicializa los valores de las variables relacionadas con los enemigos
    void InitHazard() {
        this.level = 1;
        this.hazardCount = 10;
        this.hazardSpawn = 0.5f;
        this.waveSpawn = 3;
        this.waveNumber = 1;
    }

    //Actualiza la puntuacion
    void UpdateScoreText() {
        this.scoreText.text = "SCORE: " + this.score + "\n  WAVE: " + this.waveNumber;
    }

    //Actualiza la dificultad
    void UpdateLevel() {
        int newLevel = 1;
        if (this.waveNumber >= 20 && this.level == 4) newLevel = 5;
        if (this.waveNumber > 10 && this.level == 3) newLevel = 4;    
        if (this.waveNumber >= 5 && this.level == 2) newLevel = 3;
        if (this.waveNumber >= 3 && this.level == 1) newLevel = 2;

        bool updateHazard = newLevel > this.level;

        if (updateHazard) {
            this.hazardCount += INCRASE_HAZARD_COUNT;
            this.hazardSpawn += DECRASE_HAZARD_SPAWN;
            this.waveSpawn += DECRASE_WAVE_SPAWN;
            //this.Hazard.GetComponent<Mover>().speed += INCRASE_HAZARD_SPEED * (-1);//Direccion negativa
            //Debug.Log("Asteroid Speed increase to: + " + this.Hazard.GetComponent<Mover>().speed);
            this.level = newLevel;
        }
    }

}
