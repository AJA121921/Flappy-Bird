using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Player player;

    public Text scoreText;

    public Text highScoreText;

    public GameObject playButton;

    public GameObject gameOver;

    public Pipes pipesPrefab;

    private int score;

    private int highScore;

    private Color[] backgroundColors;

    private int currentBackgroundColorIndex;

    private Color initialBackgroundColor;

    private float initialPipeSpeed = 5f;

    private float currentPipeSpeed;

    private float initialGravity;

    private float initialSpawnRate = 1.5f;

    private float currentSpawnRate;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateHighScoreText();

        backgroundColors = new Color[]
        {
            new Color(0.3294118f, 0.7529413f, 0.7882354f),
            new Color(0.2851549f, 0.5444313f, 0.5660378f),
            new Color(0.155073f, 0.3266262f, 0.3396226f),
            new Color(0.03047348f, 0.1406906f, 0.1509434f),
        };

        currentBackgroundColorIndex = 0;
        initialBackgroundColor = Camera.main.backgroundColor;
        UpdateBackgroundColor();

        currentPipeSpeed = initialPipeSpeed;
        
        initialGravity = player.gravity;

    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);

        highScoreText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;


        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for(int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

    }

     private void SpawnPipes()
    {
        Pipes pipes = Instantiate(pipesPrefab, transform.position, Quaternion.identity);
        pipes.speed = currentPipeSpeed;

         if (score > 0 && score % 50 == 0)
        {
            currentSpawnRate *= 0.5f;
            CancelInvoke("SpawnPipes");
            InvokeRepeating("SpawnPipes", 0f, initialSpawnRate);
        }

    }


    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            UpdateHighScoreText();
        }

        highScoreText.gameObject.SetActive(true);
        ResetBackgroundColor();
        ResetGravity(); 
        Pause();
    }

    private void ResetGravity()
    {
        player.gravity = initialGravity;
    }

    private void ResetBackgroundColor()
    {
        Camera.main.backgroundColor = initialBackgroundColor;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();

        if (score % 50 == 0)
        {
            currentBackgroundColorIndex = (currentBackgroundColorIndex + 1) % backgroundColors.Length;
            UpdateBackgroundColor();
            AdjustGravity();
            IncreasePipeSpeed();
        }

    }

    private void IncreasePipeSpeed()
    {
        currentPipeSpeed += 1.0f; 
    }

private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void UpdateBackgroundColor()
    {
        Camera.main.backgroundColor = backgroundColors[currentBackgroundColorIndex];
    }

    private void AdjustGravity()
    {
        player.gravity = -9.8f - (currentBackgroundColorIndex * 5f); 
    }
}
