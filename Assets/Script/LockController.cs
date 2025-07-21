using UnityEngine;

public class LockController : MonoBehaviour
{
    public Transform keySocket;
    public GameObject doorObject;
    private Vector3 doorOriginalPosition;
    private Quaternion doorOriginalRotation;
    private bool doorIsActive = true;

    void Start()
    {
        // Сохраняем оригинальное состояние двери
        if (doorObject != null)
        {
            doorOriginalPosition = doorObject.transform.position;
            doorOriginalRotation = doorObject.transform.rotation;
        }
    }

    public void KeyInserted()
    {
        // Скрываем дверь вместо удаления
        if (doorObject != null && doorIsActive)
        {
            doorObject.SetActive(false);
            doorIsActive = false;
        }

    }

    public void KeyRemoved()
    {
        // Показываем дверь обратно
        if (doorObject != null && !doorIsActive)
        {
            doorObject.SetActive(true);
            doorObject.transform.position = doorOriginalPosition;
            doorObject.transform.rotation = doorOriginalRotation;
            doorIsActive = true;
        }
    }
}