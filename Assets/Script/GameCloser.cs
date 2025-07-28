using UnityEngine;
using UnityEngine.Video;
using System;
using System.Runtime.InteropServices;
using System.Collections;

public class GameCloserAfterVideo : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public VideoClip closingVideoClip;

    [Header("Exit Settings")]
    public float delayAfterVideo = 2f;
    public bool showSystemMessage = true;
    [SerializeField] public string messageTitle = "ОШИБКА 0xE3A7B";
    [SerializeField] public string messageContent = " Нарушение целостности данных \nДоступ к следующей тестовой камере заблокирован.\nОбнаружены признаки вмешательства в систему...\nПРИЧИНА: Несанкционированный доступ к протоколам\nДЕЙСТВИЕ: Инициирование протокола изоляции\nПриложение будет немедленно закрыто.";

    // Импорты WinAPI для системных сообщений
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // Константы для стилей сообщения
    private const uint MB_ICONERROR = 0x00000010; // Иконка ошибки (красный крест)
    private const uint MB_SYSTEMMODAL = 0x00001000; // Системное модальное окно

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                Debug.LogError("GameCloser: No VideoPlayer component found!");
                return;
            }
        }

        // Назначаем видео и начинаем воспроизведение
        videoPlayer.isLooping = false;
        videoPlayer.clip = closingVideoClip;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
        Debug.Log("Video playback started");
    }

    private void OnVideoEnd(VideoPlayer source)
    {
        Debug.Log("Video finished playing");
        StartCoroutine(CloseGameRoutine());
    }

    private IEnumerator CloseGameRoutine()
    {
        // Задержка после видео
        yield return new WaitForSeconds(delayAfterVideo);

        // Показ системного сообщения
        if (showSystemMessage)
        {
            Debug.Log("Showing system message with error icon");
            ShowSystemMessage();

            // Даем время на взаимодействие с сообщением
            yield return new WaitForSeconds(0f);
        }

        QuitGame();
    }

    private void ShowSystemMessage()
    {
        try
        {
            // Показываем системное окно с иконкой ошибки
            MessageBox(IntPtr.Zero,
                messageContent,
                messageTitle,
                MB_ICONERROR | MB_SYSTEMMODAL);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to show system message: " + e.Message);
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quitting application...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnDestroy()
    {
        // Отписываемся от события
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}