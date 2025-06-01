using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        Hide();
        playerHealth.OnPlayerDeath += PlayerHealth_OnPlayerDeath;

        restartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Debug.Log("Go to main menu");
        });
    }

    private void PlayerHealth_OnPlayerDeath(object sender, System.EventArgs e)
    {
        Show();
        killsText.text = "KILLS: " + BattleSystem.Instance.GetKillCount();
        Cursor.lockState = CursorLockMode.None;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
