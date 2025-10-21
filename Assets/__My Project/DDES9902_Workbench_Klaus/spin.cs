using UnityEngine;

public class StartRotate : MonoBehaviour
{
    public float rotateSpeed = 100f; 
    private bool isRotating = false; 

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
        }
    }


    public void ToggleRotation()
    {
        isRotating = !isRotating; 
    }
}