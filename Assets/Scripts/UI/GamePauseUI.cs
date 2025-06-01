using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private GameObject gamePauseUI;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private bool isGamePaused = false;
    private void Awake()
    {
        Time.timeScale = 1f;
        gamePauseUI.SetActive(false);
    }
    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;

        restartButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void GameInput_OnPause(object sender, System.EventArgs e)
    {
        ShowHide();
    }

    private void ShowHide()
    {
        gamePauseUI.SetActive(!gamePauseUI.activeSelf);
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }else
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
