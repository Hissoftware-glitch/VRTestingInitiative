using UnityEngine;
using UnityEngine.InputSystem;

public class Something : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Сработал метод awake");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Сработал метод start");
    }


    public void Move(InputAction.CallbackContext ctx)
    {
        var moveDirection = ctx.ReadValue<Vector2>();
        Debug.Log(moveDirection);
        transform.position = new Vector3(transform.position.x + moveDirection.x, transform.position.y, transform.position.z + moveDirection.y);
    }

}
