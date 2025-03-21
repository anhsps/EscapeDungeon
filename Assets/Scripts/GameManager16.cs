using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

public class GameManager16 : Singleton<GameManager16>
{
    public static int level;
    [SerializeField] private TextMeshProUGUI lvText;
    [SerializeField] private GameObject nextBtn_win;
    [SerializeField] private GameObject winMenu, loseMenu, pauseMenu;
    [SerializeField] private RectTransform winPanel, losePanel, pausePanel;
    [SerializeField] private float topPosY = 250f, middlePosY, tweenDuration = 0.3f;
    private int maxLV = 15;

    [SerializeField] private TextMeshProUGUI scoreText, scoreText2;
    private int score;

    /*[Header("Grid")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject[] gridPrefabs;*/

    protected override void Awake()
    {
        base.Awake();
        level = PlayerPrefs.GetInt("CurrentLevel", 1);
        LoadLevel(level);
    }

    async void Start()
    {
        if (scoreText && scoreText2) UpdateScore(LoadBest());

        await HidePanel(winMenu, winPanel);
        await HidePanel(loseMenu, losePanel);
        await HidePanel(pauseMenu, pausePanel);
    }

    private void LoadLevel(int levelIndex)
    {
        if (levelIndex < 1 || levelIndex > maxLV) levelIndex = 1;

        if (levelIndex == maxLV && nextBtn_win) nextBtn_win.SetActive(false);

        PlayerPrefs.SetInt("CurrentLevel", levelIndex);

        if (lvText) lvText.text = "LEVEL " + (levelIndex < 10 ? "0" + levelIndex : levelIndex);

        //if (gridPrefabs.Length > 0) CreateGrid(levelIndex);
    }

    /*private void CreateGrid(int levelIndex)
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        if (gridPrefabs[levelIndex - 1] != null)
            Instantiate(gridPrefabs[levelIndex - 1], gridParent);
    }*/

    public void Home() => SceneManager.LoadScene("Home");
    //public void StartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void NextLV()
    {
        PlayerPrefs.SetInt("CurrentLevel", level + 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("1");//
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PauseGame() => OpenMenu(pauseMenu, pausePanel, 1);

    public async void ResumeGame()
    {
        SoundManager16.Instance.SoundClick();
        await HidePanel(pauseMenu, pausePanel);
        Time.timeScale = 1f;
    }

    public void GameWin()
    {
        UnlockNextLevel();
        OpenMenu(winMenu, winPanel, 2);
    }

    public void GameLose() => OpenMenu(loseMenu, losePanel, 3);

    private void OpenMenu(GameObject menu, RectTransform panel, int soundIndex)
    {
        SoundManager16.Instance.PlaySound(soundIndex);
        ShowPanel(menu, panel);
    }

    public void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (level >= unlockedLevel && level < maxLV)
            PlayerPrefs.SetInt("UnlockedLevel", level + 1);
    }

    public void SetCurrentLV(int levelIndex)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("1");
        //SceneManager.LoadScene(levelIndex.ToString());
    }

    private void ShowPanel(GameObject menu, RectTransform panel)
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
        menu.GetComponent<CanvasGroup>().DOFade(1, tweenDuration).SetUpdate(true);
        panel.DOAnchorPosY(middlePosY, tweenDuration).SetUpdate(true);
    }

    private async Task HidePanel(GameObject menu, RectTransform panel)
    {
        if (menu == null || panel == null) return;

        menu.GetComponent<CanvasGroup>().DOFade(0, tweenDuration).SetUpdate(true);
        await panel.DOAnchorPosY(topPosY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
        if (menu) menu.SetActive(false);
    }

    public void HackGame()
    {
        GameWin();
    }
    public void HackCoin()
    {
        IncreaseScore(100);
    }

    // score
    public void IncreaseScore(int point) => UpdateScore(score + point);

    private void UpdateScore(int score)
    {
        this.score = score;
        if (score != LoadBest())
            PlayerPrefs.SetInt("best", score);
        scoreText.text = LoadBest().ToString();
        scoreText2.text = LoadBest().ToString();
    }

    private int LoadBest() => PlayerPrefs.GetInt("best", 0);

    public async void ContinueGame()
    {
        if (LoadBest() >= 20)
        {
            IncreaseScore(-20);

            SoundManager16.Instance.SoundClick();
            await HidePanel(loseMenu, losePanel);
            Time.timeScale = 1f;

            Player16 player = FindObjectOfType<Player16>();
            player.Revive();
        }
    }
}
