using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public AnimationSpeedController animationSpeedController;
    public BGMManager bGMManager;
    bool isGame;

    public static GameManager Instance { get; private set; }

    private float initialGameSpeed = 1f;
    private float gameSpeedIncrease = 0.2f;
    public float gameSpeed { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button retryButton;

    public Player player;
    public Spawner spawner;

    // �ν����Ϳ��� ��ֹ��� �ر��� ��ǥ ������ �����մϴ�.
    // ��: 100��, 300��, 700�� ...
    [SerializeField] private float[] scoreThresholds;
    private int unlockedCount; // ���� �رݵ� ��ֹ� ������ ��

    private float score;
    public float Score => score;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }

        Application.targetFrameRate = 60;
        scoreThresholds = new float[] { 10f, 50f, 100f, 150f, 200f ,250f, 300f };
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        //player = FindObjectOfType<Player>();
        //spawner = FindObjectOfType<Spawner>();

        //NewGame();
    }

    public void startFirst()
    {
        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        NewGame();
    }

    public void NewGame()
    {    
        // �ر� ���� �ʱ�ȭ (�⺻ 1���� �رݵ� ���·� ����)
        unlockedCount = 1;
        spawner.SetUnlockedCount(unlockedCount);

        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        UpdateHiscore();

        bGMManager.NewAudioPlay();
        isGame = true;

        animationSpeedController.ResetAcceleration();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        isGame = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        UpdateHiscore();
    }

    private void Update()
    {
        if (isGame)
        {
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
            score += gameSpeed * Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString("D5");

            // ���� �ر��� ��ֹ��� �ְ�, ���� ������ ��ǥ ������ �Ѿ����� Ȯ��
            if (unlockedCount < spawner.objects.Length && score >= scoreThresholds[unlockedCount - 1])
            {
                // ��ֹ� ������ �ϳ� �� �ر�
                unlockedCount++;
                spawner.SetUnlockedCount(unlockedCount);
            }
        }
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

}
