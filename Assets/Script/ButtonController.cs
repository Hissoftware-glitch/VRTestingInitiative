using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [Header("Settings")]
    public GameObject objectToSpawn; // Префаб объекта для спавна
    public Transform spawnPoint;     // Позиция спавна объекта
    public float pressDistance = 0.1f; // Дистанция нажатия кнопки
    public float pressSpeed = 0.1f;    // Скорость нажатия
    public AudioClip buttonSound;

    private bool isPressed = false;    // Флаг нажатия
    private Vector3 initialPosition;   // Исходная позиция кнопки
    private AudioSource audioSource;   // Звук нажатия

    void Start()
    {
        initialPosition = transform.position;

        // Добавляем компонент звука (если нужен)
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем что кнопка еще не нажата и объект имеет нужный тег
        if (!isPressed && collision.gameObject.CompareTag("KeyObject"))
        {
            // Активируем флаг нажатия
            isPressed = true;

            // Запускаем анимацию нажатия
            StartCoroutine(PressAnimation());

            // Спавним объект
            Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }

    System.Collections.IEnumerator PressAnimation()
    {
        // Проигрываем звук (если назначен)
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }

        // Анимация нажатия вниз
        Vector3 targetPosition = initialPosition - Vector3.up * pressDistance;

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                pressSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}