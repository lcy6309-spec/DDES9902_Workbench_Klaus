using UnityEngine;

public class DoorMove : MonoBehaviour
{
    public Transform door;       
    public float moveAmount = 1f; 

    private Vector3 originalPos;
    private bool isUp = false;

    void Start()
    {
        if (door != null)
            originalPos = door.position;
    }

    public void ToggleDoor()
    {
        if (door == null) return;

        if (!isUp)
        {
            door.position = originalPos + Vector3.up * moveAmount;
            isUp = true;
        }
        else
        {
            door.position = originalPos;
            isUp = false;
        }
    }
}
