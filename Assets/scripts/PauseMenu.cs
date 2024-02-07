using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject backgroundObject; // Ссылка на объект фона

    private bool isPaused = false;
    private GameObject originalBackground; // Сохранение оригинального фона
    float amount1, amount2, amountToMeasure;
    public TMP_Text pauseMenuText;
    GameManager gameManager;
void Start()
    {
        gameManager = FindObjectOfType<GameManager>();;
        originalBackground = backgroundObject;
        Resume();
        Invoke("GetAmounts", 0.1f);
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    void GetAmounts() {
       amount1 =  gameManager.amount1;
       amount2 =  gameManager.amount2;
       amountToMeasure =  gameManager.amountToMeasure;
       pauseMenuText.text = "Вам необходимо отмерить " + amountToMeasure + " из ведер объемом " + amount2 + " и " + amount1 + ". Для выбора ведра нажмите ЛКМ по необходимому ведру, затем, в зависимости от области нажатия, будут происходить перелив воды из одного ведра в друге, если кликнуть по нему. Если снова кликнуть по тому же ведру, оно наполниться до краев. Если кликнуть по пустому месту, то оно выльет все содержимое.";
    }
    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        // Деактивируем меню паузы
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Возвращаем оригинальный фон на место
        originalBackground.SetActive(false);
    }

    void Pause()
    {
        // Активируем меню паузы
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Отключаем оригинальный фон
        originalBackground.SetActive(true);
    }
}
