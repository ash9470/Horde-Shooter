using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int kills = 0;
    public int level = 1;
    public EnemySpawner spawner;
    public WeaponSwitcher weaponSwitcher;
    public UIManager ui;
    public bool isLive;
    public Transform player;
    public Player _player;
    public int playerId;
    public float health;
    public float maxHealth = 100;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (ui) ui.UpdateKills(kills);
        if (spawner == null) spawner = FindObjectOfType<EnemySpawner>();
    }

    public void RegisterKill()
    {
        kills++;
        if (ui) ui.UpdateKills(kills);
        if (kills % 20 == 0) LevelUp();
    }

    void LevelUp()
    {
        level++;
        if (ui) ui.UpdateLevel(level);
        // example: increase spawn difficulty
        if (spawner != null) spawner.enemiesPerWave += 20;
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;

    }

    public void UpdateHeathUI()
    {
        float curHealth = health;
        float _maxHealth = maxHealth;
        ui.UpdateHealthUI(curHealth / _maxHealth);
    }


    public void StartGame()
    {
        ResetData();
        player.gameObject.SetActive(true);
        isLive = true;
        health = maxHealth;
        UpdateHeathUI();
        spawner.StartGame();
        weaponSwitcher.StartGame();
    }



    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    public void ResetData()
    {
        kills = 0;
        ui.UpdateKills(kills);
        level = 0;
        ui.UpdateLevel(level);

    }


    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        Stop();

    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        player.gameObject.SetActive(false);
        if (ui) ui.ShowGameOver();
        // uiJoy.localScale = Vector3.zero;
    }

}
