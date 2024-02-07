using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WinController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinCanvas;
    public GameObject backgroundObject; // Ссылка на объект фона

    private bool isPaused = false;
    private GameObject originalBackground; // Сохранение оригинального фона
    float amount1, amount2, amountToMeasure;
    public TMP_Text pauseMenuText;
    GameManager gameManager;
void Start()
    {
        pauseMenuText.text = "Вы выполнили условие!";
        gameManager = FindObjectOfType<GameManager>();;
        originalBackground = backgroundObject;
        Resume();
        
    }
    void Update()
    {
        TogglePause();
    }
    public void TogglePause()
    {
        if (gameManager.winChek == true) {
            Invoke("Pause", 3f);
        }
            
    }

    public void Resume()
    {
        // Деактивируем меню паузы
        WinCanvas.SetActive(false);
        Time.timeScale = 1f;
        originalBackground.SetActive(false);
    }

    void Pause()
    {
        // Активируем меню паузы
        WinCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        originalBackground.SetActive(true);
    }
    public void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
