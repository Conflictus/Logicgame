using UnityEngine;

public class MatchCameraSize : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        transform.position = new Vector3(0f, 0f, transform.position.z);
        if (mainCamera != null)
        {
            // Устанавливаем размер дочернего объекта
            SetSizeToCoverCamera();
        }
        else
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void SetSizeToCoverCamera()
    {
        // Устанавливаем размер объекта равным размеру камеры
        transform.localScale = new Vector3(mainCamera.orthographicSize * 2f * mainCamera.aspect, mainCamera.orthographicSize * 2f, 1f);
    }

    void Update()
    {
        // Обновляем размер при изменении размера камеры (если это необходимо)
        // SetSizeToCoverCamera();
    }
}