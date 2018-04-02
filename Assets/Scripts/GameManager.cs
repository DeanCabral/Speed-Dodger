using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private GenerateObstacles GO;
    private PlayerMovement PM;
    public CanvasGroup GameOverScreen;
    public CanvasGroup WaveCompleteScreen;
    public GameObject playerObject;
    public Image health1, health2, health3;
    public Material normalState, damagedState;
    public Text currentWave;
    public Text currentTime;
    public Text deathWave;
    public Text waveText;
    public Text livesText;
    public bool IsPaused = false;
    public bool IsDead = false;
    public int timer = 0;
    public int wave = 1;
    public int lives = 3;

    void Awake()
    {
	// Keep game manager alive indefinitely
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start () {

        GO = FindObjectOfType<GenerateObstacles>();
        PM = FindObjectOfType<PlayerMovement>();

        StartCoroutine(UpdateTime());
	}
	
	// Update is called once per frame
	void Update () {

        UpdateUI();
        CheckDeath();
	}

    private void UpdateUI()
    {
        if (wave < 10)
        {
            currentWave.text = "Wave 0" + wave.ToString();
            deathWave.text = "LASTED 0" + wave + " WAVES";
            waveText.text = "WAVE 0" + wave;

        }
        else
        {
            currentWave.text = "Wave " + wave.ToString();
            deathWave.text = "LASTED " + wave + " WAVES";
            waveText.text = "WAVE " + wave;
        }

        currentTime.text = timer + " sec";
        livesText.text = lives + " Remaining Lives";
    }

    private void CheckDeath()
    {
        if (IsDead)
        {
            if (!IsPaused)
            {
                IsPaused = true;
                StartCoroutine(FadeIn(GameOverScreen));
            }            
        }
    }

    public void NextWave()
    {
        GO.Reset();
        NextWaveScreen();
        wave++;

        if (PM.forwardSpeed != 40f)
        {
            PM.forwardSpeed += 2.5f;
            PM.defaultSpeed = PM.forwardSpeed;

            if (wave % 2 == 0)
            {
                PM.turnSpeed += 2.5f;
            }
        }

        if (wave > 10 && RenderSettings.fogDensity < 0.03f)
        {
            RenderSettings.fogDensity += 0.01f;
        }        
    }

    private void NextWaveScreen()
    {
        StartCoroutine(FadeIn(WaveCompleteScreen));        
    }

    private void ResetData()
    {
        IsPaused = false;
        IsDead = false;
        health3.gameObject.SetActive(true);
        health2.gameObject.SetActive(true);
        health1.gameObject.SetActive(true);
        timer = 0;
        wave = 1;
        lives = 3;
        StartCoroutine(UpdateTime());
        PM.turnSpeed = 15f;
        PM.forwardSpeed = 20f;
        PM.defaultSpeed = PM.forwardSpeed;
        PM.Respawn();
    }

    public void RestartGame()
    {
        StartCoroutine(FadeOut(GameOverScreen, true));
    }

    public IEnumerator DecreaseLife()
    {
        if (lives > 0)
        {
            lives -= 1;
            StartCoroutine(FlashDamage());

            if (health3.IsActive()) health3.gameObject.SetActive(false);
            else if (health2.IsActive()) health2.gameObject.SetActive(false);
            else health1.gameObject.SetActive(false);
        }        
        else IsDead = true;
        yield return new WaitForSeconds(1);
    }

    IEnumerator UpdateTime()
    {        
        yield return new WaitForSeconds(1);
        timer++;
        StartCoroutine(UpdateTime());
    }

    IEnumerator FlashDamage()
    {
        playerObject.GetComponent<Renderer>().material = damagedState;
        yield return new WaitForSeconds(0.3f);
        playerObject.GetComponent<Renderer>().material = normalState;
    }

    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float time = 1f;
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }

        StartCoroutine(FadeOut(WaveCompleteScreen, false));
    }

    IEnumerator FadeOut(CanvasGroup canvasGroup, bool IsRestart)
    {
        float time = 0.8f;

        if (IsRestart)
        {
            ResetData();
        }
        else time = 3f;

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }  

    }    

}
