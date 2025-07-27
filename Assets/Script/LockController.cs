using UnityEngine;

public class LockController : MonoBehaviour
{
    public Transform keySocket;
    public GameObject rightDoorObject;
    public GameObject leftDoorObject;
    private Vector3 rightDoorOriginalPosition;
    private Vector3 leftDoorOriginalPosition;
    private Quaternion leftDoorOriginalRotation;
    private Quaternion rightDoorOriginalRotation;
    private bool leftDoorIsActive = true;
    private bool rightDoorIsActive = true;

    void Start()
    {
        // Сохраняем оригинальное состояние двери
        if (leftDoorObject != null && rightDoorObject != null)
        {
            leftDoorOriginalPosition = leftDoorObject.transform.position;
            leftDoorOriginalRotation = leftDoorObject.transform.rotation;

            rightDoorOriginalPosition = rightDoorObject.transform.position;
            rightDoorOriginalRotation = rightDoorObject.transform.rotation;

        }
    }

    public void KeyInserted()
    {
        // Скрываем дверь вместо удаления
        if ((leftDoorObject != null && leftDoorIsActive) && (rightDoorObject != null && rightDoorIsActive))
        {
            leftDoorObject.SetActive(false);
            leftDoorIsActive = false;

            rightDoorObject.SetActive(false);
            rightDoorIsActive = false;
        }

    }

    public void KeyRemoved()
    {
        // Показываем дверь обратно
        if ((leftDoorObject != null && !leftDoorIsActive) && (rightDoorObject != null && !rightDoorIsActive))
        {
            leftDoorObject.SetActive(true);
            leftDoorObject.transform.position = leftDoorOriginalPosition;
            leftDoorObject.transform.rotation = leftDoorOriginalRotation;
            leftDoorIsActive = true;

            rightDoorObject.SetActive(true);
            rightDoorObject.transform.position = rightDoorOriginalPosition;
            rightDoorObject.transform.rotation = rightDoorOriginalRotation;
            rightDoorIsActive = true;
        }
    }
}