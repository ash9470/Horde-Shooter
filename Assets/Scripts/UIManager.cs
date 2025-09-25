using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text killsText;
    public TMP_Text levelText;
    public Slider playerHealthSlider;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public Button retyBtn;
    public Button mainMenuBtn;
    [Header("Main Menu")]
    public GameObject mainMenuPanel;
    public Button play;
    public Button quit;

    [Header("Controlls")]
    public GameObject controllsPanel;
    public Button ControllBtn;
    public Button CloseBtn;



    private void Start()
    {
        play.onClick.AddListener(Play);
        quit.onClick.AddListener(Application.Quit);
        retyBtn.onClick.AddListener(Retry);
        mainMenuBtn.onClick.AddListener(MainMenu);
        ControllBtn.onClick.AddListener(() => ControllPanel(true));
        CloseBtn.onClick.AddListener(() => ControllPanel(false));
    }

    void ControllPanel(bool Value)
    {
        controllsPanel.SetActive(Value);
    }


    public void UpdateKills(int val)
    {
        if (killsText) killsText.text = "" + val;
    }

    public void UpdateLevel(int val)
    {
        if (levelText) levelText.text = "Level: " + val;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel) gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1;
        gameOverPanel.SetActive(false);
        GameManager.Instance.spawner.ClearAllEnemies();
        GameManager.Instance.StartGame();
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.spawner.ClearAllEnemies();
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void Play()
    {
        mainMenuPanel.SetActive(false);
        GameManager.Instance.StartGame();
    }


    public void UpdateHealthUI(float health)
    {
        playerHealthSlider.value = health;

    }
}
